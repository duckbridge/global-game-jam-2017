using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SpawnEnemies : CutSceneComponent {

		public Room roomToSetAsEnemyParent;

		public Enemy[] enemiesToSpawn;

		public override void OnActivated () {
			for(int i = 0; i < enemiesToSpawn.Length ; i++) {
				enemiesToSpawn[i].gameObject.SetActive(true);
				enemiesToSpawn[i].OnActivate();
				enemiesToSpawn[i].OnSpawned(roomToSetAsEnemyParent);
				enemiesToSpawn[i].transform.parent = roomToSetAsEnemyParent.transform;

				SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, enemiesToSpawn[i].gameObject);


				roomToSetAsEnemyParent.AddEnemySpawned(enemiesToSpawn[i]);
				enemiesToSpawn[i].AddEventListener(roomToSetAsEnemyParent.gameObject);
			}

			if(enemiesToSpawn.Length > 0) {
				SceneUtils.FindObject<BattleBorders>().TurnOnBattleBorders();
			}

			DeActivate();
		}
	}
}
