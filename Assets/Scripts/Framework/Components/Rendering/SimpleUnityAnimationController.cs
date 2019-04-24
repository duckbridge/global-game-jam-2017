using UnityEngine;
using System.Collections;

public class SimpleUnityAnimationController : MonoBehaviour {

	public string clipName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play() {
		this.GetComponent<Animation>().Play(clipName);
	}

	public void DoPause() {
		this.GetComponent<Animation>()[clipName].speed = 0f;
	}

	public void DoResume() {
		this.GetComponent<Animation>()[clipName].speed = 1f;
	}
}
