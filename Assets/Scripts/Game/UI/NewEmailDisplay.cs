using UnityEngine;
using System.Collections;

public class NewEmailDisplay : MonoBehaviour {

    public float displayTimeout = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show() {

        Transform displayTransform = this.transform.Find("Display");

        displayTransform.gameObject.SetActive(true);
        displayTransform.GetComponent<Animation2D>().Play(true);

        Invoke("HideSaveDisplay", displayTimeout);

    }

    private void HideSaveDisplay() {
        Transform displayTransform = this.transform.Find("Display");
        displayTransform.gameObject.SetActive(false);
    }
}
