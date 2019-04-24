using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatColorIndexManager : BeatObject {

	public int maxIndex = 3;
	private int currentIndex = 0;

	public override void OnBeatEvent () {

		++currentIndex;

		if (currentIndex >= maxIndex) {
			currentIndex = 0;
		}

		SceneUtils.FindObjects<MenuButtonColorsInOrderOnBeatObject> ()
			.ForEach (mbco => mbco.SetCurrentIndex (currentIndex));
		SceneUtils.FindObjects<SpriteColorsInOrderOnBeatObject> ()
			.ForEach (sco => sco.SetCurrentIndex (currentIndex));
	}

	public int GetCurrentIndex() {
		return currentIndex;
	}
}
