using UnityEngine;
using System.Collections;

public class PlayerPickupComponent : MonoBehaviour {

    public float pickupTimeout = 2f;

    private SoundObject onCassettePickupSound;
    private GameObject currentPickup;
    private Transform cassettePosition;

    private Player player;
	private bool enableInputAfterPickup = true;

	// Use this for initialization
	void Start () {
	    onCassettePickupSound = this.transform.Find("Sounds/OnCassettePickupSound").GetComponent<SoundObject>();
        cassettePosition = this.transform.Find("CassettePickupPosition");

        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnObjectPickedUp(GameObject objectGO, bool enableInputAfterPickup = true) {
		this.enableInputAfterPickup = enableInputAfterPickup;

        this.currentPickup = objectGO;
        GetComponent<PlayerInputComponent>().enabled = false;
        
        onCassettePickupSound.Play();

        player.GetAnimationManager().PlayAnimationByName("CassettePickup", true);
        player.GetAnimationManager().DisableSwitchAnimations();

        GetComponent<BodyControl>().StopMoving();

        objectGO.GetComponent<Rigidbody>().isKinematic = true;
        objectGO.GetComponent<Rigidbody>().useGravity = false;

        objectGO.GetComponent<Collider>().enabled = false;
        objectGO.transform.position = cassettePosition.position;

        Invoke("OnCassettePickupDone", pickupTimeout);
    }

    public void OnCandyPickedUp(CandyDrop candyDrop) {
        OnObjectPickedUp(candyDrop.gameObject);
    }

    public void OnPlayableGamePickedUp(GamePickup gamePickup) {
       OnObjectPickedUp(gamePickup.gameObject);
    }

	public void OnCassettePickupPickedUp(CassettePickup cassettePickup, bool enableInputAfterPickup = true) {
		player.GetMusicManager().AddAvailableSong(cassettePickup.tileType);
		onCassettePickupSound.Play();
		OnObjectPickedUp(cassettePickup.gameObject, enableInputAfterPickup);
    }

    public void OnCassettePickupDone() {
        Destroy(currentPickup);

        player.GetAnimationManager().EnableSwitchAnimations();
        player.GetAnimationControl().PlayAnimationByName("Idle", true);

		if (enableInputAfterPickup) {
			GetComponent<PlayerInputComponent> ().enabled = true;
		}

        LevelBuilder levelBuilder = SceneUtils.FindObject<LevelBuilder>();
        if(levelBuilder) {
			levelBuilder.SaveData(SpawnType.NORMAL);
        }   
    }
}
