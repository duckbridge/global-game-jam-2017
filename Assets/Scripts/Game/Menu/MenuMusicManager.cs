using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuMusicManager : MonoBehaviour {

	public SoundObjectWithInfo[] tracksAvailableByDefault;

	public TileType tileTypeToPlay = TileType.specialVillage;

	private MusicHutbeatListener beatListener;

	private List<SoundObjectWithInfo> songTileTypesAvailable = new List<SoundObjectWithInfo>();
	private int currentSongIndex = 0;
	private SoundObjectWithInfo currentTrack;
	private SoundObject playSound, stopSound;

	private PlayerSaveComponent playerSaveComponent;
	private List<TileType> allUnlockedTileTypes = new List<TileType>();
	
	// Use this for initialization
	void Awake () {

		playSound = this.transform.Find("CassetteSounds/Play").GetComponent<SoundObject>();
		stopSound = this.transform.Find("CassetteSounds/Stop").GetComponent<SoundObject>();

		songTileTypesAvailable.AddRange(tracksAvailableByDefault);

		playerSaveComponent = GetComponent<PlayerSaveComponent>();
		allUnlockedTileTypes = playerSaveComponent.LoadUnlockedTileTypeTracksOfAllSlots();

		foreach(SoundObjectWithInfo soundObject in GetComponentsInChildren<SoundObjectWithInfo>()) {

			if(allUnlockedTileTypes.Contains(soundObject.tileType)) {
				if(!songTileTypesAvailable.Contains(soundObject)) {
					songTileTypesAvailable.Add (soundObject);
				}
			}
		}

		beatListener = GetComponent<MusicHutbeatListener>();

		Invoke ("PlayTrackAtCurrentIndex", .2f);
	}

	private void PlayTrackAtCurrentIndex() {

		if(currentTrack && songTileTypesAvailable.Count > 0) {
			currentTrack.Stop ();
			stopSound.Play();

			SoundObjectWithInfo oldTrack = songTileTypesAvailable [GetPreviousIndex ()];
			currentTrack = songTileTypesAvailable[currentSongIndex];

			SceneUtils.FindObject<BoomboxMusicSwapUI> ().OnMusicSwap (currentTrack, oldTrack);

			Invoke ("BeforePlayNextTrack", .3f);
		
		} else {
			
			currentTrack = songTileTypesAvailable[currentSongIndex];
			SceneUtils.FindObject<BoomboxMusicSwapUI> ().OnMusicSwap (currentTrack);
			PlayCurrentTrack();
		}
	}

	private void BeforePlayNextTrack() {
		playSound.Play();
		Invoke ("PlayCurrentTrack", .3f);
	}

	private void PlayCurrentTrack() {
		beatListener.musicToListenTo = currentTrack;
		beatListener.Initialize(currentTrack.GetSound().clip.name, 100);
		
		currentTrack.PlayScheduled(AudioSettings.dspTime);
	}

	public void SwapToNextTrack() {
		currentSongIndex = GetNextIndex ();
		PlayTrackAtCurrentIndex();
	}

	public void SwapToPreviousTrack() {
		currentSongIndex = GetPreviousIndex ();
		PlayTrackAtCurrentIndex();
	}

	private int GetNextIndex() {
		int currentIndex = currentSongIndex;

		currentIndex++;
		if(currentIndex >= songTileTypesAvailable.Count) {
			currentIndex = 0;
		}

		return currentIndex;
	}

	private int GetPreviousIndex () {
		int currentIndex = currentSongIndex;

		currentIndex--;
		if(currentIndex < 0) {
			currentIndex = songTileTypesAvailable.Count-1;
		}

		return currentIndex;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
