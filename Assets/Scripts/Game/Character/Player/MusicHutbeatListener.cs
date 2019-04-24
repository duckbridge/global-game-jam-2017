using UnityEngine;
using System.Collections;

public class MusicHutbeatListener : BeatListener {

	public SoundObject musicToListenTo;

	// Update is called once per frame
	public override void Update () {
		if(isListeningToAudio) {
			
			if(musicToListenTo.GetSound().isPlaying) {
				
				float oldSongPositionInMs = songPositionInMs;
				
				songPositionInMs = (int)((musicToListenTo.GetSound().timeSamples / 44100.0f - 0) * 1000); //maybe change 44100.0f and get it from python program
				
				if(oldSongPositionInMs > songPositionInMs) {
					lastUsedIndex = -1;
				}
				
				if(CanDoBeat(true)) {
					DispatchMessage("OnBeatEvent", null);
				}
			} 
		}
	}
}
