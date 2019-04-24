using UnityEngine;
using System.Collections;

public class ObjectThatUsesMinimapColor : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(Color newColor) {
		if(GetComponent<TextMesh>()) {
			GetComponent<TextMesh>().color = newColor;
		}

		if(GetComponent<SpriteRenderer>()) {
			GetComponent<SpriteRenderer>().color = newColor;
		}
	}
}
