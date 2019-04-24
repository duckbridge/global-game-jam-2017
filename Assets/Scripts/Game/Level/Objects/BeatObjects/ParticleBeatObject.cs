using UnityEngine;
using System.Collections;

public class ParticleBeatObject : BeatObject {


	private ParticleSystem particleEmitterToUse;

	public void Awake() {
		particleEmitterToUse = GetComponent<ParticleSystem>();
	}

	public override void OnBeatEvent () {
		if(particleEmitterToUse) {
			particleEmitterToUse.Emit(1);
		}
	}
}
