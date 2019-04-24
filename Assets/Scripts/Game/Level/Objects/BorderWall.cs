using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BorderWall : DispatchBehaviour {

	public int hitsRequiredForTrashTalk = 6;

	public TextBoxManager[] trashTalkTextboxes, sadTalkTextboxes;
	public float damageDecrementAmount = .1f;
	public int maximumMusicDamage = 10;
	
	private AnimationManager2D animationManager;
	private SpriteRenderer shadow;
	private List<Blink2D> blinkSprites;
	private float currentDamage = 0;
	
	private bool isTakingDamage = false;
	private EnemyHealthbar healthbar;

	private bool isDead = false;
	private SoundObject onDamageSound, onDeathSound;

	private int amountOfTimesHit = 0;
	private bool isBusyTalking = false;
	private EnemyBreakComponent enemyBreakComponent;
    protected OnHitParticleSpawner onHitParticleSpawner;

	private bool canUpdateHealthBar = true;
	public void Awake () {
		
		if(this.transform.Find("Healthbar")) {
			healthbar = this.transform.Find("Healthbar").GetComponent<EnemyHealthbar>();
		}
		
		if(this.transform.Find("Animations/Shadow")) {
			shadow = this.transform.Find("Animations/Shadow").GetComponent<SpriteRenderer>();
		}
		
		blinkSprites = new List<Blink2D>(this.transform.Find("Animations").GetComponentsInChildren<Blink2D>());
		
		animationManager = GetComponentInChildren<AnimationManager2D>();

		onDamageSound = this.transform.Find("Sounds/OnDamageSound").GetComponent<SoundObject>();

		if(this.transform.Find("Sounds/OnDeathSound")) {
			onDeathSound = this.transform.Find("Sounds/OnDeathSound").GetComponent<SoundObject>();
		}

        if(this.transform.Find("OnHitParticleSpawner")) {
            onHitParticleSpawner = this.transform.Find("OnHitParticleSpawner").GetComponent<OnHitParticleSpawner>();
        }

		enemyBreakComponent = GetComponent<EnemyBreakComponent>();
	}

	public void Start() {
		FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
		if (followingEye) {
			blinkSprites.Add (followingEye.transform.Find("Eye").GetComponent<Blink2D> ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public virtual void FixedUpdate() {
		if(currentDamage > 0 && damageDecrementAmount > 0) {
			currentDamage -= damageDecrementAmount;
			UpdateHealthBar();
		}
	}
	
	public virtual void DoDamage(float amountOfDamage) {

		++amountOfTimesHit;
		if(amountOfTimesHit >= hitsRequiredForTrashTalk && (currentDamage <= maximumMusicDamage / 2)) {

			amountOfTimesHit = 0;

			if(!isBusyTalking && trashTalkTextboxes.Length > 0) {
				isBusyTalking = true;
				int randomTextBoxIndex = Random.Range (0, trashTalkTextboxes.Length);

				trashTalkTextboxes[randomTextBoxIndex].AddEventListener(this.gameObject);
				trashTalkTextboxes[randomTextBoxIndex].ResetShowAndActivate();
			}
		}

		if(currentDamage >= (maximumMusicDamage /2)) {
			if(!isBusyTalking && sadTalkTextboxes.Length > 0) {
				isBusyTalking = true;
				int randomTextBoxIndex = Random.Range (0, sadTalkTextboxes.Length);
				
				sadTalkTextboxes[randomTextBoxIndex].AddEventListener(this.gameObject);
				sadTalkTextboxes[randomTextBoxIndex].ResetShowAndActivate();
			}
		}

		onDamageSound.Play();

		currentDamage += amountOfDamage;
		
		UpdateHealthBar();
		
		blinkSprites.ForEach(blinkSprite => blinkSprite.Blink(1));
		
		if(currentDamage >= maximumMusicDamage && !isDead) {

			canUpdateHealthBar = false;

			blinkSprites.ForEach(blinkSprite => blinkSprite.Blink(-1));

			this.GetComponent<Rigidbody>().isKinematic = true;
			this.GetComponent<Collider>().enabled = false;

			Invoke ("OnDie", .75f);	
		}
	}

    public void SpawnOnHitParticles(Vector3 direction) {
         if(onHitParticleSpawner) {
            onHitParticleSpawner.SpawnParticles(direction);
        }
    }

	public void OnTextDone() {
		isBusyTalking = false;
	}
	
	protected virtual void OnDie() {
		
		if(!isDead) {

			if(enemyBreakComponent) {
				enemyBreakComponent.BreakAll();
			}			

			FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
			if (followingEye) {
				Destroy (followingEye.gameObject);
			}

			if(onDeathSound) {
				onDeathSound.PlayIndependent(true);
			}

			DispatchMessage("OnBorderWallDied", null);

			healthbar.gameObject.SetActive(false);
			
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.GetComponent<Collider>().enabled = false;
			
			if(shadow) {
				shadow.enabled = false;
			}
			
			animationManager.StopHideAllAnimations();
			
			animationManager.AddEventListenerTo("Death", this.gameObject);
			animationManager.PlayAnimationByName("Death", true);

			isDead = true;
		}
	}
	
	protected void UpdateHealthBar() {
		if(canUpdateHealthBar) {
			healthbar.UpdateHealthbar(currentDamage, maximumMusicDamage);
		}
	}
	
	public virtual void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "Death") {
			OnDeath();
		}
	}
	
	protected virtual void OnDeath() {
		Destroy (this.gameObject);
	}
}
