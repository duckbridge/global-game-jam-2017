using UnityEngine;
using System.Collections;

public class BoomBox : BeatObject {

	public bool isOnPlayerBack = false;
	public Cassette cassetteGameObject;

	private Color cassetteColorForCassetteToSpawn;
	private AnimationManager2D animationManager;
	private Transform cassetteSpawnTransform;
	private ParticleSystem particleEmitter, onBeatParticleEmitter, offBeatParticleEmitter, onRollParticleEmitter;

    private bool canEmitParticle;

	void Awake() {
		animationManager = GetComponentInChildren<AnimationManager2D>();
		cassetteSpawnTransform = this.transform.Find("CassetteSpawnTransform");

        particleEmitter = this.transform.Find("BoomboxParticles").GetComponent<ParticleSystem>();

        if(this.transform.Find("OnBeatBoomboxParticles")) {
            onBeatParticleEmitter = this.transform.Find("OnBeatBoomboxParticles").GetComponent<ParticleSystem>();
        }

        if(this.transform.Find("OffBeatBoomboxParticles")) {
            offBeatParticleEmitter = this.transform.Find("OffBeatBoomboxParticles").GetComponent<ParticleSystem>();
        }

		if (this.transform.Find ("OnRollBoomboxParticles")) {
			onRollParticleEmitter = this.transform.Find ("OnRollBoomboxParticles").GetComponent<ParticleSystem> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
    public override void OnBeatEvent() {
        if(canEmitParticle) {
            particleEmitter.Emit(1);
        }
    }

    public void StartEmitting() {
        
        canEmitParticle = true;
    }

    public void StopEmitting() {
        canEmitParticle = false;
        particleEmitter.Stop();
    }

	public void OnArrivedAtDestination(Cassette cassetteGO) {
		Destroy(cassetteGO.gameObject);
		SceneUtils.FindObject<MusicManager>().PlayNewMusicOnReceive();
	}

	private void SpawnCassette() {
		Cassette cassette = (Cassette) GameObject.Instantiate(cassetteGameObject, cassetteSpawnTransform.position, Quaternion.identity);
		cassette.SetForegroundColor(cassetteColorForCassetteToSpawn);
		
		if(isOnPlayerBack) {
			MusicManager musicManager = SceneUtils.FindObject<MusicManager>();
			cassette.FlyToMusicManager(musicManager.GetTargetAboveManager());
		} else {
			cassette.FlyToMusicManager(null);
		}
	}

    public void EmitOnBeatParticle() {
        onBeatParticleEmitter.Emit(1);
    }

	public void EmitOnRollBeatParticle() {
		onRollParticleEmitter.Emit (1);
	}

    public void EmitOffBeatParticle() {
        offBeatParticleEmitter.Emit(1);
    }

	public void OnMusicSwap(Color cassetteColor) {
		this.cassetteColorForCassetteToSpawn = cassetteColor;

		if(isOnPlayerBack) {
			SpawnCassette();
		} else {
			animationManager.PlayAnimationByName("SpitOutCassette", true);
			Invoke ("SpawnCassette", .1f);
		}
	}

	public void ShowTextBox(string textBoxName) {
		Player player = SceneUtils.FindObject<Player>();
		TextBoxManager textboxmanager = player.transform.Find("BoomboxTextboxes/"+textBoxName).GetComponent<TextBoxManager>();

		textboxmanager.animationManagerToUseForTalking = this.animationManager;
		textboxmanager.ResetShowAndActivate();
	}
}
