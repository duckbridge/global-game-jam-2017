using UnityEngine;
using System.Collections;

public class TextBoxManagers : MonoBehaviour {

	public TextBoxManager[] textBoxManagers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TextBoxManager GetRandomTextBox() {
		int randomTextBox = Random.Range (0, textBoxManagers.Length);
		return textBoxManagers[randomTextBox];
	}
}
