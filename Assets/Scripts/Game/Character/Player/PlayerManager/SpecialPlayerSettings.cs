using UnityEngine;
using System.Collections;

public class SpecialPlayerSettings : MonoBehaviour {

    public bool startsWithWeapon = false;
    public bool isAtBoss = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ApplySettings(Player player) {
        player.isAtBoss = isAtBoss;
        player.GetComponent<WeaponManager>().startsWithWeapon = startsWithWeapon;
    }
}
