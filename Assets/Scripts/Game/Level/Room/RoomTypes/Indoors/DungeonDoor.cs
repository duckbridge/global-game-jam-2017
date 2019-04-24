using UnityEngine;
using System.Collections;

public class DungeonDoor : Door {

	public LoadingScreenType loadingScreenType = LoadingScreenType.dungeon_one;
	public TileType trackTileTypeRequired = TileType.three;

	public int maximumBossIdAllowed = 0;
	public bool isEntrance = true;
	public Scene sceneToLoad = Scene.DungeonScene;

    protected bool isLocked = true;

	// Use this for initialization
	public virtual void Start () {

		if(isEntrance) {

			GetComponent<Collider>().enabled = false;
			Invoke ("UnlockDoor", 1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
            if(!isLocked) {
                OpenDoor();
            }

			canInteract = true;
			ShowInput(player);
			player.GetComponent<PlayerInputComponent>().AddEventListener(this.gameObject);
			player.DisableDrumming();
		}
	}
	
	public virtual void OnTriggerExit(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
            
            if(!isLocked) {
                CloseDoor();
            }   

			DisableInteraction(player);
			player.EnableDrumming();
		}
	}

	public override void OnInteract (Player player) {

		if(canInteract) {
			if(isEntrance) {
				
				MapBuilder mapBuilder = SceneUtils.FindObject<MapBuilder>();
				if(mapBuilder) {
					mapBuilder.SaveData(SpawnType.NORMAL);
				}
			}

            player.GetComponent<PlayerInputComponent>().enabled = false;
			SceneUtils.FindObject<OnDieEffect> ().StartEffect ();
            Invoke("LoadDelayed", 1f);
		}
	}

    protected void LoadDelayed() {
        Loader.LoadScene (sceneToLoad, loadingScreenType);
    }

	public void UnlockDoor() {
		if(isEntrance) {
			PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
			UnlockDoorIfPossible(playerSaveComponent);
		}
	}

	private void UnlockDoorIfPossible(PlayerSaveComponent playerSaveComponent) {
        isLocked = true;

		if(playerSaveComponent.GetLastBossBeatenId() <= maximumBossIdAllowed && 
		   playerSaveComponent.GetUnlockedTileTypeTracks().Contains(trackTileTypeRequired) &&
		   doorAnimation) {
            isLocked = false;
			GetComponent<Collider>().enabled = true;
		}

		if(trackTileTypeRequired == TileType.none) {
            isLocked = false;
			GetComponent<Collider>().enabled = true;
		}
	}
}
