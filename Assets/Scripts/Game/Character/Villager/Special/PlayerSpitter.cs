using UnityEngine;
using System.Collections;

public class PlayerSpitter : DispatchBehaviour {

    public float spitOutDistance = 5f;

    public float swallowTime = .7f;
    public float spitOutTime = .8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpitOutPlayer(Player player) {
		
        GetComponent<BoomboxCompanion>().GetAnimationManager().PlayAnimationByName("SpitOutPlayer");
		player.OnSpitOut ();

		Vector3 newPosition = this.transform.position - new Vector3(0f, 0f, spitOutDistance);
        iTween.MoveTo(player.gameObject, new ITweenBuilder().SetPosition(newPosition).SetTime(spitOutTime).SetEaseType(iTween.EaseType.linear).Build());
    }

	public void SwallowPlayer(Player player, bool fireEventWhenDone) {

        player.GetComponent<PlayerInputComponent>().enabled = false;
        player.PlaySwallowedAnimation();

		SwallowObject(player.gameObject, fireEventWhenDone);
    }

	public void SwallowObject(GameObject go, bool fireEventOnDone) {
        GetComponent<BoomboxCompanion>().GetAnimationManager().PlayAnimationByName("SwallowPlayer");

		Hashtable iTweenBuilder = new ITweenBuilder ().SetPosition (this.transform.position).SetTime (swallowTime).SetEaseType (iTween.EaseType.linear).Build ();
		if (fireEventOnDone) {
			iTweenBuilder.Add ("oncomplete", "OnSwallowingObjectDone");
			iTweenBuilder.Add ("onCompleteTarget", this.gameObject);
		}

		iTween.MoveTo(go, iTweenBuilder);
    }

	public void OnSwallowingObjectDone() {
		DispatchMessage ("OnSwallowedObject", null);
	}
}
