using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : Loader {

	public Transform chosenAnimationSpawnPosition;
	private LoadingScreenAnimation chosenAnimation;
	
	void Awake() {

		string loadingScreenTypeString = Loader.loadingScreenType.ToString().ToLower();
		if(loadingScreenTypeString.Contains("_")) {
			string[] splittedLoadingScreenType = loadingScreenTypeString.Split('_');
			chosenAnimation = GameObject.Instantiate(Resources.Load("Loadingscreens/" +  splittedLoadingScreenType[0] + "/" + splittedLoadingScreenType[1], typeof(LoadingScreenAnimation)), chosenAnimationSpawnPosition.position , Quaternion.identity) as LoadingScreenAnimation;
			
		} else {
			chosenAnimation = GameObject.Instantiate(Resources.Load("Loadingscreens/" + loadingScreenTypeString , typeof(LoadingScreenAnimation)), chosenAnimationSpawnPosition.position , Quaternion.identity) as LoadingScreenAnimation;
		}
		chosenAnimation.transform.parent = this.transform;
		chosenAnimation.AddEventListener(this.gameObject);
	}
	
	public override void Start() {
		base.Start();
	}
	
	public override void OnLoadingDone() {
		LevelBuilder levelBuilder = SceneUtils.FindObject<LevelBuilder>();
		if(levelBuilder) {
			levelBuilder.AddEventListener(this.gameObject);
			levelBuilder.PrepareLevel();
		} else {
			Logger.Log("No Levelbuilder found!", LogType.Log);
		}
		StartGame();
	}

	protected override void OnLoadingProgressing (float progress) {
		
		string msg = "Starting up!";

		if(progress > 0.2f) {
			msg = "Getting closer!";
		}

		if(progress > 0.9f) {
			msg = "Almost done! Hold up";
		}

		OnSetLoadingText(new LoadingMessage(msg, System.Convert.ToInt32(progress*100)));
	}

	public void OnSetLoadingText(LoadingMessage loadingMessage) {
		chosenAnimation.SetProgress(loadingMessage.text, loadingMessage.progress);
	}

	private void StartGame() {
		GameManager gameManager = SceneUtils.FindObject<GameManager>();
		
		if(!gameManager) {
			Logger.Log ("No gamemanager found!", LogType.Warning);
			Destroy(this.gameObject);
			return;
		}
		
		gameManager.OnStart();
		Destroy(this.gameObject);
	}
}
