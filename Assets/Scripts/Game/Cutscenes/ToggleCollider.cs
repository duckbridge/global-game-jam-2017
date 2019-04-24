using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ToggleCollider : CutSceneComponent {
		
		public Collider[] collidersToToggle;
		public bool enableCollider = true;
		
		public override void OnActivated () {

			for(int i = 0; i < collidersToToggle.Length ; i++) {
				collidersToToggle[i].enabled = enableCollider;
			}

			DeActivate();
		}
	}
}
