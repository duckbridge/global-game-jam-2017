using UnityEngine;
using System.Collections;

public class MinimapBlock : MonoBehaviour {

	private string fullBlockName = "default";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeVisible() {
		if(GetComponent<SpriteRenderer>()) {
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	public void MakeHidden() {
		if(GetComponent<SpriteRenderer>()) {
			GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void SetFullBlockName(string blockName) {
		this.fullBlockName = blockName;
	}

	public string GetFullBlockName() {
		return this.fullBlockName;
	}

	public void SetSprite(Sprite newSprite) {
		GetComponent<SpriteRenderer>().sprite = newSprite;
	}

	public Sprite GetSprite() {
		return GetComponent<SpriteRenderer>().sprite;
	}

}
