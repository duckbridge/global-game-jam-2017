using UnityEngine;
using System.Collections;

public class BoomboxEquip : BoomboxAction {

	public Weapon boomboxToEquip;

	protected override void OnStarted () {

		Player player = SceneUtils.FindObject<Player>();

		player.GetComponent<WeaponManager>().RetrieveWeapon(boomboxToEquip, false, true);
		player.PlayEquipDBAnimation ();

		Destroy (boomboxCompanion.gameObject);

	}
}