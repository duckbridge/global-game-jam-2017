using UnityEngine;
using System.Collections;

public class ScreenshotFlash : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowFlash() {
        GetComponent<Animation2D>().Play(true);
        GetComponentInChildren<SoundObject>().Play();
    }
}
