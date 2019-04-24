using UnityEngine;
using System.Collections;

public class TogglePlayerShadow : CutSceneComponent {

	public bool enableShadow = true;

	public override void OnActivated () {
		SceneUtils.FindObject<Player>().transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = enableShadow;

		DeActivate();
	}
}
