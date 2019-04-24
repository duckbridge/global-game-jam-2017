using UnityEngine;
using System.Collections;

public class FakeRopePart : MonoBehaviour {

	public GameObject backAnchor;
	public GameObject frontAnchor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(backAnchor && frontAnchor) {
			this.transform.position = (backAnchor.transform.position + frontAnchor.transform.position) * .5f;

			Vector3 directionToTarget = MathUtils.CalculateDirection(frontAnchor.transform.position, this.transform.position);
			this.transform.right = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

			float distanceBetweenThisAndTarget = Vector3.Distance(frontAnchor.transform.position, this.transform.position);
			this.transform.localScale = new Vector3(distanceBetweenThisAndTarget * 1.7f, this.transform.localScale.y, this.transform.localScale.z);
		}
	}
}
