using UnityEngine;
using System.Collections;

public class LAVillagerThrowCassette : VillagerSpecialAction {

    public Vector3 cassetteThrowForce = new Vector3(-165f, 0f, 165f);
    public CassettePickup cassettePickup;


    public override void DoAction(Villager villager) {
        base.DoAction(villager);

        cassettePickup.gameObject.SetActive(true);
        cassettePickup.transform.parent = this.transform.parent;
        cassettePickup.GetComponent<Rigidbody>().velocity = cassetteThrowForce;

    }
}
