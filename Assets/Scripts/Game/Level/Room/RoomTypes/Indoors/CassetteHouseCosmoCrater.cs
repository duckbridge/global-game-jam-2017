using UnityEngine;
using System.Collections;

public class CassetteHouseCosmoCrater : MonoBehaviour {

    public CassettePickup cassettePickup;
    public ActionOnCassetteRoomDisable actionOnCassetteRoomDisable;

	// Use this for initialization
	void Start () {
        if(cassettePickup) {
            cassettePickup.AddEventListener(this.gameObject);
        }
	}

    public void OnCassettePickedUp() {
        actionOnCassetteRoomDisable.DoAction(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
