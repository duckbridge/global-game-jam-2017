using UnityEngine;
using System.Collections;

public class RandomFlicker2D : MonoBehaviour {

	public float flickerMinTimeout = .5f;
	public float flickerMaxTimeout = 1f;

	private float flickerTimeout;

	public float minNextFlickerTime = 2f;
	public float maxNextFlickerTime = 5f;

	private float nextFlickerTime;

	public SpriteRenderer sprite;
	private bool isFlickering = false;

	// Use this for initialization
	void Awake () {

		flickerTimeout = Random.Range(flickerMinTimeout, flickerMaxTimeout);
		nextFlickerTime = Random.Range(minNextFlickerTime, maxNextFlickerTime);
		if(!sprite) {
			sprite = this.GetComponent<SpriteRenderer>();
		}

		StartFlicker();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void Flicker() {
		if(isFlickering) {
			sprite.GetComponent<Renderer>().enabled = true;
			Invoke("Hide", nextFlickerTime);
		}
	}

	private void Hide() {
		sprite.GetComponent<Renderer>().enabled = false;
		Invoke("StartFlicker", flickerTimeout);
	}

	private void Show() {
		sprite.GetComponent<Renderer>().enabled = true;
	}

	public void StartFlicker() {
		isFlickering = true;
		Flicker();
	}

	public void StopFlicker() {
		CancelInvoke("Hide");
		CancelInvoke("Flicker");

		isFlickering = false;
		Show();
	}
}
