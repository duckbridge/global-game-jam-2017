using UnityEngine;
using System.Collections;

public class BattleBorders : MonoBehaviour {

	public iTween.EaseType easeOutType, easeInType;
	public float iTweenAnimationTime = .5f;

	public BattleBorder[] battleBorders;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(Color newColor) {
		foreach(BattleBorder battleBorder in battleBorders) {
			battleBorder.GetComponent<SpriteRenderer>().color = newColor;
		}
	}

	public void ResetColor() {
		foreach(BattleBorder battleBorder in battleBorders) {
			battleBorder.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	public void TurnOnBattleBorders() {
		foreach(BattleBorder battleBorder in battleBorders) {
			battleBorder.ResetPosition();
			iTween.MoveTo(battleBorder.gameObject, new ITweenBuilder()
			              .SetLocal().SetPosition(battleBorder.target.localPosition)
			              .SetTime(iTweenAnimationTime).SetEaseType(easeInType).Build());

			battleBorder.GetComponent<SpriteRenderer>().enabled = true;
            battleBorder.EnableBeatPulsate();
		}
	}

	public void TurnOffBattleBorders() {
		foreach(BattleBorder battleBorder in battleBorders) {
			iTween.MoveTo(battleBorder.gameObject, new ITweenBuilder()
			              .SetLocal().SetPosition(battleBorder.GetOriginalPosition())
			              .SetTime(iTweenAnimationTime).SetEaseType(easeOutType).Build());

            battleBorder.DisableBeatPulsate();
		}
	}
}
