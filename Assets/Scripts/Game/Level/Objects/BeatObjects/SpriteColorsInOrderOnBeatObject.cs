using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteColorsInOrderOnBeatObject : BeatObject {

	public bool listensToManagerForIndex = true;

	public Color[] colors;
	private int currentIndex = 0;

	private SpriteRenderer targetRenderer;
	private TextMesh targetMesh;

	public override void Start() {
		base.Start ();
		targetRenderer = GetComponent<SpriteRenderer> ();
		if (!targetRenderer) {
			targetMesh = GetComponent<TextMesh> ();
		}
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
		if (targetRenderer) {
			targetRenderer.color = colors [currentIndex];
		}

		if (targetMesh) {
			targetMesh.color = colors [currentIndex];
		}
		++currentIndex;
	}

	public void SetCurrentIndex(int index) {
		this.currentIndex = index;
		if (targetRenderer) {
			targetRenderer.color = colors [currentIndex];
		}

		if (targetMesh) {
			targetMesh.color = colors [currentIndex];
		}
	}
}
