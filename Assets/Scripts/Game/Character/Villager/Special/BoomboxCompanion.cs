using UnityEngine;
using System.Collections;

public class BoomboxCompanion : Villager {

	public Room currentRoom;
    public TextBoxManagerWithOptionMenu teleporterTextBoxWithOptionMenu;
    public TPButtonsLoader tpButtonsLoader;

    private bool teleportAfterTextBox = false;

	public void SetTextBoxBasedOnContext() {
		
        Logger.Log (currentRoom.name + "," + currentRoom);

		string pathToTextBox = "Textboxes/";

		if(currentRoom.GetComponent<CassetteRoom>()) {
			pathToTextBox += ("CassetteRoom/" +  currentRoom.GetTileType().ToString());
		} else if(currentRoom.GetComponent<GameRoom>()) {
			pathToTextBox += ("GameRoom/" +  currentRoom.GetTileType().ToString());
        } else if(currentRoom.GetComponent<DungeonBossRoom>()) {
            pathToTextBox += ("Boss/all");
            teleportAfterTextBox = true;
        } else {
			pathToTextBox += ("Village/" + currentRoom.GetTileType().ToString() + "/" + currentRoom.GetRoomNode().version);
		}

		Logger.Log ("full path: " + pathToTextBox);

		textManager = this.transform.Find(pathToTextBox).GetComponent<TextBoxManager>();
        textManager.AddEventListener(this.gameObject);

	}

	public override void OnTextBoxDoneAndHidden() {
		if (teleportAfterTextBox) {

			GetComponent<PlayerSpitter> ().AddEventListener (this.gameObject);
			GetComponent<PlayerSpitter> ().SwallowPlayer (player, true);
			player.GetComponent<PlayerInputComponent> ().enabled = false;
		} else {
			base.OnTextBoxDoneAndHidden ();
		}
    }

    public void OnItemChosen(int itemId) {

        Logger.Log(itemId);

        switch(itemId) {
            case 1:
                teleporterTextBoxWithOptionMenu.gameObject.SetActive(true);
                teleporterTextBoxWithOptionMenu.Initialize();
                teleporterTextBoxWithOptionMenu.AddEventListener(this.gameObject);
                teleporterTextBoxWithOptionMenu.ResetShowAndActivate();
                tpButtonsLoader.Initialize();

            break;
            
            case 2:
                //do nothing for now
                ReEnablePlayer();
            break;

            case -1:
                ReEnablePlayer();
            break;

            case 106: //exit on teleport selection thing
                ReEnablePlayer();
            break;
        }

        if(itemId >= 100 && itemId <= 105) {
            TeleportPlayer(itemId - 100);
        }
    }

    private void TeleportPlayer(int tileBlockId) {
        
        if(player.GetCurrentTileBlock().id != tileBlockId) {

			GetComponent<PlayerSpitter> ().AddEventListener (this.gameObject);
            GetComponent<PlayerSpitter>().SwallowPlayer(player, true);

            player.GetComponent<PlayerInputComponent>().enabled = false;

            Time.timeScale = 1f;

            MapBuilder mapBuilder = SceneUtils.FindObject<MapBuilder>();

            TileBlock tpTileBlock = mapBuilder.GetTileBlocks().Find(tileBlock => tileBlock.id == tileBlockId);
            player.SetCurrentTileBlock(tpTileBlock);
			mapBuilder.SaveData(SpawnType.TELEPORTED);

        } else {
            ReEnablePlayer();
        }
    }

    public override void OnInteract(Player player) {

        if(textManager.GetComponent<TextBoxManagerWithOptionMenu>()) {
          textManager.GetComponent<TextBoxManagerWithOptionMenu>().Initialize();
        }

        base.OnInteract(player);
    }

    private void ReEnablePlayer() {
        player.OnTalkingDone();
        player.GetComponent<PlayerInputComponent>().enabled = true;
    }

    private void ReloadLevel() {
        Loader.ReloadLevel();
    }

	public void OnSwallowedObject() {
		SceneUtils.FindObject<OnDieEffect> ().StartEffect ();
		if (teleportAfterTextBox) {
			Invoke("LoadMainScene", 1f);
		} else {
			Invoke ("ReloadLevel", 1f);
		}
	}

    private void LoadMainScene() {
        Loader.LoadScene (Scene.MainScene, LoadingScreenType.menu_default);
    }
}
