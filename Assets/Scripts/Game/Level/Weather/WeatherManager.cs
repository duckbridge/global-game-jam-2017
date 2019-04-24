using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour {
	
	public ParticleSystem snowParticle;
	public Animation2D rainAnimation;

	public enum WeatherState { NONE, RAIN, SNOW }
	private WeatherState weatherState = WeatherState.NONE;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableRain() {

		if(weatherState == WeatherState.NONE) {

			rainAnimation.Awake ();
			rainAnimation.Play ();

            SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.transform.Find("RainMusic").gameObject);
			this.transform.Find("RainMusic").GetComponent<SoundObject>().Play();
			weatherState = WeatherState.RAIN;

			rainAnimation.GetComponentInChildren<RainSplashes> ().StartSpawningRainDrops ();
		}
	}

	public void EnableSnow() {

		if(weatherState == WeatherState.NONE) {

			snowParticle.gameObject.SetActive(true);
			weatherState = WeatherState.SNOW;
		}
	}

	public void DisableSnow() {

		snowParticle.gameObject.SetActive(false);

		weatherState = WeatherState.NONE;
	}

	public void DisableRain() {

		this.transform.Find("RainMusic").GetComponent<SoundObject>().Stop ();

		rainAnimation.StopAndHide ();

		rainAnimation.GetComponentInChildren<RainSplashes> ().StopSpawningRainDrops ();

		weatherState = WeatherState.NONE;
	}

	public WeatherManager.WeatherState GetWeatherState() {
		return weatherState;
	}
}
