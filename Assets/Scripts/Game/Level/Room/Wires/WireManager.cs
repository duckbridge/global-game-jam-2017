using UnityEngine;
using System.Collections;

public class WireManager : MonoBehaviour {

	public GameObject[] shieldOrbAnimations;
	public Animation2D wireAnimation, forceFieldAnimation;

	public void Start() {}
	public void Update() {}

	public void PowerAllWires() {
		foreach(GameObject shieldOrbAnimation in shieldOrbAnimations) {
			shieldOrbAnimation.GetComponent<Animation2D>().Show ();
			shieldOrbAnimation.GetComponent<Animation2D>().Play(true);

			shieldOrbAnimation.GetComponent<Animation>().Play();
			shieldOrbAnimation.GetComponent<ParticleSystem>().Play();
		}

		wireAnimation.Play(true);
		wireAnimation.outputRenderer.color = Color.white;

		if(forceFieldAnimation) {
			forceFieldAnimation.Show ();
			forceFieldAnimation.Play(true);
		}
	}

	public void UnPowerAllWires() {
		foreach(GameObject shieldOrbAnimation in shieldOrbAnimations) {
			shieldOrbAnimation.GetComponent<Animation2D>().Stop ();
			shieldOrbAnimation.GetComponent<Animation2D>().Hide ();

			shieldOrbAnimation.GetComponent<Animation>().Stop ();

			shieldOrbAnimation.GetComponent<ParticleSystem>().Stop ();
			shieldOrbAnimation.GetComponent<ParticleSystem>().Clear ();
		}

		wireAnimation.Stop ();
		wireAnimation.outputRenderer.color = Color.grey;

		if(forceFieldAnimation) {
			forceFieldAnimation.Stop ();
			forceFieldAnimation.Hide ();
		}
	}
}
