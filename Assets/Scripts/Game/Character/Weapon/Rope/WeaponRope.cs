using UnityEngine;
using System.Collections;

public class WeaponRope : MonoBehaviour {

	public HingeJoint firstRopePart;
	public HingeJoint lastRopePart;

	public GameObject ropeParts;

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSpawned(HingeJoint head, Rigidbody tail) {
		ropeParts.gameObject.SetActive(true);

		firstRopePart.GetComponentInChildren<RopePart>().lookAtTarget = head.transform;

		head.connectedBody = firstRopePart.GetComponent<Rigidbody>();
		lastRopePart.connectedBody = tail;

		Invoke ("ShowRopePartSpritesDelayed", .05f);

	}

	private void ShowRopePartSpritesDelayed() {
		SpriteRenderer[] spriteRenderers = ropeParts.GetComponentsInChildren<SpriteRenderer>();
		for(int i = 0 ; i < spriteRenderers.Length ; i++) {
			spriteRenderers[i].enabled = true;
		}
	}
}
