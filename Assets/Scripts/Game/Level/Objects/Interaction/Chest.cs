using UnityEngine;
using System.Collections;

public class Chest : InteractionObject {

	public float[] chancesToDrop;
	public LootDrop[] lootToDrop;
	
	private Animation2D chestOpenAnimation;
	private Transform lootDropTransform;

	public void Awake() {
		chestOpenAnimation = this.transform.Find("ChestOpenAnimation").GetComponent<Animation2D>();
		lootDropTransform = this.transform.Find("LootDropPosition");
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			base.OnInteract (player);

			GetComponent<Collider>().enabled = false;
			chestOpenAnimation.AddEventListener(this.gameObject);
			chestOpenAnimation.Play (true);
		}
	}

	private void OnAnimationDone(Animation2D animation2D) {

		for(int i = 0 ; i < lootToDrop.Length ; i++) {
			
			int randomDropChance = Random.Range (0, 100);
			
			if(chancesToDrop[i] >= randomDropChance) {
				LootDrop droppedLoot = (LootDrop) GameObject.Instantiate(lootToDrop[i], lootDropTransform.position, Quaternion.identity);
				droppedLoot.DoDrop();
			}
		}

		Destroy(this.gameObject);
	}

	public override void OnTriggerEnter(Collider coll) {

		base.OnTriggerEnter(coll);

		SwordWeapon sword = coll.gameObject.GetComponent<SwordWeapon>();
		if(sword) {
			OnInteract(null);
		}
	}
}
