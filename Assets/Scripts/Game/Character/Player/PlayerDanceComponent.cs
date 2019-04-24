using UnityEngine;
using System.Collections;

public class PlayerDanceComponent : DispatchBehaviour {

	private Player player;
	private CharacterControl characterControl;
	private BodyControl bodyControl;

	private SoundObject onDanceSound, onDanceFailedSound;

	private BeatListener beatListener;
	// Use this for initialization
	void Awake() {
		player = GetComponent<Player> ();
		characterControl = GetComponent<CharacterControl> ();
		FindBeatListener ();
		bodyControl = GetComponent<BodyControl> ();

		onDanceSound = this.transform.Find ("Sounds/OnDanceCorrectSound").GetComponent<SoundObject> ();
		onDanceFailedSound = this.transform.Find ("Sounds/OnDanceFailSound").GetComponent<SoundObject> ();
	}

	private void FindBeatListener() {
		beatListener = this.transform.Find("MusicManager").GetComponent<BeatListener>();
	}

	public void ResetBeatListener() {
		FindBeatListener ();
	}

	public void SetBeatListener(BeatListener newBeatListener) {
		this.beatListener = newBeatListener;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void DoDance() {

		if (beatListener.CanDoBeat(false)) {
			player.PlayRandomDanceFrame ();
			onDanceSound.Play (true);
			DispatchMessage("OnDanceOnBeat", GetComponent<Player>());
		} else {
			player.PlayFailDanceAnimation ();
			DispatchMessage("OnDanceOffBeat", GetComponent<Player>());
			onDanceFailedSound.Play (true);
		}

		player.GetAnimationManager ().DisableSwitchAnimations ();
		bodyControl.DisableMoving ();

		Invoke ("StopDancing", .5f);
	}

	private void StopMoving() {
		player.GetComponent<BodyControl> ().StopMoving ();
	}

	private void StopDancing() {

		this.transform.Find ("Shadow").GetComponent<SpriteRenderer> ().enabled = true;

		player.GetAnimationManager ().EnableSwitchAnimations ();
		bodyControl.ReEnableMoving ();
		characterControl.SwitchState (CharacterControl.CharacterState.Idle);

	}
}
