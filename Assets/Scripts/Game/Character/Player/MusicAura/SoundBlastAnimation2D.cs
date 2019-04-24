using UnityEngine;
using System.Collections;

public class SoundBlastAnimation2D : Animation2D {

	public MusicAura musicAura;
	public int maxLastFrameOverride = 5;
	public int lastFrameOverride = -1;

	public void SetLastFrameOverride(int newLastFame) {
		this.lastFrameOverride = newLastFame;
	}

	public int GetLastFrameOverride() {
		return lastFrameOverride;
	}

	public void ResetLastFrameOverride() {
		SetLastFrameOverride(maxLastFrameOverride);
	}

	public void OnListenerTrigger(Collider coll) {
		musicAura.OnTriggerEnter(coll);
	}
}
