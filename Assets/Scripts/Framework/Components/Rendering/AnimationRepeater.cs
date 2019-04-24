using UnityEngine;
using System.Collections;

public class AnimationRepeater : MonoBehaviour {

	public float animationRepeatTimeout = 5f;
	public Animation2D animation2D;
	// Use this for initialization
	void Awake () {
		animation2D.AddEventListener(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnAnimationDone(Animation2D animation2D) {
		Invoke ("PlayAnimation", animationRepeatTimeout);
	}

	private void PlayAnimation() {
		animation2D.Play(true);
	}
}
