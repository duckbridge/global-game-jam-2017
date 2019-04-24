using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConsole : Villager {

    public TextBoxManager hasGamesTextManager, hasNoGamesTextManager;
    public GameConsoleDisplay gameConsoleDisplay;    

    private Animation2D showAnimation;
    private List<PlayableConsoleGameInfo> gamesToPlay;  

    private enum TalkState { normal, hasnogames, hasgames }
    private TalkState talkState;

    public override void Start() {
        base.Start();
        //showAnimation = this.transform.Find("DisplayContainer/Animations/ShowAnimation").GetComponent<Animation2D>();
    }

    public override void OnInteract(Player player) {
        talkState = TalkState.normal;

        base.OnInteract(player);

        gamesToPlay = new List<PlayableConsoleGameInfo>();
        
		CollectionManager collectionManager = SceneUtils.FindObject<CollectionManager>();
		foreach (GameInfo gameInfo in collectionManager.GetAllGameInfo()) {
			gamesToPlay.Add (this.transform.Find ("Games/" + gameInfo.name).GetComponent<PlayableConsoleGameInfo> ());
		}
		gamesToPlay.Add (this.transform.Find ("Games/GAME_1").GetComponent<PlayableConsoleGameInfo> ());

    }

	public override void OnTextBoxDoneAndHidden() {

        switch(talkState) {
            case TalkState.normal:
               if(gamesToPlay.Count > 0) {
                    talkState = TalkState.hasgames;
                    hasGamesTextManager.AddEventListener(this.gameObject);
                    hasGamesTextManager.ResetShowAndActivate();
                } else {
                    talkState = TalkState.hasnogames;
                    hasNoGamesTextManager.AddEventListener(this.gameObject);
                    hasNoGamesTextManager.ResetShowAndActivate();
                }
            break;    
            
            case TalkState.hasgames:
                gameConsoleDisplay.SetAvailableGames(gamesToPlay);
                ShowGameConsole();
            break;
    
			case TalkState.hasnogames:
				base.OnTextBoxDoneAndHidden ();
            break;
        }
    }

    private void ShowGameConsole() {
        this.transform.Find("DisplayContainer").gameObject.SetActive(true);

        gameConsoleDisplay.ShowGame();
        gameConsoleDisplay.AddEventListener(this.gameObject);
    }
   
    public void HideGameConsole() {
         this.transform.Find("DisplayContainer").gameObject.SetActive(false);

         DisableInteraction(player);
         player.GetComponent<PlayerInputComponent>().enabled = true;
		 player.OnTalkingDone ();
    }

    public void OnAnimationDone(Animation2D animation2D) {
        if(animation2D.name == "ShowAnimation") {
           gameConsoleDisplay.ShowGame();
        }
    }
}
