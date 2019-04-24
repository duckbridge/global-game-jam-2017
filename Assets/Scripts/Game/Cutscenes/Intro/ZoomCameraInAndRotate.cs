using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ZoomCameraInAndRotate : ZoomCameraIn {
		
		public Vector3 rotateAmount = new Vector3(0f, 0f, 30f);
		
		public override void OnActivated () {
			if (zoomTime == 0) {
				cameraToUse.transform.localEulerAngles = rotateAmount;
			} else {
				iTween.RotateTo (cameraToUse.gameObject, new ITweenBuilder().SetRotation(rotateAmount).SetLocal().SetTime(zoomTime).Build());
			}
			base.OnActivated ();
		}
	}
}
