using UnityEngine;
using System.Collections;

public class TileBeatMine : ToggleBeatObject {

	public SpriteRenderer normalSprite, onToggledSprite;
	public Collider collider;
	
	public void Awake() {}
	
	protected override void OnFirstStateEntered () {
		normalSprite.enabled = true;
		onToggledSprite.enabled = false;
		collider.enabled = false;
	}
	
	protected override void OnSecondStateEntered () {
		normalSprite.enabled = false;
		onToggledSprite.enabled = true;
		collider.enabled = true;
	}
}
