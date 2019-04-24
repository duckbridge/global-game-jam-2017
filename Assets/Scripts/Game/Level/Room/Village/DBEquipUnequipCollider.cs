using UnityEngine;
using System.Collections;

public class DBEquipUnequipCollider : MonoBehaviour {

	public Room room;
	public PathFindNode[] pathFindNodes;
	public int canMoveToChargerThreshold = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {

			BoomboxCompanion boomboxCompanion = player.GetBoomboxCompanion ();
			if(boomboxCompanion) {

				BoomboxAction boomboxAction = boomboxCompanion.GetComponent<BoomboxActionManager> ().GetBoomboxActionTypeBy (BoomboxActionType.NAVIGATE_TO_PLAYER);
				boomboxAction.GetComponent<BoomboxNavigateToPlayer>().SetPathNodes (pathFindNodes);
				boomboxAction.GetComponent<BoomboxNavigateToPlayer>().SetSourceRoom (room);

				if(boomboxCompanion.GetComponent<BoomboxActionManager>().currentBoomboxAction) {
					if(boomboxCompanion.GetComponent<BoomboxActionManager>().currentBoomboxAction.boomboxActionType != BoomboxActionType.NAVIGATE_TO_PLAYER) {
						boomboxCompanion.GetComponent<BoomboxActionManager>().SwitchState(BoomboxActionType.NAVIGATE_TO_PLAYER);
					}
				} else {
					
					boomboxCompanion.GetComponent<BoomboxActionManager>().SwitchState(BoomboxActionType.NAVIGATE_TO_PLAYER);
				}
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player != null && player.IsInTown()) {

			BoomboxActionType boomboxActionType = player.IsInTown() ? BoomboxActionType.RUN_TO_TARGET_VILLAGE : BoomboxActionType.RUNNING_RANDOMLY;
			BoomboxCompanion boomboxCompanion = player.GetBoomboxCompanion ();

			if(boomboxCompanion) {
				if(boomboxCompanion.GetComponent<BoomboxActionManager>().currentBoomboxAction) {
					if(boomboxCompanion.GetComponent<BoomboxActionManager>().currentBoomboxAction.boomboxActionType == BoomboxActionType.NAVIGATE_TO_PLAYER) {

						BoomboxAction boomboxAction = boomboxCompanion.GetComponent<BoomboxActionManager> ().GetBoomboxActionTypeBy (BoomboxActionType.NAVIGATE_TO_PLAYER);
						Logger.Log ("db is moving to player, setting threshold to " + canMoveToChargerThreshold + "!");
						if (!boomboxAction.GetComponent<BoomboxNavigateToPlayer> ().CanSwitchToOtherAction(room)) {
							Logger.Log ("Trying to set the threshold, but the source room is different than the current room, so do nothing");
						} else {
							boomboxAction.GetComponent<BoomboxNavigateToPlayer> ().SetThresHoldAndActionToSwitchTo (canMoveToChargerThreshold, boomboxActionType, room.transform.Find ("DBMovePosition").gameObject);
						}
						return;	
					}
				}

				boomboxCompanion.GetComponent<BoomboxActionManager>().SwitchStateAndSetRunTarget(boomboxActionType, room.transform.Find("DBMovePosition").gameObject);
			
			} else {
				player.UnEquipDBAndMoveDB(room);
			}
		}
	}
}
