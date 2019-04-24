using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TextCutsceneWhichCentersInCamera : TextCutSceneComponent {

		public string cameraName = "GameCamera";
		public Vector3 newLocalPosition = new Vector3(0f, 0f, 3f);

		private Transform originalParent;
		private Vector3 originalLocalPosition;

		public override void OnActivated () {

			originalParent = textBoxManager.transform.parent;
			originalLocalPosition = textBoxManager.transform.localPosition;

			textBoxManager.transform.parent = GameObject.Find(cameraName).transform;
			textBoxManager.transform.localPosition = newLocalPosition;

			base.OnActivated ();
		}

		public override void OnDeActivated () {
			base.OnDeActivated ();

			textBoxManager.transform.parent = originalParent;
			textBoxManager.transform.localPosition = originalLocalPosition;
		}
	}
}
