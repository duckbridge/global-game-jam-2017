using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PenguGameManager : MonoBehaviour {

	public bool fadeInAtStart = true;
	public float animalMaxWaitTime = 10f;

	public float timeOutBeforeLoad = 3f;
	public float fadeTime = 120f;

	public GameObject onDayDoneAndLostGameObject;
	public GameObject onDayDoneGameObject;

	public string nextSceneName = "MainScene";
	public FadingAudio backgroundMusic;

	public Fading2D fading2D;
	public List<float> spawnTimes;
	private List<QueueManager> queueManagers;

	private int happyAnimalCount = 0;
	private int angryAnimalCount = 0;

	private float musicFadeSpeed = 0.01f;

	// Use this for initialization

	void Awake() {
		if (!fadeInAtStart) {
			fading2D.targetSprite.color = Color.clear;
		}
	}
	void Start () {
		if (fadeInAtStart) {
			fading2D.AddEventListener (this.gameObject);
			fading2D.FadeInto (Color.clear, fadeTime, FadeType.FADEOUT);
		} else {
			StartGame ();
		}

		queueManagers = SceneUtils.FindObjects<QueueManager> ();

		float time = PlayerPrefs.GetFloat ("BGMUSIC_TIME", -1f);
		backgroundMusic.Play (true);
		if (time != -1f) {
			backgroundMusic.GetSound ().time = time;
		} else {

		}
		backgroundMusic.FadeIn(musicFadeSpeed);
	}

	public void OnFadingDone(FadeType fadeType) {
		if (fadeType == FadeType.FADEOUT) {
			StartGame ();
		} else {
			bool playerHasWon = true;

			if (SceneUtils.FindObject<CurrencyContainer> ().GetCurrencyAmount () < 0) {
				playerHasWon = false;
			}

			if(playerHasWon) {
				Invoke ("LoadNextScene", timeOutBeforeLoad);
			} else {
				Invoke ("LoadCurrentScene", timeOutBeforeLoad);
			}
		}
	}

	private void LoadNextScene() {
		PlayerPrefs.SetFloat ("BGMUSIC_TIME", backgroundMusic.GetSound ().time);
		PlayerPrefs.Save ();
		SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
	}

	public void LoadCurrentScene() {
		PlayerPrefs.SetFloat ("BGMUSIC_TIME", backgroundMusic.GetSound ().time);
		PlayerPrefs.Save ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	private void StartGame() {
		for (int i = 0; i < spawnTimes.Count; i++) {
			if (spawnTimes [i] == 0) {
				GetRandomQueueManager ().SpawnRandomAnimal ();
			} else {
				Invoke ("SpawnAnimalForRandomQueue", spawnTimes[i]);
			}
		}
	}

	public void OnAnimalLeftRestaurant(AnimalWithInputPattern animal) {
		if (animal.IsHappy ()) {
			++happyAnimalCount;
		} else {
			++angryAnimalCount;
		}

		if ((happyAnimalCount + angryAnimalCount) >= spawnTimes.Count) {
			Logger.Log ("game is done!");
			bool playerHasWon = true;

			if (SceneUtils.FindObject<CurrencyContainer> ().GetCurrencyAmount () < 0) {
				playerHasWon = false;
			}

			if(playerHasWon) {
				onDayDoneGameObject.SetActive (true);
				onDayDoneGameObject.GetComponent<RandomGameObjectActivator> ().ActivateRandom ();
			} else {
				onDayDoneAndLostGameObject.SetActive (true);
				onDayDoneAndLostGameObject.GetComponent<RandomGameObjectActivator> ().ActivateRandom ();
			}
			Invoke ("StartFadingOut", 1f);
		}
	}

	private void StartFadingOut() {
		fading2D.AddEventListener (this.gameObject);
		fading2D.FadeInto (Color.black, fadeTime, FadeType.FADEIN);
		backgroundMusic.FadeOut(musicFadeSpeed);
	}

	// Update is called once per frame
	void Update () {
		
	}

	private void SpawnAnimalForRandomQueue() {
		GetRandomQueueManager ().SpawnRandomAnimal ();
	}

	private QueueManager GetRandomQueueManager() {
		int randomNumber = Random.Range (0, queueManagers.Count);
		return queueManagers [randomNumber];
	}
}
