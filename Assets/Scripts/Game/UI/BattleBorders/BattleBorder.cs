using UnityEngine;
using System.Collections;

public class BattleBorder : MonoBehaviour {

	public Transform target;
	private Vector3 originalLocalPosition;

	// Use this for initialization
	void Awake () {
		originalLocalPosition = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetPosition() {
		this.transform.localPosition = originalLocalPosition;	
	}

	public Vector3 GetOriginalPosition() {
		return this.originalLocalPosition;
	}

    public void EnableBeatPulsate() {
        GetComponent<BeatObject>().Activate();
    }

    public void DisableBeatPulsate() {
        GetComponent<BeatObject>().Deactivate();
    }
}
