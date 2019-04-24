using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundUtils {

	private static float backgroundSoundVolume = 0f;
	private static float fxSoundVolume = 0f;
    private static SettingsSaveComponent settingsSaveComponent;
    
    public static void SetBGVolume(float newBgVolume) {
        backgroundSoundVolume = newBgVolume;
    }

    public static void SetFXVolume(float newFxVolume) {
        fxSoundVolume = newFxVolume;
    }

	public static void SetSoundVolumeToSavedValue(SoundType soundType) {

		if(soundType == SoundType.FX) {
			SetSoundVolume(SoundType.FX, fxSoundVolume);
		} else {
			SetSoundVolume(SoundType.BG, backgroundSoundVolume);
		}
	}

	public static void SetSoundVolumeToSavedValue() {

		float bgVolume = backgroundSoundVolume;
		float fxVolume = fxSoundVolume;
		SetSoundVolume(fxVolume, bgVolume);
	}

	public static void SetSoundVolumeToSavedValueForGameObject(SoundType soundType, GameObject sourceGameObject) {

		List<SoundObject> soundObjects = new List<SoundObject>(sourceGameObject.GetComponentsInChildren<SoundObject>());
		soundObjects.AddRange(sourceGameObject.GetComponents<SoundObject>());

		for(int i = 0 ; i < soundObjects.Count; i++) {
			if(soundType == SoundType.FX && soundObjects[i].soundType == SoundType.FX) {
				soundObjects[i].SetVolume(fxSoundVolume);
			} else if(soundType == SoundType.BG && soundObjects[i].soundType == SoundType.BG) {
				soundObjects[i].SetVolume(backgroundSoundVolume);
			}
		}
	}


	public static void SaveSoundVolume(SoundType soundType, float volume) {

        if(!settingsSaveComponent) {
           settingsSaveComponent = SceneUtils.FindObject<SettingsSaveComponent>();    
        }

		if(soundType == SoundType.FX) {
            settingsSaveComponent.SetFxVolume(volume);
		} else {
			settingsSaveComponent.SetBgVolume(volume);
		}
		
        settingsSaveComponent.SaveSettingsData();
	}

	public static void SetSoundVolume(SoundType soundType, float newVolume, bool saveNewVolume = true) {

		if(newVolume > 1 || newVolume < 0) {
			Logger.Log ("you cannot have the volume higher than 1 or lower than 0", LogType.Error);
			return;
		}

		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.SetVolume(newVolume);
			}
		}

		if (saveNewVolume) {
			if (soundType == SoundType.FX) {
				fxSoundVolume = newVolume;
			}

			if (soundType == SoundType.BG) {
				backgroundSoundVolume = newVolume;
			}
		}
	}

	public static void SetSoundVolume(float newFXVolume, float newBGVolume) {
		
		if(newFXVolume > 1 || newFXVolume < 0 || newBGVolume > 1 || newBGVolume < 0) {
			Logger.Log ("you cannot have the volume higher than 1 or lower than 0", LogType.Error);
			return;
		}
		
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == SoundType.FX) {
				sound.SetVolume(newFXVolume);
			} else {
				sound.SetVolume(newBGVolume);
			}
		}

		fxSoundVolume = newFXVolume;
		backgroundSoundVolume = newBGVolume;
	}

	public static void SetTimeScale(float newTimeScale) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.SetTimeScale(newTimeScale);
		}
	}

	public static void ResetTimeScale() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.ResetTimeScale();
		}
	}

	public static void FadeOutBackgroundMusic(float amount = 0) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(FadingAudio fadingAudio in sounds) {
			if(fadingAudio.soundType == SoundType.BG) {
				if(amount > 0) {
					fadingAudio.FadeOut(amount);
				}
			}
		}
	}
	public static void MuteAll() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.Mute();
		}
	}

	public static void UnMuteAll() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.UnMute();
		}
	}

	public static void MuteAll(SoundType soundType) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.Mute();
			}
		}
	}

	public static void UnMuteAll(SoundType soundType) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.UnMute();
			}
		}
	}

	public static void SwapBackgroundMusic(List<SoundObject> oldMusic, List<SoundObject> newMusic, bool playForced = true) {

		foreach(SoundObject oldMusicSound in oldMusic) {
			oldMusicSound.Stop ();
			oldMusicSound.gameObject.SetActive(false);
		}
		
		newMusic.ForEach(music => music.gameObject.SetActive(true));

		SetSoundVolumeToSavedValue(SoundType.BG);
		
		foreach(SoundObject newSong in newMusic) {
			newSong.Play(playForced);
		}
	}

	public static float GetVolume(SoundType soundType){ 

		if(soundType == SoundType.FX) {
			return fxSoundVolume;
		} else {
			return backgroundSoundVolume;
		}

	}

}
