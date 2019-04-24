using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation2D))]
public class RoomDecoration : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show() {
        GetComponent<Animation2D>().Play(true);
    }

    public void Hide() {
        GetComponent<Animation2D>().StopAndHide();
    }
}
