using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : DispatchBehaviour {
	
	public float soundblastResetTimeout = .25f;
	public float pushBackForce = 50f;

	public bool isAtBoss = false;

	private Vector2 gridLocation;

	private AnimationManager2D animationManager;
	private AnimationControl animationControl;

	private WeaponManager weaponManager;

	private RunPosition[] runPositions;
	private RunPositions circleRunPositions;

	private List<Blink2D> blinkSprites;
	private bool canBeHit = true;

	private bool isInside = false;
	private bool isInTown = false;
	private bool canDoSoundBlast = true;
	private bool isInDanceMinigame = false;

	private RoomNode currentRoomNode;
	private RoomNode startRoomNode;
	private TileBlock currentTileBlock;

	private SoundObject hpPickupSound, onHitSound, onEnterRoomWithEnemiesSound, onClearedRoomSound, onEquipSound, onUnEquipSound, onStartPressedSound, onDanceCorrectSound, onDanceFailSound;
	private CandyContainer candyContainer;

	protected MusicManager musicManager;
	protected BeatListener beatListener;

	private PlayerInputButtonHelper playerInputButtonHelper;

	private bool isDead = false;

	private BoomboxCompanion boomboxCompanion;
	private CharacterControl characterControl;

	private PowerUpComponent powerupComponent;
	private PlayerStunComponent playerStunComponent;

	private int lastRandomFrame = -1;
	private bool canDrum = true;

	private int updatesRequiredForRotationAutoFix = 100;
    private bool isInitialized = false;

	private Transform feetTransform;
	private bool isRolling = false;
	private string nameOfSpawnRoomNode = "playerroom";

	// Use this for initialization
	public virtual void Start () {
		Initialize();
	}

    public void Initialize() {
        if(!isInitialized) {
    
            isInitialized = true;

            characterControl = GetComponent<CharacterControl>();
			playerStunComponent = GetComponent<PlayerStunComponent> ();

            circleRunPositions = this.transform.Find ("RunPositions").GetComponent<RunPositions>();
    
            animationManager = this.transform.Find("Body/Animations").GetComponent<AnimationManager2D>();
            animationControl = this.transform.Find("Body/Animations").GetComponent<AnimationControl>();
    
            weaponManager = GetComponent<WeaponManager>();
            blinkSprites = new List<Blink2D>(this.transform.Find("Body/Animations").GetComponentsInChildren<Blink2D>());
    
            onEnterRoomWithEnemiesSound = this.transform.Find("Sounds/OnEnterRoomWithEnemiesSound").GetComponent<SoundObject>();
            onClearedRoomSound = this.transform.Find("Sounds/OnClearedRoomSound").GetComponent<SoundObject>();
    
            onHitSound = this.transform.Find("Sounds/OnHitSound").GetComponent<SoundObject>();
            hpPickupSound = this.transform.Find("Sounds/HpPickupSound").GetComponent<SoundObject>();
           
            onEquipSound = this.transform.Find("Sounds/OnEquipSound").GetComponent<SoundObject>();
            onUnEquipSound = this.transform.Find("Sounds/OnUnEquipSound").GetComponent<SoundObject>();
            onStartPressedSound = this.transform.Find("Sounds/OnStartPressedSound").GetComponent<SoundObject>();
    
            onDanceCorrectSound = this.transform.Find("Sounds/OnDanceCorrectSound").GetComponent<SoundObject>();
            onDanceFailSound = this.transform.Find("Sounds/OnDanceFailSound").GetComponent<SoundObject>();
    
            if(isAtBoss) {
                OnSpawned ();
                musicManager = this.transform.Find("MusicManager").GetComponent<MusicManager>();
                beatListener = musicManager.GetComponent<BeatListener>();
            }
    
            powerupComponent = GetComponent<PowerUpComponent>();
    
            playerInputButtonHelper = GetComponentInChildren<PlayerInputButtonHelper>();

			feetTransform = this.transform.Find ("Feet");
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
		--updatesRequiredForRotationAutoFix;

		if(updatesRequiredForRotationAutoFix <= 0) {
			updatesRequiredForRotationAutoFix = 100;
			this.transform.localRotation = Quaternion.identity;
		}
	}

	public void OnSpawned() {
		UnFreezePositionY ();
	}


	public void SetGridLocation(Vector2 gridLocation) {
		this.gridLocation = gridLocation;
	}

	public Vector2 GetGridLocation() {
		return this.gridLocation;
	}

	public CharacterControl GetCharacterControl() {
		return characterControl;
	}

	public void OnCollisionEnter(Collision coll) {
		
		Enemy enemy = coll.gameObject.GetComponent<Enemy>();
		OnTouchedEnemy (enemy);
	}

	public void OnTriggerEnter(Collider coll) {
		
		Enemy enemy = coll.gameObject.GetComponent<Enemy>();
		OnTouchedEnemy (enemy);
	}

	private void OnTouchedEnemy(Enemy enemy) {
		if(enemy && enemy.CanHitPlayer()) {
			if (enemy.IsStunned ()) {
				PhysicsUtils.IgnoreCollisionBetween (this.GetComponent<Collider> (), enemy.GetColliders ());
			} else {
				OnHitByEnemy (enemy);
			}
		}
	}


	private void OnHitByEnemy(Enemy enemy) {

		if(enemy is Villager) {
			return;
		}

		OnHit(enemy.transform.position);
	}

	public void OnHit(Vector3 damageSourcePosition) {

		if(canBeHit) {

			FreezePositionY ();

			canBeHit = false;

			animationControl.PlayAnimationByName("Knockback", true);
			animationManager.DisableSwitchAnimations();

			onHitSound.Play ();

			if(currentRoomNode) {
				foreach(Enemy enemy in currentRoomNode.GetRoom().GetComponentsInChildren<Enemy>()) {
					PhysicsUtils.IgnoreCollisionBetween(this.GetComponent<Collider>(), enemy.GetCollider());
				}
			}

			BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(boxCollider.size.x, .5f, boxCollider.size.z);
            
			Vector3 directionFromSourceToPlayer = MathUtils.CalculateDirection(this.transform.position, damageSourcePosition);

			GetComponent<VibrationComponent>().Vibrate();
            GetComponent<BodyControl>().Push(directionFromSourceToPlayer * pushBackForce, .5f);

			StartBlinking();

			Invoke ("EnableAnimationSwitch", .5f);
			Invoke ("ResetCanBeHit", 1f);

			DispatchMessage("OnPlayerHit", 1);

		}
	}

	public void DoRoll() {
		if(powerupComponent.HasUnlockedRolling() && !characterControl.IsRolling() && characterControl.IsWalking() && beatListener.CanDoBeat(false)) {
			isRolling = true;
			canBeHit = false;

			if(currentRoomNode) {

				foreach(ColliderToIgnoreOnRolling colliderToIgnore in currentRoomNode.GetRoom().GetComponentsInChildren<ColliderToIgnoreOnRolling>()) {
					colliderToIgnore.IgnoreCollisionWith(this);
				}
			}
		}
	}

	private void StopRolling() {
		isRolling = false;
		ResetCanBeHit();

		foreach (ColliderToIgnoreOnRolling colliderToIgnore in currentRoomNode.GetRoom().GetComponentsInChildren<ColliderToIgnoreOnRolling>()) {
			colliderToIgnore.RestoreCollisionWith (this);
		}
	}

	private void ResetCanBeHit() {

		UnFreezePositionY ();

		BoxCollider boxCollider = GetComponent<BoxCollider>();
		boxCollider.size = new Vector3(boxCollider.size.x, 1f, boxCollider.size.z);

		if(currentRoomNode) {
			foreach(Enemy enemy in currentRoomNode.GetRoom().GetComponentsInChildren<Enemy>()) {
				if (!enemy.IsStunned ()) {
					PhysicsUtils.RestoreCollisionBetween (this.GetComponent<Collider> (), enemy.GetColliders ());
				}
			}
		}

		canBeHit = true;
	}

	public void PlayRandomDanceFrame() {
		PlayRandomFrameOfAnimation (animationControl.GetAnimationPrefix() + "Dancing");
	}

	public void PlayRandomJumpAnimation(GameObject listeningGameObject) {
		string animationGroupName = animationControl.GetCurrentAnimationGroup().ToString();
		Animation2D[] jumpAnimations = animationManager.transform.Find(animationGroupName + "/JumpRope").GetComponentsInChildren<Animation2D>();
		int randomAnimation = Random.Range (0, jumpAnimations.Length);

		animationManager.AddEventListenerTo (jumpAnimations [randomAnimation].name, listeningGameObject);
		animationManager.PlayAnimationByName(jumpAnimations[randomAnimation].name, true);
	}

	private void PlayRandomFrameOfAnimation(string animationName) {

		int frameCount = animationManager.GetAnimationByName(animationName).frames.Length;
		int randomFrame = Random.Range (0, frameCount);
		
		if(randomFrame == lastRandomFrame) {
			if(randomFrame == frameCount - 1) {
				randomFrame = 0;
			} else {
				++randomFrame;
			}
		}
		
		lastRandomFrame = randomFrame;
		
		animationManager.PlayAnimationByName(animationName, true);
		animationManager.SetFrameForAnimation(animationName, randomFrame);
	}

	public void PlayIdleJumpRopeAnimation() {
		animationManager.PlayAnimationByName("IdleJumpRope", true);
	}

	public void PlayFailDanceAnimation() {
		animationManager.PlayAnimationByName(animationControl.GetAnimationPrefix() + "FailDancing", true);
		this.transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = false;
	}

    public void PlaySwallowedAnimation() {

        animationManager.EnableSwitchAnimations();
        this.transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = false;
        animationManager.PlayAnimationByName("Teleporting", true);
        animationManager.DisableSwitchAnimations();

    }
    
    public void PlaySpittedOutAnimation() {
        animationManager.PlayAnimationByName("Teleporting-Reversed", true);
        animationManager.DisableSwitchAnimations();
    }

	public void OnHealthChanged(int newHealthAmount) {
	}

	private void EnableAnimationSwitch() {

		animationManager.EnableSwitchAnimations();

		blinkSprites = new List<Blink2D>(this.transform.Find("Body/Animations").GetComponentsInChildren<Blink2D>());
		blinkSprites.ForEach(blink2D => blink2D.Blink(3));
	}

	private void StartBlinking() {
		blinkSprites = new List<Blink2D>(this.transform.Find("Body/Animations").GetComponentsInChildren<Blink2D>());
		blinkSprites.ForEach(blink2D => blink2D.Blink(3));
	}

	public void OnHealthPickedUp(HeartDrop heartDrop) {
		DispatchMessage("OnHeartPickedUp", heartDrop);
	}

	public void OnExtraHealthPickedUp() {
		DispatchMessage("ExpandHeartsBy", 1);
	}

	public void OnCandyPickedup(CandyDrop candyDrop) {
        DispatchMessage("OnCandyPickedUp", candyDrop);
        GetComponent<PlayerPickupComponent>().OnCandyPickedUp(candyDrop);
    }

	public void OnDie() {
		
		if(!isDead) {
			isDead = true;

			GameObject gameCamera = SceneUtils.FindObject<CameraShaker>().gameObject;
			
            if(currentRoomNode) {
                foreach(Enemy enemy in currentRoomNode.GetRoom().GetComponentsInChildren<Enemy>()) {
                    enemy.gameObject.SetActive(false);
                }

                foreach(ColliderToIgnoreOnRolling colliderToIgnore in currentRoomNode.GetRoom().GetComponentsInChildren<ColliderToIgnoreOnRolling>()) {
                    colliderToIgnore.gameObject.SetActive(false);
                }
            }

			this.transform.Find("Slomos/Default").GetComponent<SlomoComponent>().DoSlomo(true);

			canBeHit = false;
			CancelInvoke ("ResetCanBeHit");

			GetComponent<Rigidbody>().velocity = Vector3.zero;

			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().isKinematic = true;

			GetCharacterControl().OnDie();

			this.transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = false;

			GetComponent<PlayerInputComponent>().enabled = false;

			weaponManager.RemoveWeaponOnBack();
			animationManager.StopHideAllAnimations();

            PlayDeathAnimation();

			SceneUtils.FindObject<PlayerSaveComponent>().UpdateSpawnInfo(SpawnType.NORMAL, null);
			SceneUtils.FindObject<OnDieEffect> ().StartEffect ();

			Invoke ("RestartGame", 2f);
		}
	}

    public void OnTalking() {
        animationControl.PlayAnimationByName("Talking", true);
        animationManager.DisableSwitchAnimations();
        characterControl.StopIdleAnimation();
    }

    public void OnTalkingDone() {
        animationManager.EnableSwitchAnimations();
        animationControl.PlayAnimationByName("Idle", true);
    }

	private void RestartGame() {
		this.transform.Find("Slomos/Default").GetComponent<SlomoComponent>().StopSlomo();
		this.transform.Find("Slomos/OnKillRegularEnemy").GetComponent<SlomoComponent>().StopSlomo();

		Loader.ReloadLevelWithoutLoadingScene(false);
	}

	public RunPosition GetRandomRunPositions() {
		runPositions = this.transform.Find("RunPositions").GetComponentsInChildren<RunPosition>();
		int randomIndex = Random.Range (0, runPositions.Length);

		return runPositions[randomIndex];
	}

	public void SetStartRoomNode(RoomNode roomNode) {
		startRoomNode = roomNode;
		SetCurrentRoomNode(roomNode);
	}

	public RoomNode GetStartRoomNode() {
		return startRoomNode;
	}

	public RoomNode GetCurrentRoomNode() {
		return currentRoomNode;
	}

	public void SetCurrentRoomNode(RoomNode roomNode) {
		this.currentRoomNode = roomNode;
		SetGridLocation(roomNode.gridLocation);
	}

	public bool HasEnemiesInRoom() {
		return currentRoomNode.GetRoom().HasEnemies();
	}

	public void OnExittedRoom(RoomNode roomNode) {
		weaponManager.PullBackWeapon();
	}

	public void OnRoomEntered(RoomNode roomNode) {

		if(roomNode.GetRoomNodeType() == RoomNodeType.VillageExit ||
		   roomNode.GetRoomNodeType() == RoomNodeType.Village || 
		   roomNode.GetRoomNodeType() == RoomNodeType.VillageCenter) {
			SetInTown(true);
		}

		if(roomNode.GetRoomNodeType() == RoomNodeType.Normal) {
			SetInTown(false);
		}

		if(roomNode.GetRoom() && 
		   (roomNode.GetRoom().GetComponent<CassetteRoom>() || roomNode.GetRoom().GetComponent<GameRoom>() || roomNode.GetRoom().GetComponent<FishRoom>())) {
			SetInTown(true);
		}

		if(roomNode.GetRoom().HasEnemies()) {
			onEnterRoomWithEnemiesSound.Play ();
		}

		currentRoomNode = roomNode;
	}

	public void OnRoomCleared() {
		onClearedRoomSound.Play();
	}

	public void SetInside(bool isInside) {
		this.isInside = isInside;
	}

	public void SetInTown(bool isInTown) {
		this.isInTown = isInTown;
	}

	public void PlayHPPickupSound() {
		hpPickupSound.Play();
	}

	public bool IsInside() {
		return isInside;
	}

	public bool IsInTown() {
		return isInTown;
	}

	public void ShowButton(string deviceName, string rawButtonName) {
		playerInputButtonHelper.Show (deviceName, rawButtonName);
	}

	public void HideButton() {
		playerInputButtonHelper.Hide();
	}

	public bool CanSwitchRooms(RoomNode oldRoomNode, RoomNode newRoomNode) {
		bool canSwitchRooms = true;

		if(IsInside()) {
			canSwitchRooms = false;
		}

		if(HasEnemiesInRoom()) {
			canSwitchRooms = false;
		}

		if(oldRoomNode.GetRoomNodeType() == RoomNodeType.VillageExit && newRoomNode.GetRoomNodeType() == RoomNodeType.Normal) {
			if(weaponManager.GetWeaponOnBack() == null) {
				canSwitchRooms = false;
			}
		}

		if(oldRoomNode.GetRoom().GetComponent<CassetteRoom>() || oldRoomNode.GetRoom().GetComponent<GameRoom>()) {
			if(weaponManager.GetWeaponOnBack() == null) {
				canSwitchRooms = false;
			}
		}

		return canSwitchRooms;
	}

	public void DoSoundBlast(MusicAuraTypes musicAuraType) {
		
	}

	private void ResetCanDoSoundBlast() {
		canDoSoundBlast = true;
	}

	public void PlayEquipDBAnimation() {
		this.transform.Find ("Body/EquipDBAnimation").GetComponent<Animation2D> ().Play (true);
	}

	public void OnVillageEnteredFromOutside(RoomNode roomNode) {
		
	}

    private void PlayDeathAnimation() {
      

    }

	public void UnEquipDBAndMoveDB(Room room) {
		
        UnEquipDB();

		BoomboxActionType boomboxActionType = IsInTown() ? BoomboxActionType.RUN_TO_TARGET_VILLAGE : BoomboxActionType.RUNNING_RANDOMLY;

		boomboxCompanion.GetComponent<BoomboxActionManager>().SwitchStateAndSetRunTarget(boomboxActionType, 
		                                                                                 room.transform.Find("DBMovePosition").gameObject);

		SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);
	}

    public void UnEquipDB() {
       
    }

	public void OnVillageExitEnteredFromVillage(RoomNode roomNode) {}

	public void OnVillageExited(RoomNode oldRoomNode, RoomNode newRoomNode) {
		if(animationControl.GetCurrentAnimationGroup() == AnimationGroup.NormalVillage) {
			animationControl.SwapAnimationGroup(AnimationGroup.Normal);
		}

		GetMusicManager().PlayMusicByTileType(newRoomNode.GetRoom().GetTileType());

		SceneUtils.FindObject<LevelBuilder>().SaveData(SpawnType.NORMAL);
	}

	public void SetCandyContainer(CandyContainer candyContainer) {
		this.candyContainer = candyContainer;
	}

	public CandyContainer GetCandyContainer() {
		return candyContainer;
	}

	public void OnStart(SpawnType SpawnType = SpawnType.NORMAL) {
	    if(!isInTown) {
			Invoke ("PlayMusicDelayed", .5f);
        } else {
            Invoke("PlayTownMusicDelayed", .5f);
        }
	}


	public void PrepareForSpitOut() {
		this.transform.Find ("Shadow").GetComponent<SpriteRenderer> ().enabled = false;

		FreezePositionY ();
		this.GetComponent<Collider> ().enabled = false;

		this.GetAnimationManager ().Initialize ();
		this.GetAnimationManager ().StopHideAllAnimations ();
		this.GetAnimationManager ().DisableSwitchAnimations ();
	}

	public void OnSpitOut() {
		FreezePositionY ();

		animationManager.EnableSwitchAnimations();
		PlaySpittedOutAnimation();
		Invoke("OnSpittingOutDone", 1f);
	}

	private void OnSpittingOutDone() {

		this.transform.Find ("Shadow").GetComponent<SpriteRenderer> ().enabled = true;
		this.GetComponent<Collider> ().enabled = true;
		UnFreezePositionY ();

		GetComponent<PlayerInputComponent>().enabled = true;
        animationManager.EnableSwitchAnimations();
    }

	private void PlayMusicDelayed() {
		musicManager.PlayMusicByTileType(musicManager.GetCurrentMusicTileType());
	}

    private void PlayTownMusicDelayed() {
        musicManager.PlayMusicByTileType(TileType.specialVillage);
    }

	public void FindMusicComponents() {
		musicManager = this.transform.Find("MusicManager").GetComponent<MusicManager>();
		beatListener = musicManager.GetComponent<BeatListener>();
	}

	public RunPositions GetCircleRunPositions() {
		return circleRunPositions;
	}

	public MusicManager GetMusicManager() {
		return musicManager;
	}

	public TileBlock GetCurrentTileBlock() {
		return currentTileBlock;
	}

	public void SetCurrentTileBlock(TileBlock tileBlock) {
		if(this.currentTileBlock) {
			this.currentTileBlock.isPlayerCurrentTileBlock = false;
		}
		this.currentTileBlock = tileBlock;
		this.currentTileBlock.isPlayerCurrentTileBlock = true;
	}

	public bool CanSwitchMusic() {
		bool canSwitchMusic = true;

		if(!musicManager) {
			musicManager = this.transform.Find("MusicManager").GetComponent<MusicManager>();
		}

		if(musicManager.IsBusy()) {
			canSwitchMusic = false;
		}

		if(!weaponManager.GetCurrentWeapon() && !weaponManager.GetWeaponOnBack()) {
			canSwitchMusic = false;
		}

		if(weaponManager.GetCurrentWeapon() && weaponManager.GetCurrentWeapon().IsInAir()) {
			canSwitchMusic = false;
		}

		return canSwitchMusic;
	}

	public AnimationManager2D GetAnimationManager() {
		return this.transform.Find("Body/Animations").GetComponent<AnimationManager2D>();
	}

	public AnimationControl GetAnimationControl() {

		if(!animationControl) {
			animationControl = this.transform.Find("Body/Animations").GetComponent<AnimationControl>();
		}

		return animationControl;
	}

	public BoomboxCompanion GetBoomboxCompanion() {
		return boomboxCompanion;
	}

	public void SetBoomBoxCompanion(BoomboxCompanion boomboxCompanion) {
		this.boomboxCompanion = boomboxCompanion;
	}

	public void PlayEquipSound() {
		onEquipSound.Play ();
	}

	public void PlayUnEquipSound() {
		onUnEquipSound.Play ();
	}

	public void PlayPauseUnPauseSound() {
		onStartPressedSound.Play(true);
	}

	public void PlayOnDanceCorrectSound() {
		onDanceCorrectSound.Play(true);
	}

	public void PlayOnDanceFailSound() {
		onDanceFailSound.Play(true);
	}

	public void DisableDrumming() {
		canDrum = false;
	}

	public void EnableDrumming() {
		canDrum = true;
	}

	public bool HasUnlockedSecondAttack() {
		return powerupComponent.HasUnlockedSecondAttack ();
	}

	public bool HasUnlockedThirdAttack() {
		return powerupComponent.HasUnlockedThirdAttack ();
	}

	public Transform GetFeet() {
		return feetTransform;
	}

	public bool IsRolling() {
		return isRolling;
	}

	public bool IsInDanceMinigame() {
		return isInDanceMinigame;
	}

	public void SetInDanceMinigame(bool isInDanceMinigame) {
		this.isInDanceMinigame = isInDanceMinigame;
	}
		
	private void UnFreezePositionY() {
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
	}

	private void FreezePositionY() {
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	}

	public string GetNameOfPlayerSpawnRoomNode() {
		return nameOfSpawnRoomNode;
	}

	public void SetNameOfPlayerSpawnRoomNode(string roomNodeName){ 
		this.nameOfSpawnRoomNode = roomNodeName;
	}
}