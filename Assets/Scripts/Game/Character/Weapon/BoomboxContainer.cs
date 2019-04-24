using UnityEngine;
using System.Collections;

public class BoomboxContainer : MonoBehaviour {

	private SpriteRenderer shadowSprite;
	private Vector3 originalShadowSpritePosition;

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveShadowsBy(float amount) {
		shadowSprite.transform.position += new Vector3(0f, 0f, amount);
	}

	public void ShowShadow(Direction directionToShow) { 

		if(shadowSprite) {
			shadowSprite.enabled = false;
		}

		shadowSprite = this.transform.Find("Shadows/" + directionToShow.ToString()).GetComponent<SpriteRenderer>();
		shadowSprite.enabled = true;
		originalShadowSpritePosition = shadowSprite.transform.position;
	}
	
	public void OnRetracting() {
		Invoke ("ResetSprites", .1f);
	}

	private void ResetSprites() {
		shadowSprite.transform.position = originalShadowSpritePosition;
	}
}
