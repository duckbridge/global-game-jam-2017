using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSplitter : DispatchBehaviour {

	public Vector3 extraRotation;
	public float pushForce = 5f;
	public float destroyTimeout = 2f;
	public SpriteRenderer spriteToSplitOnHit;
	private List<GameObject> splitParts;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void DoSplit(Transform objectThatHits, Direction directionItHitsIn) {
		if(objectThatHits) {

			float onHitPosition = 0f;
			bool cutsHorizontally = false;

			if(directionItHitsIn == Direction.UP || directionItHitsIn == Direction.DOWN) {
				onHitPosition = objectThatHits.position.x;
			}

			if(directionItHitsIn == Direction.LEFT || directionItHitsIn == Direction.RIGHT) {
				onHitPosition = objectThatHits.transform.position.y;
				cutsHorizontally = true;
			}

			float intersectPercentage = GetIntersectPercentage(spriteToSplitOnHit.bounds, cutsHorizontally, objectThatHits.position);

			splitParts = SpriteCropper.SplitSpriteByInPieces(spriteToSplitOnHit, SpriteCropper.SplitType.EIGHT, intersectPercentage, cutsHorizontally, extraRotation);
			spriteToSplitOnHit.enabled = false;

			for(int i = 0; i < splitParts.Count ; i++) {

				splitParts[i].AddComponent<Rigidbody>();
				splitParts[i].GetComponent<Rigidbody>().useGravity = true;
				splitParts[i].GetComponent<Rigidbody>().isKinematic = false;
				splitParts[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

				switch(directionItHitsIn) {
					case Direction.LEFT:
						splitParts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-pushForce, 0), 0f, Random.Range (-pushForce, pushForce)), ForceMode.Impulse);
					break;
				
					case Direction.RIGHT:
						splitParts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, pushForce), 0f, Random.Range (-pushForce, pushForce)), ForceMode.Impulse);
					break;

					case Direction.UP:
						splitParts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-pushForce, pushForce), 0f, Random.Range (0, pushForce)), ForceMode.Impulse);
					break;

					case Direction.DOWN:
						splitParts[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-pushForce, pushForce), 0f, Random.Range (-pushForce, 0f)), ForceMode.Impulse);
					break;
				}

			}
			
			Invoke ("DestroyParts", destroyTimeout);

		}
	}

	private void DestroyParts() {
		for(int i = 0 ; i < splitParts.Count ; i++) {

			GameObject tempObject = splitParts[i];

			splitParts.RemoveAt(i);
			Destroy (tempObject);

			i--;
		}

		DispatchMessage("OnObjectSplittingDone", null);
	}
	
	private float GetIntersectPercentage(Bounds boundss, bool cutsHorizontally, Vector3 cutPosition) {
		float min = boundss.center.x - boundss.extents.x;
		float max = boundss.center.x + boundss.extents.x;
		float intersectionPoint = boundss.ClosestPoint(cutPosition).x - min;

		if(cutsHorizontally) {
			min = boundss.center.z - boundss.extents.z;
			max = boundss.center.z + boundss.extents.z;
			intersectionPoint = boundss.ClosestPoint(cutPosition).z - min;
		}
		
		float scaledMaximum = max - min;
		
		Logger.Log ("From a scale from 0 to " +scaledMaximum + ", the intersect is at " + intersectionPoint);

		float multiplier = 1/scaledMaximum;
		float intersectPercentage = intersectionPoint * multiplier;
		
		Logger.Log ("The X/Y intersect in at " + intersectPercentage*100 + " %");

		return intersectPercentage;
	}
}
