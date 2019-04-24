using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ScaleObjectUp : CutSceneComponent {

		public float scaleTime = .5f;
		public Vector3 newScale;
		public Transform objectToScale;

		public override void OnActivated () {
			iTween.ScaleTo(objectToScale.gameObject, new ITweenBuilder().SetScale(newScale).SetTime(scaleTime).Build());
			Invoke("DeActivate", scaleTime);
		}
	}
}
