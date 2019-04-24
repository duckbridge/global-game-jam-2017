using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

	public TileType overrideMusicTileTypeToPlay = TileType.none;

	public Cassette cassetteGameObject;

	public BeatListener beatListener;
	public float fadeInOutSpeed = .05f;

	private Dictionary<TileType, SoundObjectWithInfo> musicByTileType;
	private Transform levelMusicContainer;

	private SoundObjectWithInfo currentMusic;
	private SoundObjectWithInfo soundToPlay;

	private bool isMuted = false;

	private MusicAura musicAura;

	private SoundObject playSound, stopSound, takeOutSound;

	private TileType currentMusicTileType = TileType.none;

	private List<TileType> songTileTypesAvailable = new List<TileType>();
	private int currentSongIndex = 0;

	private bool isBusy = false;
	private Transform targetAboveManager;
	private bool isInitialized = false;

	private List<TileType> tracksUnlockedByTileType = new List<TileType>();

	private PlayerSaveComponent playerSaveComponent;
	private PowerUpComponent powerupComponent;

	// Use this for initialization
	void Awake () {

		Initialize ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void PlayMusicByTileTypeInstantly(TileType musicTileType) {
		soundToPlay = GetMusicByTileType(musicTileType);

		if(soundToPlay) {
			
			CancelInvoke("PlayOnPlaySound");
			CancelInvoke("PlayNewMusic");
			
			if(currentMusic) {
				
				currentMusic.Stop();
				stopSound.Play();

				Invoke ("PlayNewMusicOnReceive", .3f);
			} else {
				PlayNewMusicOnReceive();
			}
		}
	}

	public void PlayMusicByTileType(TileType tileType, bool force = false) {

		if(HasUnlockedNewTrackForTileType(tileType)) {
            soundToPlay = GetMusicByTileType(tileType);
			
			if(soundToPlay) {

				if (currentMusic && currentMusic == soundToPlay && !force) {
					Logger.Log ("same track");
					return;
				}

				CancelInvoke("PlayOnPlaySound");
				CancelInvoke("PlayNewMusic");

				powerupComponent.SetBlastDamageIncrementAmount(soundToPlay.sphereBlastPower, soundToPlay.crossBlastPower, soundToPlay.diamondBlastPower, soundToPlay.bulletBlastPower);
				powerupComponent.SetTinyBlastDamageIncrementAmount(soundToPlay.tinySphereBlastPower, soundToPlay.tinyCrossBlastPower,  soundToPlay.tinyDiamondBlastPower, soundToPlay.tinyBulletBlastPower);

				if(currentMusic) {

					isBusy = true;

					currentMusic.Stop();
					stopSound.Play();

					SceneUtils.FindObject<BoomboxMusicSwapUI>().OnMusicSwap(soundToPlay, currentMusic);

				} else {
					SceneUtils.FindObject<BoomboxMusicSwapUI>().OnMusicSwap(soundToPlay);
				}
			}
		}
	}

	private bool HasUnlockedNewTrackForTileType(TileType tileType) {
		if(tileType == TileType.none) {
			return false;
		}

		if(tileType == TileType.one || tileType == TileType.boss || tileType == TileType.specialVillage) {
			return true;
		} else if(tileType == overrideMusicTileTypeToPlay) {
			return true;
		} else {
			return playerSaveComponent.GetUnlockedTileTypeTracks().Contains(tileType);
		}
	}

	public void StopPlayingMusic() {
		CancelInvoke("PlayOnPlaySound");
		CancelInvoke("PlayNewMusic");

		if(currentMusic) {
			StopCurrentMusic();
			stopSound.Play();

			currentMusicTileType = TileType.none;
		}

        BoomBox boombox = SceneUtils.FindObject<BoomBox>();
        if(boombox && boombox.isOnPlayerBack) {
            boombox.StopEmitting();
        }
	}

    public void StopCurrentMusic() {
        currentMusic.Stop();
    }

    public void PlayCurrentMusic() {
        currentMusic.Play();
    }


	public void ThrowCassetteAtBoombox(Color cassetteColor) {
		Cassette cassette = (Cassette) GameObject.Instantiate(cassetteGameObject, this.transform.position, Quaternion.identity);
		BoomBox boomBox = SceneUtils.FindObject<BoomBox>();

		cassette.SetForegroundColor(cassetteColor);

		if(boomBox.isOnPlayerBack) {
			cassette.FlyToBoomBox(GetTargetAboveManager());
		} else {
			cassette.FlyToBoomBox(null);
		}
	}

	public void OnArrivedAtDestination(Cassette cassetteGO) {
		Destroy(cassetteGO.gameObject);
	}

	public void PlayNewMusicOnReceive() {
		playSound.Play();
		Invoke ("PlayNewMusic", .3f);
	}

	private void PlayNewMusic() {
		soundToPlay.Stop ();
		soundToPlay.PlayScheduled(AudioSettings.dspTime);
		
		beatListener.Initialize(soundToPlay.GetSound().clip.name, soundToPlay.onBeatRange);

		currentMusicTileType = soundToPlay.tileType;
		currentMusic = soundToPlay;

        BoomBox boombox = SceneUtils.FindObject<BoomBox>();
        if(boombox && boombox.isOnPlayerBack) {
            boombox.StartEmitting();
        }

		isBusy = false;
	}


	public void FadeOutCurrentMusic() {
		if(currentMusic) {
			currentMusic.FadeOut(fadeInOutSpeed);
		}
	}

	public void MuteUnMuteMusic() {
		if(!isMuted) {
			currentMusic.Mute();
			isMuted = true;
		
		} else {
			currentMusic.UnMute();

			if(!currentMusic.GetSound().isPlaying) {
				currentMusic.Play();
			}

			isMuted = false;
		}
	}

	public SoundObjectWithInfo GetCurrentMusic() {
		return currentMusic;
	}

	public bool HasMusicMuted() {
		return isMuted;
	}

	public TileType GetCurrentMusicTileType() {

		if(overrideMusicTileTypeToPlay != TileType.none) {
			return overrideMusicTileTypeToPlay;
		}

		if(currentMusic) {
			return currentMusicTileType;
		} else {
			return TileType.none;
		}
	}

	public void PlayNextTrack() {
		++currentSongIndex;
		if(currentSongIndex >= songTileTypesAvailable.Count) {
			currentSongIndex = 0;
		}

		PlayMusicByTileType(songTileTypesAvailable[currentSongIndex]);
	}

	public void PlayPreviousTrack() {
		--currentSongIndex;
		if(currentSongIndex <= -1) {
			currentSongIndex = songTileTypesAvailable.Count - 1;
		}
		
		PlayMusicByTileType(songTileTypesAvailable[currentSongIndex]);
	}

	public void Initialize() {

		if(!isInitialized) {

			isInitialized = true;

			targetAboveManager = this.transform.Find("TargetAboveManager");

			powerupComponent = SceneUtils.FindObject<PowerUpComponent>();

			playSound = this.transform.Find("CassetteSounds/Play").GetComponent<SoundObject>();
			stopSound = this.transform.Find("CassetteSounds/Stop").GetComponent<SoundObject>();
			takeOutSound = this.transform.Find("CassetteSounds/TakeOut").GetComponent<SoundObject>();
			
			musicAura = SceneUtils.FindObject<MusicAura>();
			
			levelMusicContainer = this.transform.Find("MusicContainer");
			musicByTileType = new Dictionary<TileType, SoundObjectWithInfo>();
			
			foreach(SoundObjectWithInfo soundObject in levelMusicContainer.GetComponentsInChildren<SoundObjectWithInfo>()) {
				musicByTileType.Add (soundObject.tileType, soundObject);
			}

			playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
			tracksUnlockedByTileType = playerSaveComponent.LoadUnlockedTileTypeTracks();
		}
	}

	public void SetCurrentMusicTileType(TileType newTileType) {
		this.currentMusicTileType = newTileType;
	}

	public void Initialize(TileType newTileType, List<MusicInfo> songsInfoAvailable) {
		Initialize ();

		SetCurrentMusicTileType(newTileType);

		for(int i = 0 ; i < songsInfoAvailable.Count ; i++) {
			this.songTileTypesAvailable.Add (songsInfoAvailable[i].tileType);
		}
	}

	public void AddAvailableSong(TileType tileType) {
		this.songTileTypesAvailable.Add (tileType);

		SoundObjectWithInfo soundObjectInfoToSave;
		musicByTileType.TryGetValue(tileType, out soundObjectInfoToSave);

		SceneUtils.FindObject<CollectionManager>().AddMusicAsInfo(soundObjectInfoToSave);
	}

	public bool IsBusy() {
		return isBusy;
	}

	public Transform GetTargetAboveManager() {
		return this.targetAboveManager;
	}

    public bool IsPlayingMusic() {
		return currentMusic && currentMusic.IsPlaying();
    }
        
    public SoundObjectWithInfo GetMusicByTileType(TileType tileType) {
        SoundObjectWithInfo soundToReturn;
        musicByTileType.TryGetValue(tileType, out soundToReturn);
        
        return soundToReturn;

    }
}
