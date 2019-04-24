using UnityEngine;
using System.Collections;

public class FishCollider : MonoBehaviour {

	public string fishToSpawn = "DefaultFish";
	public Direction requiredDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnFish() {
		SwimmingFish swimmingFish = (SwimmingFish) GameObject.Instantiate(Resources.Load("Fishing/Fishes/" + fishToSpawn, typeof(SwimmingFish)), this.transform.position, Quaternion.identity);
		swimmingFish.SetMoveBounds(GetComponent<Collider>().bounds);
		swimmingFish.transform.parent = this.transform;
	}
}
