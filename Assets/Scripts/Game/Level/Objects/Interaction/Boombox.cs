using UnityEngine;
using System.Collections;

public class Boombox : InteractionObject {

	private MusicManager musicManager;
	private ParticleSystem boomboxParticles;

	// Use this for initialization
	void Start () {
		musicManager = SceneUtils.FindObject<MusicManager>();
		boomboxParticles = this.transform.Find("BoomboxParticles").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			base.OnInteract (player);

			if(musicManager.HasMusicMuted()) {
				boomboxParticles.Play();
			} else {
				boomboxParticles.Stop();

			}

			musicManager.MuteUnMuteMusic();
		}
	}

}
