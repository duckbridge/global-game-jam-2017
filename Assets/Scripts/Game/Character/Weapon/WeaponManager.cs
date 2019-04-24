using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : DispatchBehaviour {

	public MusicAura musicAura;

	public int maxDistanceFromWeapon = 5;
	public float originalRotation = 55f;
	public bool startsWithWeapon = false;
	public Weapon weaponToStartWith;

	private List<string> weaponsByPath = new List<string>();
	private List<WeaponOnBack> weaponsOnBack = new List<WeaponOnBack>();

	private CharacterControl characterControl;
	private BodyControl bodyControl;
	private Player player; //temp?

	private Weapon currentWeapon;
	private Transform weaponOnBackPosition;
	private WeaponOnBack weaponOnBack;

	private BeatInputHelperObject beatInputHelperObject;

	private enum WeaponState {
		None,
		Charging,
		Thrown
	}

	private WeaponState weaponState = WeaponState.None;

	// Use this for initialization
	void Awake () {
		characterControl = GetComponent<CharacterControl>();
		bodyControl = GetComponent<BodyControl>();
		player = GetComponent<Player>();
		weaponOnBackPosition = this.transform.Find("Body/WeaponOnBackSuperContainer/WeaponOnBackContainer/WeaponOnBack");
		beatInputHelperObject = SceneUtils.FindObject<BeatInputHelperObject>();
	}

	public void Start() {
		if(startsWithWeapon) {
			RetrieveWeapon(weaponToStartWith, false, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDirectionPressed(Direction throwDirection) {

		if(weaponState != WeaponState.Thrown && weaponsByPath.Count > 0) {
			bodyControl.ResetMoveSpeed();

			weaponState = WeaponState.Thrown;

			PrepareFirstWeapon();

			currentWeapon.OnDirectionPressed(throwDirection);

			musicAura.TrackWeapon(currentWeapon.transform);

			Invoke("ResetWeaponState", .25f);
		}
	}

	private void ResetWeaponState() {
		weaponState = WeaponState.None;
	}

	private void PrepareFirstWeapon() {
		if(weaponsByPath[0] != null && weaponsByPath[0].Length > 0) {

			DispatchMessage("OnItemUsed", null);

			weaponsByPath.RemoveAt(0);

			RemoveWeaponOnBack();

			currentWeapon.AddEventListener(characterControl.gameObject); //change!
		}
	}

	public void RemoveWeaponOnBack() {
		if(weaponsOnBack.Count > 0) {
			WeaponOnBack weaponOnBack = weaponsOnBack[0];
			weaponsOnBack.RemoveAt(0);
			Destroy (weaponOnBack.gameObject);
		}
	}
	
    public void RetrieveWeapon(Weapon weapon, bool destroyWeaponOnDone, bool playSoundEffect) {

		if(beatInputHelperObject) {
			beatInputHelperObject.DoEnable();
		}
        
        if(playSoundEffect) {
            player.PlayEquipSound();
        }

		string weaponNameWithoutClone = weapon.name.Split('(')[0];

		musicAura.TrackPlayer();

		weaponsByPath.Add(weapon.prefabPath + weaponNameWithoutClone);
		
		DispatchMessage("OnItemPickedUp", weapon.uiPrefabPath + weaponNameWithoutClone);
		
		weaponOnBack = (WeaponOnBack)
			GameObject.Instantiate(Resources.Load(weapon.uiPrefabPath + weaponNameWithoutClone, typeof(WeaponOnBack)), weaponOnBackPosition.position , Quaternion.identity) as WeaponOnBack;
		
		weaponOnBack.transform.parent = weaponOnBackPosition;
		weaponOnBack.transform.localPosition = new Vector3(weaponOnBack.transform.localPosition.x, weaponOnBack.transform.localPosition.y, weaponOnBack.offsetY);

		weaponOnBack.SetOriginalRotation(originalRotation);
        
       
        BoomBox boombox = weaponOnBack.GetComponent<BoomBox>();
        MusicManager musicManager = player.GetMusicManager();
        
        if(musicManager && boombox && boombox.isOnPlayerBack && musicManager.IsPlayingMusic()) {
            boombox.StartEmitting();
        }

		weaponsOnBack.Add(weaponOnBack);

		if(destroyWeaponOnDone) {
			weapon.DestroyWeapon();
		}

		weaponState = WeaponState.None;
	}

	public void PullBackWeapon() {
		if(currentWeapon) {
			currentWeapon.RetractWeapon();
		}
	}

	public Weapon GetCurrentWeapon() {
		return currentWeapon;
	}

	public bool HasWeapons() {
		return weaponsByPath.Count > 0;
	}

	public void DestroyCurrentWeapon() {
		if(currentWeapon) {	
			currentWeapon.DestroyWeapon();
		}
	}

	public WeaponOnBack GetWeaponOnBack() {
		return weaponOnBack;
	}

	public void RemoveWeapon() {

        player.PlayUnEquipSound();

		DispatchMessage("OnItemUsed", null);

		if(weaponOnBack != null) {
			RemoveWeaponOnBack();
		}
		
		if(currentWeapon != null) {
			DestroyCurrentWeapon();
		}

		if(weaponsByPath.Count > 0) {
			weaponsByPath[0] = null;
			weaponsByPath = new List<string>();
		}
	}

	public void SetPlayerName(PlayerCharacterName playerCharacterName) {
		
	}
}
