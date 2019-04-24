using UnityEngine;
using System.Collections;

public class OnChanceSlomoComponent : SlomoComponent {

	public int chanceToSlomo = 100;

	public override void DoSlomo (bool slomoSound = true) {
		int randomNumber = Random.Range (0, 100);
		if (chanceToSlomo >= randomNumber) {
			base.DoSlomo (slomoSound);
		}
	}

	public override void DoSlomoIndependent (bool slomoSound = true) {
		int randomNumber = Random.Range (0, 100);
		if (chanceToSlomo >= randomNumber) {
			base.DoSlomoIndependent (slomoSound);
		}
	}
}
