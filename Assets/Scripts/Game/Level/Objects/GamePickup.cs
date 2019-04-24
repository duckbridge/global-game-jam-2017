using UnityEngine;
using System.Collections;

public class GamePickup : DispatchBehaviour {

	public string minigameName;
	public string minigameDescription;

	public void Start() {}
	public void Update() {}

	public void EnablePickingUp() {
		GetComponent<Collider> ().enabled = true;
		this.transform.Find ("FakeGamePickupCollider").gameObject.SetActive (false);
	}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			OnPickedUpByPlayer(player);
		}
	}

	public void OnPickedUpByPlayer(Player player) {
		
		SceneUtils.FindObject<CollectionManager> ().AddGameAsInfo (this);

		player.GetComponent<PlayerPickupComponent>().OnPlayableGamePickedUp(this);
	}
}
