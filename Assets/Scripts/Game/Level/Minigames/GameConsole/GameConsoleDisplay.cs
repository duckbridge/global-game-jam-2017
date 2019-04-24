using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class GameConsoleDisplay : DispatchBehaviour {

    private TextMesh gameNameOutput;
    private TextMesh gameGenreOutput;
    private TextMesh yolOutput;
    private TextMesh noGamesTextOutput;

    private SpriteRenderer currentGameSprite;

    private List<PlayableConsoleGameInfo> allPlayableGames;

    private int currentIndex = 0;  
    private PlayerInputActions playerInputActions;    
    
    private PlayableConsoleGameInfo currentlySelectedGame;

	// Use this for initialization
	void Awake () {
        gameNameOutput = this.transform.Find("GameNameOutput").GetComponent<TextMesh>();
        gameGenreOutput = this.transform.Find("GameGenreOutput").GetComponent<TextMesh>();
        noGamesTextOutput = this.transform.Find("NoGamesTextOutput").GetComponent<TextMesh>();
        yolOutput = this.transform.Find("GameYOLOutput").GetComponent<TextMesh>();

        currentGameSprite = this.transform.Find("GameSprite").GetComponent<SpriteRenderer>();
	}
	
    void Start() {
        playerInputActions = PlayerInputHelper.LoadData();
    }

	// Update is called once per frame
	void Update () {
	   if(playerInputActions.left.WasPressed) {
            SelectNextGame();
        }
        
        if(playerInputActions.right.WasPressed) {
            SelectPreviousGame();
        }

        if(playerInputActions.interact.WasReleased) {
            if(currentlySelectedGame) {
				
				SceneUtils.FindObject<LevelBuilder> ().SaveData (SpawnType.ATGAMECONSOLE);
				Loader.LoadScene (Scene.GamerGameScene, LoadingScreenType.overworld_default);
                //Loader.LoadScene(currentlySelectedGame.sceneToLoad, LoadingScreenType.overworld_default); //temp
            }
        }

		if(playerInputActions.pause.WasReleased || playerInputActions.back.WasReleased) {
            DispatchMessage("HideGameConsole", null);
        }
	}

    public void SetAvailableGames(List<PlayableConsoleGameInfo> playableConsoleGamesInfo) {
        allPlayableGames = playableConsoleGamesInfo;
    }

    private void SelectNextGame() {
        ++currentIndex;
        if(currentIndex >= allPlayableGames.Count) {
            currentIndex = 0;
        }

        ShowGame();
    }

    private void SelectPreviousGame() {
        --currentIndex;
        if(currentIndex <= -1) {
            currentIndex = allPlayableGames.Count - 1;
        }

        ShowGame();
    }

    public void ShowGame() {
         if(allPlayableGames.Count > 0) {

            noGamesTextOutput.GetComponent<MeshRenderer>().enabled = false;
            
            currentlySelectedGame = allPlayableGames[currentIndex];

            gameNameOutput.text = currentlySelectedGame.name;
            gameGenreOutput.text = currentlySelectedGame.genre;
            yolOutput.text = currentlySelectedGame.YearOfRelease;

            currentGameSprite.sprite = currentlySelectedGame.GetComponent<SpriteRenderer>().sprite;       
            
        } else {
            noGamesTextOutput.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
