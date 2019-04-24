using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : DispatchBehaviour {

    public float spawnTimeout = .5f;

	public bool canBePushed = true;
	public float damageDecrementAmount = .1f;
	public int maximumMusicDamage = 10;

	public float[] chancesToDrop;
	public LootDrop[] lootToDrop;

	protected EnemyAIActionManager enemyAIActionManager;

	protected AnimationManager2D animationManager;
	protected AnimationControl animationControl;

	protected bool canHitPlayer = true;
	protected SpriteRenderer shadow;
	protected bool isDead = false;

	protected List<Blink2D> blinkSprites;
	protected float currentDamage = 0;

	protected bool isTakingDamage = false;
	protected EnemyHealthbar healthbar;

	protected Player player;
	protected Room currentRoom;
	protected EnemyBreakComponent enemyBreakComponent;

	protected bool isBroken = false;
	protected SoundObject onDamageSound, onDeathSound, onStunSound;
    protected OnHitParticleSpawner onHitParticleSpawner;

    protected bool canDie = true;
	protected bool isStunned = false;

	protected bool isDeathBlinking = false;

	public virtual void Awake () {

		if(this.transform.Find("Healthbar")) {
			healthbar = this.transform.Find("Healthbar").GetComponent<EnemyHealthbar>();
		}

		if(this.transform.Find("Animations/Shadow")) {
			shadow = this.transform.Find("Animations/Shadow").GetComponent<SpriteRenderer>();
		}

		if(this.transform.Find ("Sounds/OnDeathSound")) {
			onDeathSound = this.transform.Find("Sounds/OnDeathSound").GetComponent<SoundObject>();
		}

		if (this.transform.Find ("Sounds/OnStunSound")) {
			onStunSound = this.transform.Find ("Sounds/OnStunSound").GetComponent<SoundObject> ();
		}

        if(this.transform.Find("OnHitParticleSpawner")) {
            onHitParticleSpawner = this.transform.Find("OnHitParticleSpawner").GetComponent<OnHitParticleSpawner>();
        }

		blinkSprites = new List<Blink2D>(this.transform.Find("Animations").GetComponentsInChildren<Blink2D>());

		animationManager = GetComponentInChildren<AnimationManager2D>();
		animationControl = GetComponentInChildren<AnimationControl>();

		enemyAIActionManager = GetComponent<EnemyAIActionManager>();

		player = SceneUtils.FindObject<Player>();

		if(GetComponent<CharacterToTargetTurner>()) {
			GetComponent<CharacterToTargetTurner>().SetTarget(player.transform);
		}

		enemyBreakComponent = GetComponent<EnemyBreakComponent>();

		onDamageSound = this.transform.Find("Sounds/OnDamageSound").GetComponent<SoundObject>();

	}

	public virtual void OnSpawned(Room room) {
		this.currentRoom = room;
	}

	public Room GetRoom() {
		return this.currentRoom;
	}

	public virtual void Start() {
        FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
        if(followingEye) {
            followingEye.Initialize(player.transform);
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

	public virtual void Stun(float unStunSpeed, Color stunColor) {
		if (!this.isStunned) {
			PlatformerAIAction foundAction = enemyAIActionManager.FindActionByType ("Stun");
			if (foundAction) {
				StunAction stunAction = foundAction.GetComponent<StunAction> ();
				stunAction.SetActionToSwitchTo (enemyAIActionManager.GetCurrentActionTypeName ());
				stunAction.SetUnStunTime (unStunSpeed);
				stunAction.SetStunColor (stunColor);
				enemyAIActionManager.OnActionDone ("Stun");
			}
		}
	}

	public virtual void DoDamage(float amountOfDamage, MusicAuraTypes musicAuraType) {

		onDamageSound.Play();

		currentDamage += amountOfDamage;

		UpdateHealthBar();

		blinkSprites.ForEach(blinkSprite => blinkSprite.Blink(1));

		OnHit (amountOfDamage);

		if(currentDamage >= maximumMusicDamage && !isDead) {

			SetCanBeHit (false);
			blinkSprites.ForEach(blinkSprite => blinkSprite.Blink(-1));

			currentDamage = maximumMusicDamage;

			UpdateHealthBar();

			OnBeforeDie ();

			if (!isDeathBlinking) {
				isDeathBlinking = true;

				float calculatedBlinkTime = EnemyUtil.CalculateBlinkTime (currentRoom);

				Invoke ("OnDie", calculatedBlinkTime);	
			}
		}
	}

    public void DecreaseDamage(float amountOfDamage) {
        
        currentDamage -= amountOfDamage;
        if(currentDamage < 0f) {
            currentDamage = 0f;        
        }

        UpdateHealthBar();
    }

	public void Kill() {

		OnBeforeDie ();
		OnDie();
	}

	private void OnBeforeDie() {

		animationManager.StopAllAnimations ();

		this.GetComponent<Collider>().enabled = false;
		this.GetComponent<Rigidbody>().isKinematic = true;

		enemyAIActionManager.StopCurrentAction();
	}
	
	public virtual void OnHit(float damage) {
       
	}

	private void ResetCanBeHit() {
		canHitPlayer = true;
	}

	protected virtual void OnDie() {

        if(!canDie) {
            canDie = true;        
        }

		if(!isDead) {
            isDead = true;

			if(enemyBreakComponent) {
				enemyBreakComponent.BreakAll();
			}

			FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
			if(followingEye) {
				Destroy (followingEye.gameObject);
			}

			if(this.onDeathSound) {
				this.onDeathSound.PlayIndependent(true);
			}

			HideHealthbar();

			DispatchMessage("OnEnemyDied", this);

			OnDeActivate();

			if(GetComponent<CharacterControl>()) {
				GetComponent<CharacterControl>().OnDie();
			}

			DropLoot();
			
			if(shadow) {
				shadow.enabled = false;
			}

			if(animationControl) {
				animationControl.Disable();
			}

			animationManager.StopHideAllAnimations();

			animationManager.AddEventListenerTo("Death", this.gameObject);
			animationManager.PlayAnimationByName("Death", true);

			DoExtraOnDeath();
		}
	}

	public void PlayStunnedSound() {
		if(onStunSound) {
			onStunSound.Play (true);
		}
	}

	public void HideHealthbar() {
		healthbar.gameObject.SetActive(false);
	}

	public void ShowHealthbar() {
		healthbar.gameObject.SetActive(true);
	}

	protected virtual void DoExtraOnDeath() {
		player.transform.Find("Slomos/OnKillRegularEnemy").GetComponent<SlomoComponent> ().DoSlomo ();
	}
	
	protected virtual void SpawnRandomCritter() {
		CritterNames[] critterNames = (CritterNames[]) System.Enum.GetValues(typeof(CritterNames));
		int randomCritterNameIndex = Random.Range (0, critterNames.Length);

		CritterNames chosenName = critterNames[randomCritterNameIndex];

		AnimalCompanion animalCompanion = (AnimalCompanion)
			GameObject.Instantiate(Resources.Load("Critters/" + chosenName.ToString(), typeof(AnimalCompanion)), this.transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f))) as AnimalCompanion;

		animalCompanion.SetOriginalName(chosenName.ToString());
		animalCompanion.SetCurrentRoom(currentRoom);
	}

	protected void UpdateHealthBar() {
		healthbar.UpdateHealthbar(currentDamage, maximumMusicDamage);
	}

	protected void DropLoot() {

		Transform lootDropTransform = this.transform;
		if(this.transform.Find("LootDropPosition")) {
			lootDropTransform = this.transform.Find("LootDropPosition");
		}

		for(int i = 0 ; i < lootToDrop.Length ; i++) {
			
			int randomDropChance = Random.Range (0, 100);

			if(chancesToDrop[i] >= randomDropChance) {
				LootDrop droppedLoot = (LootDrop) GameObject.Instantiate(lootToDrop[i], lootDropTransform.position, Quaternion.identity);
				droppedLoot.DoDrop();
			}
		}
	}

	public void PushInDirection(Vector3 force, bool usesTinyBlast) {

		if(canBePushed) {
			if(GetComponent<BodyControl>()) {
				GetComponent<BodyControl>().DisableMoving();

				if(usesTinyBlast) { 
					Invoke ("EnableMoving", .25f);
				} else {
					Invoke ("EnableMoving", .5f);
				}
			}

			this.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}

    public virtual void SpawnOnHitParticles(Vector3 direction) {
         if(onHitParticleSpawner) {
            onHitParticleSpawner.SpawnParticles(direction);
        }
    }

	private void EnableMoving() {
		if(GetComponent<BodyControl>()) {
			GetComponent<BodyControl>().ReEnableMoving();
		}

	}

	public virtual void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "Death") {
			OnDeathAnimationDone();
		}
	}

	protected virtual void OnDeathAnimationDone() {
		Destroy (this.gameObject);
	}

	public virtual void OnActivate() {
		if(enemyAIActionManager && !isDead) {
            Invoke("StartFirstAction", spawnTimeout);
		}

		Invoke ("EnableInsideCameraKeeper", 1f);
	}

    private void StartFirstAction() {
		if (!isDead && !isDeathBlinking) {
			enemyAIActionManager.StartFirstAction ();
		}
    }

	private void EnableInsideCameraKeeper() {
		if(GetComponent<EnemyInsideCameraKeeper>()) {
			GetComponent<EnemyInsideCameraKeeper>().DoEnable();	
		}
	}

	public virtual void OnDeActivate() {
		if(enemyAIActionManager && !isDead) {
			enemyAIActionManager.StopCurrentAction();
		}
	}

	public void OnObjectSplittingDone() {
		Destroy(this.gameObject);
	}

	public virtual void HideAnimationByName(string animationName) {
		animationManager.GetAnimationByName(animationName).Hide ();
	}

	public virtual void ShowAnimationByName(string animationName) {
		animationManager.GetAnimationByName(animationName).Show ();
	}

	public virtual void PlayAnimationByName(string animationName, bool reset) {

		if(isBroken) {
			animationName += "-Broken";
		}

		animationManager.PlayAnimationByName(animationName, reset);
	}

	public virtual void AddEventListenerTo(string animationName, GameObject go) {
		animationManager.AddEventListenerTo(animationName, go);
	}

	public virtual void RemoveEventListenerFrom(string animationName, GameObject go) {
		animationManager.RemoveEventListenerFrom(animationName, go);
	}

	public void SetCanBeHit(bool canBeHit) {
		this.canHitPlayer = canBeHit;
	}

	public bool CanHitPlayer() {
		return canHitPlayer;
	}

	public virtual Collider GetCollider() {
		return GetComponent<Collider>();
	}

	public virtual Collider[] GetColliders() {
		return new Collider[] {GetCollider()};
	}

	public AnimationControl GetAnimationControl() {
		return animationControl;
	}

	public AnimationManager2D GetAnimationManager() {
		return animationManager;
	}

    public void SetCanDie(bool canDie) {
        this.canDie = canDie;
    }

	public void SetStunned(bool isStunned) {
		this.isStunned = isStunned;
	}

	public bool IsStunned() {
		return isStunned;
	}

	public bool IsDeathBlinking() {
		return isDeathBlinking;
	}
}
