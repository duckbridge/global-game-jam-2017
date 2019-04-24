using UnityEngine;
using System.Collections;

public class ColliderThatDamagesPlayer : MonoBehaviour {

	public virtual void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			player.OnHit(this.transform.position);
		}
	}
}
