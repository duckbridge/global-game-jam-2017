using UnityEngine;
using System.Collections;

public class BorderBeatObject : BasicBeatObject {
	
    public override void OnBeatEvent () {

        iTween.StopByName(this.gameObject, "Bouncing");
        this.transform.localScale = originalScale;
        Invoke("DoTweenDelayed", .1f);    

    }

    private void DoTweenDelayed() {
        iTween.PunchScale(this.gameObject, new ITweenBuilder().SetName("Bouncing").SetAmount(pulsateAmount).SetTime(pulsateTime).Build()); 
    }
}
