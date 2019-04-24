using UnityEngine;
using System.Collections;

public class BossRope : MonoBehaviour {

    private Transform endOfRope;
    private Transform fakeTarget;

    public void Awake() {   
        endOfRope = this.transform.Find("Rope/EndOfRope");
        fakeTarget = this.transform.Find("FakeTarget");
    }

	// Update is called once per frame
	void Update () {
	
	}

    public Transform GetEndOfRope() {
        return this.endOfRope;    
    }

    public Transform GetFakeTarget() {
        return this.fakeTarget;
    }
}
