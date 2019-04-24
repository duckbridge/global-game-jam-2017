using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

namespace Cutscenes {
	public class ToggleCameraBlackWhite : CutSceneComponent {

		public bool enableBlackWhite = true;

        public override void OnActivated () {

			GameObject.Find ("UICameraContainer/UICamera").GetComponent<Grayscale> ().enabled = enableBlackWhite;

            DeActivate();
        }

    }
}