using UnityEngine;
using System.Collections;

public class CandyDropContainer : LootDrop {

	public CandyDrop candyDropPrefab;

	public int minimumAmountOfCandy = 2;
	public int maximumAmountOfCandy = 5;

	public Vector2 minimumPushForce = new Vector2(-5f, -5f);
	public Vector2 maximumPushForce = new Vector2(5f, 5f);

	public override void DoDrop () {
		int randomCandyDropAmount = Random.Range (minimumAmountOfCandy, maximumAmountOfCandy);
		for(int i = 0 ; i < randomCandyDropAmount ; i++) {
			CandyDrop candyDrop = (CandyDrop) GameObject.Instantiate(candyDropPrefab, this.transform.position, Quaternion.identity);
			candyDrop.GetComponent<Rigidbody>()
				.AddForce(new Vector3(Random.Range (minimumPushForce.x, maximumPushForce.x), 0f, Random.Range (minimumPushForce.y, maximumPushForce.y)));

			candyDrop.DoDrop();
		}

		Destroy (this.gameObject);
	}
}
