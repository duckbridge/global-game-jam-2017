using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Loader : DispatchBehaviour {

	public static LoadingScreenType loadingScreenType = LoadingScreenType.menu_default;
	public static bool IS_USING_LOADER = false;
	public static bool HAS_DONE_FULL_RELOAD = false;

	private static bool HAS_USED_LOADER = false;
	private enum LoaderState { PreLoading, Loading, Done, None}

	private static Scene loadingScene = Scene.NONE;

	public virtual void Start() {
		if(transform.parent) {
			Debug.Log("[LOADER] WARNING! Loader should NOT have a parent object!");
		}
		StartLoading();
	}

	void Update() {
	}

	IEnumerator AsynchronousLoad (string scene) {
		yield return null;

		AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
		ao.allowSceneActivation = false;
		
		Logger.Log("Start loading");

		while (! ao.isDone) {
			float progress = Mathf.Clamp01(ao.progress / 0.9f);
			
			OnLoadingProgressing(progress);

			if (ao.progress == 0.9f) {
				ao.allowSceneActivation = true;
			}

			yield return null;
		}

		OnLoadingDone();
	}

	protected virtual void OnLoadingProgressing(float progress) {

	}

	private void StartLoading() {
		float fxVolume = PlayerPrefs.GetFloat(GameSettings.FX_SAVE_NAME, GameSettings.DEFAULT_FX_VOLUME);
		SoundUtils.SetSoundVolume(SoundType.FX, fxVolume);
	
		DontDestroyOnLoad(transform.gameObject);
		StartCoroutine(AsynchronousLoad(loadingScene.ToString()));

	}

	public static void LoadScene(Scene scene, LoadingScreenType loadingScreenType) {

		PlayerInputHelper.ResetInputHelper ();

		PauseHelper.ResumeGame();
		Time.timeScale = 1f;
		
		Loader.loadingScreenType = loadingScreenType;
		Loader.IS_USING_LOADER = true;
		Loader.HAS_USED_LOADER = true;
		Loader.HAS_DONE_FULL_RELOAD = true;

		loadingScene = scene;

		SceneManager.LoadScene(Scene.Empty.ToString(), LoadSceneMode.Single);
		SceneManager.LoadScene(Scene.Loading.ToString(), LoadSceneMode.Additive);
	}

	public static void LoadSceneQuickly(Scene scene) {

		PlayerInputHelper.ResetInputHelper ();

		PauseHelper.ResumeGame();
		Time.timeScale = 1f;

		Loader.IS_USING_LOADER = false;
		Loader.HAS_DONE_FULL_RELOAD = false;

		loadingScene = scene;

		SceneManager.LoadScene (scene.ToString());
	}

	public static void ReloadLevelWithoutLoadingScene(bool isUsingLoader = true) {

		PlayerInputHelper.ResetInputHelper ();

		PauseHelper.ResumeGame();
		Time.timeScale = 1f;

		string sceneToLoad = loadingScene.ToString();

		if(!Loader.HAS_USED_LOADER) {
			sceneToLoad = Application.loadedLevelName;
		}

		Loader.IS_USING_LOADER = isUsingLoader;

		HAS_DONE_FULL_RELOAD = false;
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
	}
	
	public static void ReloadLevel() {

		string sceneToLoad = loadingScene.ToString();
		if(!Loader.HAS_USED_LOADER) {
			sceneToLoad = Application.loadedLevelName;
		}

		Scene scene = (Scene) System.Enum.Parse(typeof(Scene), sceneToLoad);
		HAS_DONE_FULL_RELOAD = true;

		Loader.LoadScene(scene, Loader.loadingScreenType);
	}

	public static void ReloadLevelAndStopLoading() {
		PlayerInputHelper.ResetInputHelper ();
		Destroy(SceneUtils.FindObject<Loader>().gameObject);

		string sceneToLoad = loadingScene.ToString();
		if(!Loader.HAS_USED_LOADER) {
			sceneToLoad = Application.loadedLevelName;
		}
		
		Scene scene = (Scene) System.Enum.Parse(typeof(Scene), sceneToLoad);
		HAS_DONE_FULL_RELOAD = true;
		
		Loader.LoadScene(scene, Loader.loadingScreenType);
	}

	public static Scene GetLoadingScene() {
		return loadingScene;
	}

	public virtual void OnLoadingDone() {
		Destroy (this.gameObject);
	}
}
