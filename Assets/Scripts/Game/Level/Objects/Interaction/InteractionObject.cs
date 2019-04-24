using UnityEngine;
using System.Collections;

public class InteractionObject : DispatchBehaviour {
	
	protected bool canInteract = true;

	// Use this for initialization
	public virtual void Start() {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			canInteract = true;
			ShowInput(player);
			EnableInteraction (player);
		}
	}

	public virtual void OnTriggerExit(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			DisableInteraction(player);
		}
	}

	public virtual void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
            EnableInteraction(player);
		}
	}
	
	public virtual void OnCollisionExit(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			DisableInteraction(player);
		}
	}

	public virtual void OnInteract(Player player) {
		if(player && canInteract) {
			HideInput(player);
		}
	}

	public virtual void OnDanceOnBeat(Player player) {
		Logger.Log ("YOU DANCED ON THE BEAT!!");
	}

	public virtual void OnDanceOffBeat(Player player) {
		Logger.Log ("YOU DANCED OFF THE BEAT!!");
	}

	public virtual void EnableInteraction(Player player) {
        canInteract = true;
        ShowInput(player);
        player.GetComponent<PlayerInputComponent>().AddEventListener(this.gameObject);
		player.GetComponent<PlayerDanceComponent>().AddEventListener(this.gameObject);
    }

	public virtual void DisableInteraction(Player player) {
		canInteract = false;
		HideInput(player);
		player.GetComponent<PlayerInputComponent>().RemoveEventListener(this.gameObject);
		player.GetComponent<PlayerDanceComponent>().RemoveEventListener(this.gameObject);
	}

	public virtual void ShowInput(Player player) {
		string[] deviceNameAndInputName = PlayerInputHelper.DecideDeviceNameAndInputNameForInteract();

		player.ShowButton (deviceNameAndInputName[0], deviceNameAndInputName[1]);
	}

	public virtual void HideInput(Player player) {
		player.HideButton ();
	}
}
