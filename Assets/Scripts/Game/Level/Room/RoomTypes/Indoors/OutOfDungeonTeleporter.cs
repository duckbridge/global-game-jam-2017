using UnityEngine;
using System.Collections;

public class OutOfDungeonTeleporter : DungeonDoor {

    public float emissionRateOnTeleport = 100f;
	public float teleportTimeout = 1f;

    private Animation2D getOnAnimation, getOffAnimation;

    void Awake() {
        getOnAnimation = this.transform.Find("GetOnTeleporter").GetComponent<Animation2D>();
        getOffAnimation = this.transform.Find("GetOffTeleporter").GetComponent<Animation2D>();
    }

	public override void OnInteract (Player player) {
		if(canInteract) {
			player.GetComponent<PlayerInputComponent>().enabled = false;
            
            player.PlaySwallowedAnimation();   

            ParticleSystem.EmissionModule emissionModule = this.transform.Find("Particles").GetComponent<ParticleSystem>().emission;
            emissionModule.rate = new ParticleSystem.MinMaxCurve(emissionRateOnTeleport);
            
			Invoke ("LoadNewScene", teleportTimeout);
		}
	}

	private void LoadNewScene() {
		Loader.LoadScene (sceneToLoad, loadingScreenType);
	}

    public override void OnTriggerEnter(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {

            getOnAnimation.Play(true);
            getOffAnimation.StopAndHide();          
        }
        
        base.OnTriggerEnter(coll);
    }
    
    public override void OnTriggerExit(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {

            getOnAnimation.StopAndHide();
            getOffAnimation.Play(true);    

        }

        base.OnTriggerExit(coll);
    }
}
