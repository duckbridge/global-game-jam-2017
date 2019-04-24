using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class UnityAnimationRepeater : MonoBehaviour {

	public float minimumAnimationTimeout;
	public float maximumAnimationTimeout;

	// Use this for initialization
	void Start () {
		Invoke ("PlayAnimation", Random.Range (minimumAnimationTimeout, maximumAnimationTimeout));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnAnimationDone() {
		Invoke ("PlayAnimation", Random.Range (minimumAnimationTimeout, maximumAnimationTimeout));
	}

	private void PlayAnimation() {
		GetComponent<Animation>().Play();
	}
}
