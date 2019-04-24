using UnityEngine;
using System.Collections;

public class BossWeaponRope : WeaponRope {

    public HingeJoint head;
    public Rigidbody tail;

	// Use this for initialization
    public override void Start() {
        OnSpawned(head, tail);
    }
}
