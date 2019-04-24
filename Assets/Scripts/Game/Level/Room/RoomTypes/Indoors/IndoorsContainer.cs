using UnityEngine;
using System.Collections;

public class IndoorsContainer : DispatchBehaviour {

	protected Door door;

	private Vector3 originalCameraPosition;
	private Vector3 playerSpawnPositionOnExit, animalSpawnPositionOnExit;
	private GameObject gameCamera;

	private Player player;
	private WeatherManager weatherManager;
	private WeatherManager.WeatherState savedWeatherState;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnPlayerEntered(Player player, GameObject gameCamera, Vector3 playerSpawnPositionOnExit) {
		this.player = player;

		this.playerSpawnPositionOnExit = new Vector3(playerSpawnPositionOnExit.x, player.transform.position.y, playerSpawnPositionOnExit.z);
	
		this.gameCamera = gameCamera;

		originalCameraPosition = gameCamera.transform.position;

		door = this.transform.Find("Door").GetComponent<Door>();
		door.AddEventListener(this.gameObject);

		door.OnEntered(player);

        if(gameCamera.GetComponent<FollowCamera2D>()) {
            gameCamera.GetComponent<FollowCamera2D>().enabled = false;
        }

		gameCamera.transform.position = GetCameraPosition().position;

		gameCamera.transform.Find("SnowParticle").transform.localPosition = gameCamera.transform.Find("IndoorsRainSnowPosition").localPosition;
		gameCamera.transform.Find("RainAnimation").transform.localPosition = gameCamera.transform.Find("IndoorsRainSnowPosition").localPosition;

		weatherManager = SceneUtils.FindObject<WeatherManager>();
		savedWeatherState = weatherManager.GetWeatherState();

		SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);

		SceneUtils.FindObjects<SoundThatDimsOnDoorEnter>().ForEach(soundObject => soundObject.DimSound());
	}

	public virtual void OnPlayerExitted() {
		player.transform.position = playerSpawnPositionOnExit;

		gameCamera.transform.position = originalCameraPosition;

        if(gameCamera.GetComponent<FollowCamera2D>() && player.GetCurrentRoomNode().GetRoom().GetComponent<VillageRoom>()) {
            gameCamera.GetComponent<FollowCamera2D>().enabled = true;
        }

		gameCamera.transform.Find("SnowParticle").transform.localPosition = gameCamera.transform.Find("RegularRainSnowPosition").localPosition;
		gameCamera.transform.Find("RainAnimation").transform.localPosition = gameCamera.transform.Find("RegularRainSnowPosition").localPosition;

		if(savedWeatherState == WeatherManager.WeatherState.RAIN) {
			weatherManager.EnableRain();
		}

		if(savedWeatherState == WeatherManager.WeatherState.SNOW) {
			weatherManager.EnableSnow();
		}

		SceneUtils.FindObjects<SoundThatDimsOnDoorEnter>().ForEach(soundObject => soundObject.ResetSound());

		DispatchMessage("OnPlayerExitHouse", null);

		this.gameObject.SetActive(false);
	}

	public Transform GetCameraPosition() {
		return this.transform.Find("CameraPosition");
	}
}
