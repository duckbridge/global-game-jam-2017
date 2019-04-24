using UnityEngine;
using System.Collections;

public class Door : InteractionObject {
	
    protected SoundObject doorOpenSound, doorCloseSound;

    protected Animation2D doorAnimation;
	protected Player player;

    void Awake() {
        if(this.transform.Find("DoorAnimation")) {
            doorAnimation = this.transform.Find("DoorAnimation").GetComponent<Animation2D>();
        }

        if(this.transform.Find("Sounds/CloseSound")) {
            doorCloseSound = this.transform.Find("Sounds/CloseSound").GetComponent<SoundObject>();
        }

        if(this.transform.Find("Sounds/OpenSound")) {
            doorOpenSound = this.transform.Find("Sounds/OpenSound").GetComponent<SoundObject>();
        }
    }

	public void OnEntered(Player player) {
		player.SetInside(true);
		player.transform.position = GetSpawnPosition().position;

		this.canInteract = false;
		player.GetComponent<PlayerInputComponent>().RemoveEventListener(this.gameObject);
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			this.player = player;
			base.OnInteract(player);
			player.SetInside(false);
			player.GetComponent<PlayerInputComponent> ().enabled = false;

			DoorEnterExitAnimation doorEnterExitAnimation = SceneUtils.FindObject<DoorEnterExitAnimation> ();

			doorEnterExitAnimation.AddEventListener (this.gameObject);
			doorEnterExitAnimation.hideWhenDone = false;
			doorEnterExitAnimation.Play (true, false);
		}
	}

    public override void OnTriggerEnter(Collider coll) {
        base.OnTriggerEnter(coll);
    
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
            OpenDoor();
        }
    }

    public override void OnTriggerExit(Collider coll) {
        base.OnTriggerExit(coll);

        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
            CloseDoor();
        }
    }

    public void OpenDoor() {
        if(doorAnimation) {
            bool reset = false;
    
            if(doorAnimation.GetCurrentFrame() >= (doorAnimation.frames.Length - 1) || doorAnimation.GetCurrentFrame() <= 0) {
                reset = true; 
            }   

            doorAnimation.Play(reset);
        }

		if(doorOpenSound) {
			doorOpenSound.Play(true);
		}

    }

    public void CloseDoor() {
        if(doorAnimation) {
            if(doorAnimation.GetCurrentFrame() > 0) {
                bool reset = false;
    
                if(doorAnimation.GetCurrentFrame() >= (doorAnimation.frames.Length - 1) || doorAnimation.GetCurrentFrame() <= 0) {
                    reset = true;
                }
        
                doorAnimation.Play(reset, true);
            }
        }

		if(doorCloseSound) {
			doorCloseSound.Play(true);
		}
    }

	public Transform GetSpawnPosition() {
		return this.transform.Find("SpawnPosition");
	}


	public void OnAnimationDone(Animation2D animation2D) {
		if (animation2D.name == "OnDoorEnterExitAnimation") {
			player.GetComponent<PlayerInputComponent> ().enabled = true;
			DispatchMessage ("OnPlayerExitted", null);

			DoorEnterExitAnimation doorEnterExitAnimation = SceneUtils.FindObject<DoorEnterExitAnimation> ();

			doorEnterExitAnimation.RemoveEventListener(this.gameObject);
			doorEnterExitAnimation.hideWhenDone = true;
			doorEnterExitAnimation.Play (true, true);

		}
	}
}
