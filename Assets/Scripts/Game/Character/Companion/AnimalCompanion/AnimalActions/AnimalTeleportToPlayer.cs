using UnityEngine;
using System.Collections;

public class AnimalTeleportToPlayer : AnimalAction {

	private Player player;
	private RunPosition randomRunPosition;

	protected override void OnUpdate () {}
	
	protected override void OnStarted () {

		player = SceneUtils.FindObject<Player>();
		randomRunPosition = player.GetRandomRunPositions();

		animalBodycontrol.transform.position = 
			new Vector3(randomRunPosition.transform.position.x, animalBodycontrol.transform.position.y, randomRunPosition.transform.position.z);

		FinishAction(AnimalActionType.FOLLOWING_PLAYER);
	}
}