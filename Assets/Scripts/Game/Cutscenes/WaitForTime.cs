using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class WaitForTime : CutSceneComponent {

		public float waitTime = 1f;

		public override void OnActivated () {
			Invoke ("DeActivate", waitTime);
		}
	}
}