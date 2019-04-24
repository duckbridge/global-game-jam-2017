using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour {

	private List<AnimalInfo> allAnimalInfo = new List<AnimalInfo>();
	private List<MusicInfo> allMusicInfo = new List<MusicInfo>();
	private List<GameInfo> allGameInfo = new List<GameInfo>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddAnimalAsInfo(AnimalCompanion animalCompanion) {

        AnimalInfo animalInfo = new AnimalInfo();

        animalInfo.name = animalCompanion.GetOriginalName();
        animalInfo.description = animalCompanion.description;

        if(FindAnimalInfo(animalInfo) == null) {
		    allAnimalInfo.Add(animalInfo);
        }
	}

	public void AddMusicAsInfo(SoundObjectWithInfo soundObjectWithInfo) {

		MusicInfo musicInfo = new MusicInfo();

		musicInfo.name = soundObjectWithInfo.title;
		musicInfo.description = soundObjectWithInfo.description;
		musicInfo.tinyBlastPower = soundObjectWithInfo.tinySphereBlastPower;
		musicInfo.frontColor = soundObjectWithInfo.frontColor;
		musicInfo.onBeatRange = soundObjectWithInfo.onBeatRange;

        if(FindMusicInfo(musicInfo) == null) {
		    allMusicInfo.Add (musicInfo);
        }
	}

	public void AddGameAsInfo(GamePickup gamePickup) {

		GameInfo gameInfo = new GameInfo ();

		gameInfo.name = gamePickup.minigameName;
		gameInfo.description = gamePickup.minigameDescription;

		if(FindGameInfo(gameInfo) == null) {
			allGameInfo.Add (gameInfo);
		}
	}

	public void AddAnimalInfoRange(List<AnimalInfo> animalInfo) {
		this.allAnimalInfo.AddRange(animalInfo);
	}

	public void AddMusicInfoRange(List<MusicInfo> musicInfo) {
		this.allMusicInfo.AddRange(musicInfo);
	}

	public void AddGameInfoRange(List<GameInfo> gameInfo) {
		this.allGameInfo.AddRange (gameInfo);
	}


	public List<MusicInfo> GetAllMusicInfo() {
		return allMusicInfo;
	}

	public List<AnimalInfo> GetAllAnimalInfo() {
		return allAnimalInfo;
	}

	public List<GameInfo> GetAllGameInfo() {
		return allGameInfo;
	}

    private AnimalInfo FindAnimalInfo(AnimalInfo animalInfo) {
        AnimalInfo foundInfo = null;

        foreach(AnimalInfo aInfo in allAnimalInfo) {
            if(aInfo.name == animalInfo.name) {
                foundInfo = aInfo;
            }
        }
        
        return foundInfo;
    }

    private MusicInfo FindMusicInfo(MusicInfo musicInfo) {
        MusicInfo foundInfo = null;

        foreach(MusicInfo mInfo in allMusicInfo) {
            if(mInfo.name == musicInfo.name) {
                foundInfo = mInfo;
            }
        }
        
        return foundInfo;
    }

	private GameInfo FindGameInfo(GameInfo gameInfo) {
		GameInfo foundInfo = null;

		foreach(GameInfo gInfo in allGameInfo) {
			if(gInfo.name == gameInfo.name) {
				foundInfo = gInfo;
			}
		}

		return foundInfo;
	}
}
