using UnityEngine;
using System.Collections;

public class OnHitParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyDelayed(float timeout) {
        Invoke("DoDestroy", timeout);
    }

    private void DoDestroy() {
        Destroy(this.gameObject);
    }
}
