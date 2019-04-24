using UnityEngine;
using System.Collections;

public class TransitionRoom : Room {

	public override void Initialize (RoomNode roomNode) {
		base.Initialize (roomNode);
		roomNode.SetRoomNodeType(RoomNodeType.Transition);

		BorderWall borderWall = GetComponentInChildren<BorderWall>();
		if(borderWall) {
			borderWall.AddEventListener(this.gameObject);
		}

		Invoke ("DisableBorderWallIfAlreadyDead", 2f);
	}

	private void DisableBorderWallIfAlreadyDead() {
		BorderWall borderWall = GetComponentInChildren<BorderWall>();
		if(borderWall) {
			if(SceneUtils.FindObject<PlayerSaveComponent>().GetDeadBorderWalls().Contains(this.GetTileType())) {

				if(borderWall.GetComponent<EnemyBreakComponent>()) {
					borderWall.GetComponent<EnemyBreakComponent>().BreakAll();
				}

				borderWall.gameObject.SetActive(false);
				CutSceneManager csManager = GetComponentInChildren<CutSceneManager>();
				if(csManager) {
					csManager.gameObject.SetActive(false);
				}
			}
		}
	}

	public void OnBorderWallDied() {
		SceneUtils.FindObject<PlayerSaveComponent>().AddDeadBorderWall(this.GetTileType());
	}

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		base.OnEntered (enemyActivationDelay, ref playerEntered);

		FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
		if(followingEye) {
			followingEye.Initialize(playerEntered.transform);
		}

		DisableBorderWallIfAlreadyDead();
	}

	public override void OnExitted () {
		base.OnExitted ();

		FollowingEye followingEye = GetComponentInChildren<FollowingEye>();
		if(followingEye) {
			followingEye.UnInitialize();
		}
	}
}
