using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class StartRotatingPlanet : CutSceneComponent {

		public Planet planet;

		public override void OnActivated () {
			planet.StartRotating();
			Invoke("DeActivate", .5f);
		}
	}
}
