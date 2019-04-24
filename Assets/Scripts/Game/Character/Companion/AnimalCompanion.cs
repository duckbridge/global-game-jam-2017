using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalCompanion : DispatchBehaviour {

	public float damageDecrementAmount = .1f;
	public int maximumMusicDamage = 10;

	[TextArea(3,10)]
	public string description = "A Foxy fox"; 

	protected AnimalActionManager animalActionManager;

	protected AnimationControl animationControl;
	protected AnimationManager2D animationManager;

	protected Room currentRoom;
	protected Transform target;
	protected string originalName;

	protected EnemyHealthbar healthbar;
	protected float currentDamage = 0;
	protected SoundObject onDamageSound;

	protected bool isUsingHealthbar = true;
	protected AnimalInteractionObject animalInteractionObject;
    protected ParticleSystem heartParticles;    
    protected Teleporter teleporter;

	public virtual void Awake () {
		animalActionManager = GetComponent<AnimalActionManager>();
		animationManager = this.transform.Find("Body/Animations").GetComponent<AnimationManager2D>();
		animationControl = this.transform.Find("Body/Animations").GetComponent<AnimationControl>();
		animalInteractionObject = GetComponentInChildren<AnimalInteractionObject>();

		if(this.transform.Find("Healthbar")) {
			healthbar = this.transform.Find("Healthbar").GetComponent<EnemyHealthbar>();
		}

        if(this.transform.Find("HeartParticles")) {
            heartParticles = this.transform.Find("HeartParticles").GetComponent<ParticleSystem>();
        }
		onDamageSound = this.transform.Find("Sounds/OnHitSound").GetComponent<SoundObject>();
	}


	public void FixedUpdate() {
		if(currentDamage > 0 && damageDecrementAmount > 0 && isUsingHealthbar) {
			currentDamage -= damageDecrementAmount;
			UpdateHealthBar();
		}
	}

	private void UpdateHealthBar() {
		healthbar.UpdateHealthbar(currentDamage, maximumMusicDamage);
		
		if(animalActionManager.currentAnimalAction.animalActionType == AnimalActionType.FOLLOWING_PLAYER) {	
			if(currentDamage <= 1) {
				if(isUsingHealthbar) {
					DispatchMessage("OnAnimalNoHealth", null);
				}
			}
		}
	}

    public void SetAtTeleporter(Teleporter teleporter) {
        this.teleporter = teleporter;
    }

	public virtual void DoDamage(float amountOfDamage) {
		
		onDamageSound.Play();
		
		currentDamage += amountOfDamage;
		
		UpdateHealthBar();

		if(currentDamage >= maximumMusicDamage) {
			currentDamage = maximumMusicDamage;

			if(animalActionManager.currentAnimalAction.animalActionType == AnimalActionType.WAKEUP ||
			   animalActionManager.currentAnimalAction.animalActionType == AnimalActionType.IDLE) {
				DispatchMessage("OnAnimalFullHealth", null);
			}
		}
	}

	public void OnIdleInVillage() {
		DisableHealthbar();

		if(animalInteractionObject) {
			animalInteractionObject.GetComponent<Collider>().enabled = true;
		}
	}

	public void DisableHealthbar() {
		isUsingHealthbar = false;
		healthbar.gameObject.SetActive(false);
	}

	public virtual void Start() {
		animalActionManager.Initialize();
	}

	public void SwitchAnimalAction(AnimalActionType newAnimalActionType) {
		animalActionManager.SwitchState(newAnimalActionType);
	}

	public void SwitchAnimalActionAndSetRunTarget(AnimalActionType newAnimalActionType, GameObject newTarget) {
		animalActionManager.SwitchStateAndSetRunTarget(newAnimalActionType, newTarget);
	}

	public void PlayAnimationByName(string animationName) {
		animationManager.PlayAnimationByName(animationName, true);
	}

	public void ResumeAnimationByName(string animationName) {
		animationManager.ResumeAnimationByName(animationName);
	}

	public AnimationControl GetAnimationControl() {
		return animationControl;
	}

	public AnimationManager2D GetAnimationManager() {
		return animationManager;
	}


	public void SetCurrentRoom(Room currentRoom) {
		this.currentRoom = currentRoom;
	}

	public Room GetCurrentRoom() {
		return this.currentRoom;
	}

	public void SetTarget(Transform newTarget) {
		this.target = newTarget;
	}

	public Transform GetTarget() {
		return this.target;
	}

	public void SetOriginalName(string originalName) {
		this.originalName = originalName;
	}
	
	public string GetOriginalName() {
		return originalName;
	}
    
    public void StartHeartParticles() {
        if(heartParticles) {
            heartParticles.Play();
        }
    }

    public void StopHeartParticles() {
        if(heartParticles) {
            heartParticles.Stop();
        }
    }

    public Teleporter GetTeleporter() {
        return teleporter;
    }
}
