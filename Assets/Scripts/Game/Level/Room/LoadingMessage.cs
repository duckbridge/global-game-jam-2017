using UnityEngine;
using System.Collections;

public struct LoadingMessage {

	public string text;
	public int progress;

	public LoadingMessage(string text, int progress) {
		this.text = text;
		this.progress = progress;
	}
}
