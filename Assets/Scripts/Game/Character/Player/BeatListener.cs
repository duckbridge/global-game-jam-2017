using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatListener : DispatchBehaviour {

	public int onBeatRange = 100;
	public MusicManager musicManager;
	
	protected int[] beatTimesInMs;
	
	protected int songPositionInMs;
	protected bool isListeningToAudio = false;

	protected int lastUsedIndex = -1;
	protected Dictionary<string, int[]> beatTimesByFileName = new Dictionary<string, int[]>();

	void Start () {
	}

	public void Initialize(string musicName, int onBeatRange) {
		isListeningToAudio = false;
		this.onBeatRange = onBeatRange;
		LoadBeatTimesFromFile(musicName);
		isListeningToAudio = true;
	}

	// Update is called once per frame
	public virtual void Update () {
		if(isListeningToAudio) {
			
			if(musicManager.GetCurrentMusic().GetSound().isPlaying) {

				float oldSongPositionInMs = songPositionInMs;

				songPositionInMs = (int)((musicManager.GetCurrentMusic().GetSound().timeSamples / 44100.0f - 0) * 1000); //maybe change 44100.0f and get it from python program

				if(oldSongPositionInMs > songPositionInMs) {
					lastUsedIndex = -1;
				}

				if(CanDoBeat(true)) {
					DispatchMessage("OnBeatEvent", null);
				}
			} 
		}
	}
	
	private void LoadBeatTimesFromFile(string fileName) {

		int[] foundBeatTimes;
		beatTimesByFileName.TryGetValue(fileName, out foundBeatTimes);

		if(foundBeatTimes != null) {

			beatTimesInMs = foundBeatTimes;

		} else {
			TextAsset textAsset = Resources.Load("InternalData/"+fileName) as TextAsset;
			string text = textAsset.text;
			string[] splittedData = text.Split(',');
			
			beatTimesInMs = new int[splittedData.Length];
			
			for(int i = 0 ; i < splittedData.Length ; i++) {
				beatTimesInMs[i] = System.Convert.ToInt32(splittedData[i]);
			}

			beatTimesByFileName.Add (fileName, beatTimesInMs);
		}
	}

	public bool CanDoBeat(bool useLastUsedIndex) {

		if(beatTimesInMs != null) {
			for(int i = 0 ; i < beatTimesInMs.Length ; i++) {

				int timeDiff = Mathf.Abs (beatTimesInMs[i] - songPositionInMs);
				if(timeDiff < onBeatRange) {

					if(useLastUsedIndex) {

						if(i > lastUsedIndex) {
							lastUsedIndex = i;
							return true;
						}

					} else {
						return true;
					}
				}
			} 
		}

		return false;
	}
}