using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuButtonColorsInOrderOnBeatObject : BeatObject {

	public bool listensToManagerForIndex = true;

	public Color[] colors;
	private int currentIndex = 0;
	private MenuButtonWithColors menuButton;

	public override void Start() {
		base.Start ();
		menuButton = GetComponent<MenuButtonWithColors> ();

		SyncCurrentIndex ();
	}

	public void SyncCurrentIndex() {
		if (listensToManagerForIndex) {
			SetCurrentIndex (SceneUtils.FindObject<BeatColorIndexManager> ().GetCurrentIndex ());
		}
	}

	public override void OnBeatEvent () {

		if (listensToManagerForIndex) {
			return;
		}

		if (currentIndex >= colors.Length) {
			currentIndex = 0;
		}
		menuButton.SetOriginalColor (colors [currentIndex]);
		++currentIndex;
	}

	public void SetCurrentIndex(int index) {
		this.currentIndex = index;

		if (!menuButton) {
			menuButton = GetComponent<MenuButtonWithColors> ();
		}

		menuButton.SetOriginalColor (colors [currentIndex]);
	}
}
