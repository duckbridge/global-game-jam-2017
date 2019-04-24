using UnityEngine;
using System.Collections;

public class VillageRoom : Room {

	public GameObject[] doorsWithAnimation;

    private Player player;
    private LevelBuilder levelBuilder;

    public override void Awake() {
        base.Awake();
    }

	public virtual void Start() {
		DisableSpawning ();
	}

	public override void SpawnCorrectGroundTile() {}

    public void ActivateAndListenToAllBorders(Player playerEntered) {
        foreach(VillageBorder villageBorder in GetComponentsInChildren<VillageBorder>()) {
            villageBorder.AddEventListener(this.gameObject);
            villageBorder.ActivateBorder(playerEntered);
        }    
    }

    public void DeActivateAndStopListeningToAllBorders() {
        foreach(VillageBorder villageBorder in GetComponentsInChildren<VillageBorder>()) {
            villageBorder.RemoveEventListener(this.gameObject);
            villageBorder.DeActivateBorder();
        }
    }
    
	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
        player = playerEntered;

       ActivateAndListenToAllBorders(playerEntered);

		for(int i = 0 ; i < doorsWithAnimation.Length ;i++) {

			if(doorsWithAnimation[i] != null) {

				if(doorsWithAnimation[i].GetComponent<DungeonDoor>()) {
                    doorsWithAnimation[i].GetComponent<DungeonDoor>().UnlockDoor();
				}
			}
		}

		base.OnEntered (enemyActivationDelay, ref playerEntered);
	}

    public void OnVillageBorderPassed(Vector2 newGridLocation) {

        if(!levelBuilder) {
            levelBuilder = SceneUtils.FindObject<LevelBuilder>();
        }

        RoomNode roomToTranfserTo = RoomNodeHelper.GetRoomNodeAt(player.GetCurrentTileBlock(), player.GetGridLocation() + newGridLocation);
        RoomNode oldRoomNode = this.GetRoomNode();

        RoomTransferData roomTransferData = new RoomTransferData();

        roomTransferData.roomNode = roomToTranfserTo;

        player.OnExittedRoom(oldRoomNode);
        OnExitted();

        levelBuilder.OnRoomEntered(roomTransferData, 0f);
        
    }

    public override void OnExitted() {

        DeActivateAndStopListeningToAllBorders();

        base.OnExitted();
    }
}
