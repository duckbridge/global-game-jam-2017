using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class EquipDBOnPlayer : CutSceneComponent {

		public GameObject oldDB;
		public Weapon boomboxToEquip;

		public override void OnActivated () {

            if(oldDB) {
			    Destroy(oldDB);
            }

			Player player = SceneUtils.FindObject<Player>();
			player.PlayEquipDBAnimation ();
			player.GetComponent<WeaponManager>().RetrieveWeapon(boomboxToEquip, false, true);

			DeActivate();
		}
	}
}