using UnityEngine;
using System.Collections;

public class Teleporter : InteractionObject {
	
    public float emissionRateOnTeleport = 100f;
    private ParticleSystem.MinMaxCurve originalEmissionRate;

    private Animation2D getOnAnimation, getOffAnimation;
    private Player player;

    void Awake() {    
        ParticleSystem.EmissionModule emissionModule = this.transform.Find("Particles").GetComponent<ParticleSystem>().emission;
        originalEmissionRate = emissionModule.rate;
        
        getOnAnimation = this.transform.Find("GetOnTeleporter").GetComponent<Animation2D>();
        getOffAnimation = this.transform.Find("GetOffTeleporter").GetComponent<Animation2D>();
    }

	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInteract (Player player) {

		if(canInteract) {
			base.OnInteract (player);
            
            this.player = player;

			TileBlock currentTileBlock = player.GetCurrentTileBlock();
			player.GetComponent<VillageTeleporterComponent>().UnlockTileblockTeleporter(ref currentTileBlock);

            DisableInteraction(player);
		}
	}

    public void PlayPlayerTeleportAnimationDelayed(float delay) {

        Invoke("PlayPlayerTeleportAnimation", delay);

    }

    private void PlayPlayerTeleportAnimation() {
        player.PlaySwallowedAnimation();   
    }

    public void IncreaseEmission(float resetTimeout = 0f) {
        ParticleSystem.EmissionModule emissionModule = this.transform.Find("Particles").GetComponent<ParticleSystem>().emission;
        emissionModule.rate = new ParticleSystem.MinMaxCurve(emissionRateOnTeleport);

        if(resetTimeout > 0) {
            Invoke("ResetEmission", resetTimeout);
        }
    }

    public void ResetEmission() {
         CancelInvoke("ResetEmission");
         ParticleSystem.EmissionModule emissionModule = this.transform.Find("Particles").GetComponent<ParticleSystem>().emission;
         emissionModule.rate = originalEmissionRate;
    }

	public override void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			player.GetComponent<VillageTeleporterComponent>().SetAtTeleporter(true);
            player.GetComponent<VillageTeleporterComponent>().SetCurrentTeleporter(this);

            getOnAnimation.Play(true);
            getOffAnimation.StopAndHide();   		
        }

        AnimalCompanion animalCompanion = coll.gameObject.GetComponent<AnimalCompanion>();
        if(animalCompanion) {
            Logger.Log("setting animal at teleporter");
            animalCompanion.SetAtTeleporter(this);  
        }
		
		base.OnTriggerEnter(coll);
	}
	
	public override void OnTriggerExit(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			player.GetComponent<VillageTeleporterComponent>().SetAtTeleporter(false);
            player.GetComponent<VillageTeleporterComponent>().SetCurrentTeleporter(null);

            ResetEmission();

            getOnAnimation.StopAndHide();
            getOffAnimation.Play(true);    

		}

		base.OnTriggerExit(coll);
	}

	public void ShowAndActivate() {

		this.GetComponent<Collider>().enabled = true;

	}
}
