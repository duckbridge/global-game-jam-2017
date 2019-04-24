using UnityEngine;
using System.Collections;

public class BossShockerPatrolAction : PatrolAction {

	protected override Vector4 GetPatrolArea () {
		return new Vector4(gameCamera.ViewportToWorldPoint(new Vector3(.15f, 0f, 0f)).x,
                           gameCamera.ViewportToWorldPoint(new Vector3(.85f, 0f, 0f)).x,
                 	       gameCamera.ViewportToWorldPoint(new Vector3(0f, .1f, 0f)).z,
		                   gameCamera.ViewportToWorldPoint(new Vector3(0f, .6f, 0f)).z);
	}
}
