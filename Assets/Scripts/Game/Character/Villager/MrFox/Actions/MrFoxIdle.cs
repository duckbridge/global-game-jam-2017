using UnityEngine;
using System.Collections;

public class MrFoxIdle : MrFoxAction {

	protected override void OnStarted() {
		mrFox.GetAnimationManager().PlayAnimationByName("Idle", true);
		mrFox.AddEventListener(this.gameObject);
	}
}
