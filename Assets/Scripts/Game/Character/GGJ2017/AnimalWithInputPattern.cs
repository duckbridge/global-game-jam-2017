using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWithInputPattern : DispatchBehaviour {

	public float restartTime = 5f;
	public int attemptsAllowed = 1;

	public int currencyGainOnCorrect = 100;
	public int currencyLossOnCorrect = 50;

	public float moveSpeed = 2f;
	public List<InputPatternInfo> inputPatternsInfo;

	protected bool isHappy = true;

	protected List<InputPattern> playerPatterns;
	protected List<InputPattern> myPatterns;

	private int frameCounter = 0;
	private InputPattern currentAutoPlayInputPattern;
	private bool isStarted = false;

	private float previousTimeSinceStartup;
	private float deltaTime = 0;
	private float elapsedTime;

	private QueuePosition currentQueuePosition;
	protected AnimationManager2D animationManager;

	private ParticleSystem particleSystem;

	protected int amountOfAttempts = 0;
	private TriggerListener inRangeTrigger;
	private bool playerInTrigger = false;

	private Vector3? originalScale = null;

	// Use this for initialization
	public virtual void Awake () {
		animationManager = GetComponentInChildren<AnimationManager2D> ();
		inRangeTrigger = this.transform.Find ("PlayerInRangeTrigger").GetComponent<TriggerListener>();
		inRangeTrigger.GetComponent<Collider> ().enabled = false;

		particleSystem = this.transform.Find ("CakParticles").GetComponent<ParticleSystem> ();
	}

	void Start() {
		Invoke ("CreatePatterns", .5f);
	}
		
	protected void CreatePatterns() {
		InputPatternFactory ipf = SceneUtils.FindObject<InputPatternFactory> ();

		playerPatterns = ipf.CreateInputPatterns (inputPatternsInfo);
		myPatterns = ipf.CopyInputPatterns (playerPatterns);
	}

	public void StartPattern() {
		if (playerInTrigger) {
			frameCounter = 0;
			elapsedTime = 0;
			isStarted = true;
		}
	}

	public void OnListenerTrigger(Collider coll) {
		MainPlayer mainPlayer = coll.gameObject.GetComponent<MainPlayer> ();
		if (mainPlayer) {
			playerInTrigger = true;

			mainPlayer.GetComponent<PlayerInputPatternManager> ().AddEventListener (this.gameObject);
			mainPlayer.GetComponent<PlayerInputPatternManager> ().SetInputPatterns (playerPatterns);
			StartPattern ();
		}
	}

	public void OnListenerTriggerExit(Collider coll) {
		MainPlayer mainPlayer = coll.gameObject.GetComponent<MainPlayer> ();
		if (mainPlayer) {
			playerInTrigger = false;

			this.animationManager.PlayAnimationByName ("Idle", true);
			mainPlayer.GetComponent<PlayerInputPatternManager> ().RemoveEventListener (this.gameObject);
			mainPlayer.GetComponent<PlayerInputPatternManager> ().SetInputPatterns (null);
			StopPattern ();
		}
	}

	public virtual void OnInputPatternCorrect(PlayerInputPatternManager playerInputPatternManager) {
		OnDoneInQueue ();
		Logger.Log ("HAPPY");
		this.isHappy = true;
		//do eat animation
		DispatchMessage ("OnFirstCustomerHelped", this);
	}

	public virtual void OnInputPatternInCorrect(PlayerInputPatternManager playerInputPatternManager) {
		amountOfAttempts++;

		if (amountOfAttempts >= attemptsAllowed) {
			OnDoneInQueue ();
			Logger.Log ("ANGRY");
			this.isHappy = false;
			//do angry animation
			DispatchMessage ("OnFirstCustomerHelped", this);
		} else {
			
		}
	}

	protected void OnDoneInQueue() {
		CancelInvoke ("TimeOutOfAnimal");
		SceneUtils.FindObject<PlayerInputPatternManager> ().RemoveEventListener (this.gameObject);
		SceneUtils.FindObject<PlayerInputPatternManager> ().SetInputPatterns (null);
		inRangeTrigger.RemoveEventListener (this.gameObject);
		inRangeTrigger.GetComponent<Collider> ().enabled = false;

		StopPattern ();
	}

	// Update is called once per frame
	void Update () {

		float realtimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realtimeSinceStartup - previousTimeSinceStartup;
		previousTimeSinceStartup = realtimeSinceStartup;

		elapsedTime += deltaTime;

		if (isStarted) {
			frameCounter++;

			currentAutoPlayInputPattern = InputPatternUtils.FindUnfinishedInputPatternByElapsedTime (myPatterns, (double)elapsedTime, true);
			if (currentAutoPlayInputPattern != null) {
				if (!currentAutoPlayInputPattern.IsFinished()) {
					currentAutoPlayInputPattern.UpdateFramesHeldBy (10);
					currentAutoPlayInputPattern.PlayOnce ();
					this.animationManager.PlayAnimationByName ("Talking", true);
					particleSystem.Emit (1);
				}
			}

			InputPattern lastPattern = myPatterns [myPatterns.Count - 1];
			if (elapsedTime >= (lastPattern.start + lastPattern.range)) {
				StopPattern ();
				Invoke ("StartPattern", restartTime);
			}
		}
	}

	protected void StopPattern() {
		CancelInvoke ("StartPattern");
		isStarted = false;
		ResetPattern ();
	}

	private void RestartPattern() {
		ResetPattern ();
		isStarted = true;
	}

	private void ResetPattern() {
		myPatterns.ForEach (ip => ip.ResetPattern ());
		frameCounter = 0;
		elapsedTime = 0;
	}

	public void MoveTo(QueuePosition queuePosition) {
		iTween.StopByName (this.gameObject, "Moving");

		if (currentQueuePosition) {
			currentQueuePosition.SetUnOccupied ();
		}

		this.currentQueuePosition = queuePosition;
		queuePosition.SetOccupied ();

		if (originalScale == null) {
			originalScale = this.transform.localScale;
		}

		if (queuePosition.transform.position.x > this.transform.position.x) {
			this.transform.localScale = originalScale.Value;
			this.particleSystem.transform.position = new Vector3 (particleSystem.transform.position.x, 20, particleSystem.transform.position.z);
		} else {
			this.transform.localScale = new Vector3 (-originalScale.Value.x, originalScale.Value.y, originalScale.Value.z);
			this.particleSystem.transform.position = new Vector3 (particleSystem.transform.position.x, 20, particleSystem.transform.position.z);
		}

		iTween.MoveTo (this.gameObject, new ITweenBuilder ()
			.SetName("Moving")
			.SetPosition (queuePosition.transform.position)
			.SetSpeed (moveSpeed)
			.SetOnCompleteTarget (this.gameObject)
			.SetEaseType(iTween.EaseType.linear)
			.SetOnComplete ("OnArrivedAtQueuePosition").Build ());

		this.animationManager.PlayAnimationByName ("Walking" + (isHappy ? "" : "-sad"), true);
	}

	public void SpawnFood() {
		StopPattern ();
		animationManager.PlayAnimationByName ("Eating", true);
		animationManager.DisableSwitchAnimations ();

		Invoke ("OnDoneEating", 1.5f);
	}

	protected virtual void OnDoneEating() {
		animationManager.EnableSwitchAnimations ();
		if (this.isHappy) {
			animationManager.PlayAnimationByName ("Happy", true);
			Invoke ("ResumeQueue", 1f);
		} else {
			ResumeQueue ();
		}
	}

	protected virtual void ResumeQueue() {
		MoveTo (SceneUtils.FindObject<ExitQueuePosition> ());
		DispatchMessage("MoveAllAnimals", null);
	}

	private void OnArrivedAtQueuePosition() {

		currentQueuePosition.OnArrived (this);

		if (currentQueuePosition.position == 0) {
			Logger.Log ("ITS MY TURN");
			Invoke ("TimeOutOfAnimal", SceneUtils.FindObject<PenguGameManager> ().animalMaxWaitTime);

			inRangeTrigger.AddEventListener (this.gameObject);
			inRangeTrigger.GetComponent<Collider> ().enabled = true;
			//do something
			this.animationManager.PlayAnimationByName ("Idle", true);
		} else {
			this.animationManager.PlayAnimationByName ("Idle", true);
		}
	}

	private void TimeOutOfAnimal() {
		OnDoneInQueue ();
		Logger.Log ("ANGRY");
		this.isHappy = false;
		SceneUtils.FindObject<QueueManager> ().UpdateCurrency (this);
		OnDoneEating ();
	}

	public QueuePosition GetCurrentQueuePosition() {
		return currentQueuePosition;
	}

	public bool IsHappy() {
		return isHappy;
	}

	public Transform GetHead() {
		return this.transform.Find("Head");
	}

	public ParticleSystem GetParticleSystem() {
		return particleSystem;
	}
}
