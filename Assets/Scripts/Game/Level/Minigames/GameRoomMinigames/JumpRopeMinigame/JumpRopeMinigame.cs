using UnityEngine;
using System.Collections;

public class JumpRopeMinigame : GameRoomMinigame {

	public CutSceneManager cutsceneMangerOnMinigameStart;

	public TextMesh pointDisplay;
	public int correctJumpsRequired = 5;
	public int minimumFrameToJumpOn, maximumFrameToJumpOn;
	public JumpRope jumpRope;

	private int currentCorrectJumps = 0;
	private int currentFrame = 0;
	private bool canPress = true;
	private bool hasPressedJump = false;
	private bool playerIsInsideJumpArea = false;

	private SoundObject onCorrectSound, onFailSound, musicToSing;
    private ParticleSystem singParticles;

   	private InteractionObjectListener interactionObjectListener;

	public override void Start () {
		base.Start ();

        singParticles = this.transform.Find("SingParticles").GetComponent<ParticleSystem>();

		onCorrectSound = this.transform.Find("Sounds/OnCorrectJumpSound").GetComponent<SoundObject>();
		onFailSound = this.transform.Find("Sounds/OnFailedJumpSound").GetComponent<SoundObject>();

        musicToSing = this.transform.Find("Sounds/MusicToSing").GetComponent<SoundObject>();

		SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, onCorrectSound.gameObject);
		SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, onFailSound.gameObject);

        interactionObjectListener = GetComponentInChildren<InteractionObjectListener>();
        interactionObjectListener.AddEventListener(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInteract(Player player) {
		if (canPress) {
			hasPressedJump = true;
			canPress = false;
		}
	}

	public override void OnTriggerEnter (Collider coll) {
		player = coll.gameObject.GetComponent<Player> ();
		if (player) {
			playerIsInsideJumpArea = true;
		}

		base.OnTriggerEnter (coll);
	}

	public override void OnTriggerExit (Collider coll) {
		player = coll.gameObject.GetComponent<Player> ();
		if (player) {
			playerIsInsideJumpArea = false;
		}

		base.OnTriggerExit (coll);
	}

	void FixedUpdate() {
		if(hasStarted) {
			currentFrame++;
			if(currentFrame == maximumFrameToJumpOn) {
				if (playerIsInsideJumpArea) {
					if (!hasPressedJump) {
						OnJumpingFailed ();
					} else {
						CancelInvoke ("ResetPressing");
						ResetPressing ();
					}
				}
			}
		}
	}

	public void OnAnimationDone(Animation2D animation2D) {
		if (animation2D.name.StartsWith ("JumpRope")) {
			if (currentFrame < minimumFrameToJumpOn && currentFrame > maximumFrameToJumpOn) {
				OnJumpingFailed ();
			} else {
				++currentCorrectJumps;
				onCorrectSound.Play ();
				pointDisplay.text = currentCorrectJumps + "/" + correctJumpsRequired;
				OnJumpingDone ();
			}
		}
	}

	private void OnJumpingDone() {
		ResetPressing ();
	}

	private void OnJumpingFailed() {
		currentCorrectJumps = 0;
		onFailSound.Play();

		player.GetComponent<PlayerInputComponent> ().enabled = false;
		player.PlayFailDanceAnimation();
		player.GetAnimationManager ().DisableSwitchAnimations ();

		pointDisplay.text = currentCorrectJumps + "/" + correctJumpsRequired;

		Invoke ("RecoverFromJumpFail", .25f);
	}

	private void RecoverFromJumpFail() {

		OnJumpingDone ();

		player.GetComponent<PlayerInputComponent> ().enabled = true;
		player.GetAnimationManager ().EnableSwitchAnimations ();
		player.transform.Find ("Shadow").GetComponent<SpriteRenderer> ().enabled = true;
	}
	
	private void ResetPressing() {
		canPress = true;
		hasPressedJump = false;
	}
	
	public override void StartMinigame () {
		pointDisplay.text = "0/" + correctJumpsRequired;
		jumpRope.AddEventListener(this.gameObject);

		musicToSing.Play();
		singParticles.Play();

		canPress = true;

		currentFrame = -30;
		jumpRope.StartJumpRopeAnimationOnMinigameStart();
		hasStarted = true;

	}

	protected override void ResetMinigame () {
		currentFrame = 0;
		currentCorrectJumps = 0;

		pointDisplay.text = "0/" + correctJumpsRequired;
	}

	public override void OnRoomEntered () {}

	public void OnJumpRopeReset() {
		currentFrame = 0;
		hasPressedJump = false;
	}

	protected override void OnMinigameDone () {
		base.OnMinigameDone ();
		pointDisplay.text = "";
		jumpRope.ResumeJumpRopeAnimation ();
		player.OnTalkingDone ();
	}

	public void OnRopeSwingDone() {
		if(currentCorrectJumps >= correctJumpsRequired && !hasWonMinigame) {
			OnWonMinigame(player);
			StopMinigame ();
		}
	}
	
	public override void StopMinigame() {
		textBoxOnWin.AddEventListener(this.gameObject);
		player.OnTalking ();
		textBoxOnWin.ResetShowAndActivate();
		currentTextBoxManager = textBoxOnWin;

		jumpRope.PauseJumpRopeAnimation ();
	}

	public override void OnRoomExitted() {
		jumpRope.StopJumpRopeAnimation();
	}

	protected override void DoLock () {
		base.DoLock ();
		hasWonMinigame = true;
	}
}
