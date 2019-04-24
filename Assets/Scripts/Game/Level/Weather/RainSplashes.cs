using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RainSplashes : MonoBehaviour {

	public int amountOfRaindrops = 7;

	private float xMin, xMax, yMin, yMax;
	private List<Animation2D> rainSplashes = new List<Animation2D>();
	private bool isEnabled = false;

	void Awake () {
		xMin = this.transform.Find("xMin").localPosition.x;
		xMax = this.transform.Find("xMax").localPosition.x;

		yMin = this.transform.Find("yMin").localPosition.z;
		yMax = this.transform.Find("yMax").localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartSpawningRainDrops() {
		isEnabled = true;

		if(rainSplashes.Count == 0) {
			SpawnNewRainDrops(amountOfRaindrops);
		} else {
			ToggleAllRainDrops(true);
		}
	}

	public void StopSpawningRainDrops() {
		if(isEnabled) {
			isEnabled = false;

			ToggleAllRainDrops(false);
		}
	}

	private void ToggleAllRainDrops(bool enableRainDrops) {
		for(int i = 0 ; i < rainSplashes.Count ; i++) {
			if(enableRainDrops) {
				rainSplashes[i].AddEventListener(this.gameObject);
				rainSplashes[i].PlayDelayed(Random.Range (0f, 1f));
			} else {
				rainSplashes[i].RemoveEventListener(this.gameObject);
				rainSplashes[i].StopAndHide();
			}
		}
	}

	private void SpawnNewRainDrops(int amountOfRaindropsToSpawn) {
		for(int i = 0 ; i < amountOfRaindropsToSpawn; i++) {
			Animation2D rainSplash = (Animation2D) GameObject.Instantiate(Resources.Load("Rooms/Rainsplash", typeof(Animation2D)), this.transform.position, Quaternion.Euler(90f, 0f, 0f));
			rainSplash.transform.parent = this.transform;
			rainSplash.transform.localPosition = GetRandomPosition();

			rainSplashes.Add (rainSplash);
		}

		ToggleAllRainDrops(true);
	}

	private Vector3 GetRandomPosition() {
		return new Vector3(Random.Range (xMin, xMax), 0f, Random.Range (yMin, yMax));
	}

	public void OnAnimationDone(Animation2D animation2D) {
		if(isEnabled) {
			animation2D.transform.localPosition = GetRandomPosition();
			animation2D.PlayDelayed(Random.Range (0f, 1f));
		}
	}
}
