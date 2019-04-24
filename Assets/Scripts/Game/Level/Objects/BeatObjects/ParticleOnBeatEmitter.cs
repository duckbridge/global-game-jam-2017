using UnityEngine;
using System.Collections;

public class ParticleOnBeatEmitter : BeatObject {

	private ParticleSystem particleEmitter;

	// Use this for initialization
	public override void Start () {
		particleEmitter = GetComponent<ParticleSystem> ();
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnBeatEvent() {
		particleEmitter.Emit(1);
	}
}
