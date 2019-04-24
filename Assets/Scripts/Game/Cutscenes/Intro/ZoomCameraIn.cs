using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ZoomCameraIn : CutSceneComponent {
		
		public float zoomTime = .5f;
		public float newSize;
		public Camera cameraToUse;
		
		public override void OnActivated () {
			if(zoomTime == 0) {
				cameraToUse.orthographicSize = newSize;
				DeActivate();
			} else {
				iTween.ValueTo(this.gameObject, new ITweenBuilder().SetFromAndTo((float)cameraToUse.orthographicSize, newSize).SetTime(zoomTime).SetOnUpdate("OnZoomed").Build());
				Invoke ("DeActivate", zoomTime);
			}
		}

		public void OnZoomed(float newValue) {
			cameraToUse.orthographicSize = newValue;
		}
	}
}
