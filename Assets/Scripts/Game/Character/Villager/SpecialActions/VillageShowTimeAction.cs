using UnityEngine;
using System.Collections;
using System;

public class VillageShowTimeAction : VillagerSpecialAction {

    public GameObject timeDisplay;
    public float showTimeout = 1f;

    public override void DoAction(Villager villager) {
        
        DateTime date = DateTime.Now;
        string dateAsString = date.ToString("HH:mm"); 

        timeDisplay.GetComponent<TextMesh>().text = dateAsString;
        timeDisplay.SetActive(true);

        CancelInvoke("HideDisplay");
        Invoke("HideDisplay", showTimeout);

    }

    private void HideDisplay() {
        timeDisplay.SetActive(false);
		DispatchMessage ("OnVillageActionDone", null);
    }
}
