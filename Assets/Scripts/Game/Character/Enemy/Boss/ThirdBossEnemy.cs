using UnityEngine;
using System.Collections;

public class ThirdBossEnemy : BossEnemy {

	protected override void OnReallyDied () {}

	protected override void OnDie () {
        animationManager.EnableSwitchAnimations();
		base.OnDie ();
	}
}
