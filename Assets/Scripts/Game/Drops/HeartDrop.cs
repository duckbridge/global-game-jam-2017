using UnityEngine;
using System.Collections;

public class HeartDrop : LootDrop {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		Enemy enemy = coll.gameObject.GetComponent<Enemy>();
		if(player) {
			CancelInvoke("DoDestroy");
			player.OnHealthPickedUp(this);

			Destroy (this.gameObject);
		}

		if(enemy) {
			PhysicsUtils.IgnoreCollisionBetween(this.GetComponent<Collider>(), enemy.GetColliders());
		}
	}
}
