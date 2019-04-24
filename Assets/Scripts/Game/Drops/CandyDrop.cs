using UnityEngine;
using System.Collections;

public class CandyDrop : LootDrop {
	
	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		Enemy enemy = coll.gameObject.GetComponent<Enemy>();

		if(player) {
            OnPickedupByPlayer(player);
		}

		if(enemy) {
			PhysicsUtils.IgnoreCollisionBetween(this.GetComponent<Collider>(), enemy.GetColliders());
		}
	}

    protected virtual void OnPickedupByPlayer(Player player) {
        CancelInvoke("DoDestroy");
        
        if(this.transform.Find("Shadow")) {
            this.transform.Find("Shadow").gameObject.SetActive(false);
        }

        player.OnCandyPickedup(this);
    }
}
