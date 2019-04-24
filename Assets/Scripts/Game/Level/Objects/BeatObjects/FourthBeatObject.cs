using UnityEngine;
using System.Collections;

public class FourthBeatObject : ToggleBeatObject {

	protected override void OnStateEntered(int stateIndex) {
		switch(stateIndex) {
			case 0:
				iTween.MoveBy(this.gameObject, new ITweenBuilder().SetAmount(new Vector3(0f, 0f, 3)).SetTime(.5f).Build());
			break;

			case 1:
				iTween.MoveBy(this.gameObject, new ITweenBuilder().SetAmount(new Vector3(0f, 0f, 3)).SetTime(.5f).Build());
			break;

			case 2:
				iTween.MoveBy(this.gameObject, new ITweenBuilder().SetAmount(new Vector3(0f, 0f, 3)).SetTime(.5f).Build());
			break;
				
			case 3:
				iTween.MoveBy(this.gameObject, new ITweenBuilder().SetAmount(new Vector3(0f, 0f, -9)).SetTime(.5f).Build());
			break;
		}
	}
}
