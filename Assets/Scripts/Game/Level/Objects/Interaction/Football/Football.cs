using UnityEngine;
using System.Collections;

public class Football : InteractionObject {

	public float shootForce = 50f;
	private Transform originalParent;

	public override void Start() {
		originalParent = this.transform.parent;
	}

	public override void OnInteract (Player player) {

		if(canInteract) {
			Vector3 directionFromPlayer = this.transform.position - player.transform.position;
			directionFromPlayer.Normalize();

			GetComponent<Rigidbody>().AddForce(directionFromPlayer * shootForce, ForceMode.Impulse);

			base.OnInteract (player);
		}

	}
}
