using UnityEngine;
using System.Collections;

public class LootDrop : MonoBehaviour {

	public float destroyTimeout = 5f;
	private Blink2D blink2D;

	// Use this for initialization
	public virtual void Awake() {
		blink2D = GetComponentInChildren<Blink2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void DoDrop() {

		Invoke ("StartBlinking", destroyTimeout * .5f);
		Invoke ("BlinkFaster", destroyTimeout * .7f);
		Invoke ("BlinkFastest", destroyTimeout * .9f);

		Invoke ("DoDestroy", destroyTimeout);
	}

	private void StartBlinking() {
		blink2D.BlinkWithTimeout(99, 0.2f);
	}

	private void BlinkFaster() {
		blink2D.BlinkWithTimeout(99, 0.1f);
	}

	private void BlinkFastest() {
		blink2D.BlinkWithTimeout(99, 0.05f);
	}

	private void DoDestroy() {
		Destroy (this.gameObject);
	}
}
