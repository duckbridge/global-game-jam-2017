using UnityEngine;
using System.Collections;

public class CassettePickup : DispatchBehaviour {

	public bool enableInputsAfterPickup = true;
	public TileType tileType;
	
	public void Start() {}
	public void Update() {}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			OnPickedUpByPlayer(player);
		}
	}

	public void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			OnPickedUpByPlayer(player);
		}
	}

	void FixedUpdate() {
		if (this.transform.rotation.eulerAngles != Vector3.zero) {
			this.transform.rotation = Quaternion.identity;
		}
	}

	public virtual void OnPickedUpByPlayer(Player player) {
		SceneUtils.FindObject<PlayerSaveComponent>().AddUnlockedTileTypeTrack(tileType);
		
		DispatchMessage("OnCassettePickedUp", null);
		
		player.GetComponent<PlayerPickupComponent>().OnCassettePickupPickedUp(this, enableInputsAfterPickup);
	}
}
