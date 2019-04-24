using UnityEngine;
using System.Collections;

public class EnemyThatSpawnsAnimal : Enemy {

	public Transform animalSpawnPosition;
	public CritterNames critterName = CritterNames.Chicken;


    public override void Awake() {
        base.Awake();
        canHitPlayer = false;
    }

	protected override void DoExtraOnDeath () {
		base.DoExtraOnDeath ();

		AnimalCompanion animalCompanion = (AnimalCompanion)
			GameObject.Instantiate(Resources.Load("Critters/" + critterName.ToString(), typeof(AnimalCompanion)), animalSpawnPosition.position, Quaternion.Euler(new Vector3(90f, 0f, 0f))) as AnimalCompanion;

		animalCompanion.SetOriginalName(critterName.ToString());
		animalCompanion.SetCurrentRoom(currentRoom);

        BoomBox boomboxOnBack = player.GetComponentInChildren<BoomBox>();
        if(boomboxOnBack) {
            boomboxOnBack.ShowTextBox("OnAnimalSaved");
        }
    }
}
