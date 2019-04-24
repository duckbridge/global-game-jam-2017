using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeRope : MonoBehaviour {

	public int ropeParts = 30;
	public float ropePartSize = 1f;
	public FakeRopePart fakeRopePartPrefab;
	public GameObject ropeFrontEnd, ropeBackEnd;

	private List<FakeRopePart> fakeRopeParts = new List<FakeRopePart>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnSpawned(Transform ropeOrigin) {
        for(int i = 0 ; i < ropeParts ; i++) {
            fakeRopeParts.Add((FakeRopePart) GameObject.Instantiate(fakeRopePartPrefab, this.transform.position, Quaternion.identity));
        }

        RewireRope();
	}

	private void RewireRope() {
		for(int i = 0 ; i < fakeRopeParts.Count; i++) {
			
			if(i == 0) {
				
				fakeRopeParts[i].frontAnchor = ropeFrontEnd;
				fakeRopeParts[i].backAnchor = fakeRopeParts[i + 1].gameObject;
				
			} else if(i == fakeRopeParts.Count -1) {
				
				fakeRopeParts[i].frontAnchor = fakeRopeParts[i - 1].gameObject;
				fakeRopeParts[i].backAnchor = ropeBackEnd;
				
			} else {
				
				fakeRopeParts[i].frontAnchor = fakeRopeParts[i - 1].gameObject;
				fakeRopeParts[i].backAnchor = fakeRopeParts[i + 1].gameObject;
				
			}
		}
	}
}
