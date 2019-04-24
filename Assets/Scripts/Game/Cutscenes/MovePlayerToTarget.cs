using UnityEngine;
using System.Collections;

public class MovePlayerToTarget : CutSceneComponent {
		
	public float closeToTargetDistance = .5f;
	public Transform moveTarget;

	private Player playerToMove;
	private Vector3 movePosition;

	public override void OnActivated () {
		playerToMove = SceneUtils.FindObject<Player>();

		movePosition = new Vector3(moveTarget.position.x, playerToMove.transform.position.y, moveTarget.position.z);

	}

	public void FixedUpdate() {
		if(isActivated) {
			
			if(Vector2.Distance(new Vector2(moveTarget.position.x, moveTarget.position.z), 
			                    new Vector2(playerToMove.transform.position.x, playerToMove.transform.position.z)) > closeToTargetDistance) {

				Vector3 moveDirection = 
					MathUtils.CalculateDirection(movePosition, playerToMove.transform.position);

				playerToMove.GetComponent<BodyControl>().DoMove(moveDirection.x, moveDirection.z, false);
				
				
			} else {
                playerToMove.GetComponent<Rigidbody>().velocity = Vector3.zero;
                playerToMove.transform.position = new Vector3(moveTarget.position.x, playerToMove.transform.position.y, moveTarget.position.z);
				DeActivate();	
			}
		}
	}
}
