using UnityEngine;
using System.Collections;

public class AnimalTeleportToJunkyard : AnimalAction {

	protected override void OnStarted () {
        animalBodycontrol.StopMoving();

        Player player = SceneUtils.FindObject<Player>();
        player.GetComponent<PlayerCircleGrowComponent>().AddAnimalToJunkyardQueue(animalCompanion.GetOriginalName());
       
        BoomboxCompanion boomboxCompanion = player.GetBoomboxCompanion();
        boomboxCompanion.GetComponent<PlayerSpitter>().SwallowObject(animalCompanion.gameObject, false);
        
        animalCompanion.GetAnimationManager().PlayAnimationByName("Teleporting", true);

        Invoke("DestroyGameObject", 1f);
        
    }

    private void DestroyGameObject() {
        Destroy(animalCompanion.gameObject);
    }
}
