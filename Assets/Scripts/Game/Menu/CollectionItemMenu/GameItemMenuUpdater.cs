using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameItemMenuUpdater : CollectionItemMenuUpdater {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateVisibleButtons(CollectionManager collectionManager, List<ButtonRow> buttonRows) {
		List<GameInfo> allGameInfo = collectionManager.GetAllGameInfo ();

		foreach(GameInfo gInfo in allGameInfo) {
			Logger.Log (gInfo.name);		
		}

		for(int i = 0; i < buttonRows.Count ; i++) {
           ButtonRow buttonRow = buttonRows[i];

            for(int j = 0; j < buttonRow.buttons.Count; j++) {

				GameItemButton gameItemButton = (GameItemButton) buttonRow.buttons[j];

				GameInfo foundGameInfo = allGameInfo.Find(gameInfo => gameInfo.name.Equals(gameItemButton.name));
                bool showButton = (foundGameInfo != null);

                gameItemButton.SetVisible(showButton);
            }
        }
    }
}
