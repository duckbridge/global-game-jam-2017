using UnityEngine;
using System.Collections;

public class MoleEnemy : Enemy {

	public override void Awake () {
		base.Awake ();
		HideHealthbar();
	}
}
