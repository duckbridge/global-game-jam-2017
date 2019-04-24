using UnityEngine;
using System.Collections;

public class BasicBeatObject : BeatObject {

	public float pulsateTime = .5f;
	public Vector3 pulsateAmount = new Vector3(.5f, .5f, .5f);
	
    protected Vector3 originalScale;

	public void Awake() {
		Initialize();
	}

	public override void Initialize() {
		originalScale = this.transform.localScale;
	}

	public override void OnBeatEvent () {
		iTween.StopByName(this.gameObject, "Bounce");
		this.transform.localScale = originalScale;
		iTween.PunchScale(this.gameObject, new ITweenBuilder().SetName("Bounce").SetAmount(pulsateAmount).SetTime(pulsateTime).Build());
	}
}
