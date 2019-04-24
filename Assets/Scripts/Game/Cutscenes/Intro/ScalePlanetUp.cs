using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ScalePlanetUp : ScaleObjectUp {

		public override void OnActivated () {
			iTween.ScaleTo(objectToScale.gameObject, new ITweenBuilder().SetScale(newScale).SetTime(scaleTime).Build());
			Invoke("DoneGrowing", scaleTime);
		}
		private void DoneGrowing() {
			objectToScale.GetComponent<BeatObject>().Initialize();
			DeActivate();
		}
	
	}
}
