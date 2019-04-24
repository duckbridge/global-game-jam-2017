using UnityEngine;
using System.Collections;

public class HouseDoor : Door {

	public string roomToLoad = "Rooms/indoors/house01";
	protected IndoorsContainer loadedHouse;

	private Transform playerSpawnPosition;

	public override void Start () {
		playerSpawnPosition = this.transform.Find("SpawnPosition");
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			CameraBorderManager cameraBorderManager = SceneUtils.FindObject<CameraBorderManager>();

			if(!cameraBorderManager.IsTransitioning()) {
				this.player = player;
				player.GetComponent<PlayerInputComponent> ().enabled = false;

				DoorEnterExitAnimation doorEnterExitAnimation = SceneUtils.FindObject<DoorEnterExitAnimation> ();

				doorEnterExitAnimation.AddEventListener (this.gameObject);
				doorEnterExitAnimation.hideWhenDone = false;
				doorEnterExitAnimation.Play (true, false);
			}

			this.canInteract = false;
			player.GetComponent<PlayerInputComponent>().RemoveEventListener(this.gameObject);

		}
	}

	public virtual void OnPlayerExitHouse() {
	
	}

	public void OnAnimationDone(Animation2D animation2D) {

		if (animation2D.name == "OnDoorEnterExitAnimation") {
			GameObject gameCamera = GameObject.Find ("CameraContainer");

			if (loadedHouse) {

				loadedHouse.gameObject.SetActive (true);
				loadedHouse.OnPlayerEntered (player, gameCamera, playerSpawnPosition.position);

			} else {
				IndoorsContainer house = (IndoorsContainer)GameObject.Instantiate (Resources.Load (roomToLoad, typeof(IndoorsContainer)), new Vector3 (gameCamera.transform.position.x, 100f, gameCamera.transform.position.z), Quaternion.identity);
				house.OnPlayerEntered (player, gameCamera, playerSpawnPosition.position);
				loadedHouse = house;
			}

			loadedHouse.AddEventListener (this.gameObject);
			this.player.GetComponent<PlayerInputComponent> ().enabled = true;

			DoorEnterExitAnimation doorEnterExitAnimation = SceneUtils.FindObject<DoorEnterExitAnimation> ();

			doorEnterExitAnimation.RemoveEventListener(this.gameObject);
			doorEnterExitAnimation.hideWhenDone = true;
			doorEnterExitAnimation.Play (true, true);

			DoExtraOnDoorEnterExitAnimation ();
		}
	}

	protected virtual void DoExtraOnDoorEnterExitAnimation() {}
}
