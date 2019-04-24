using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class WaitForBorderWallsToDie : CutSceneComponent {

		public BorderWall[] borderWalls;

		private int amountOfBorderWallsAlive;

		public override void OnActivated () {
			amountOfBorderWallsAlive = borderWalls.Length;

			for(int i = 0 ; i < borderWalls.Length; i++) {
				borderWalls[i].AddEventListener(this.gameObject);
			}
		}

		public void OnBorderWallDied() {
			--amountOfBorderWallsAlive;

			if(amountOfBorderWallsAlive <= 0) {
				DeActivate();
			}
		}
	}
}