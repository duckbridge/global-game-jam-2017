using UnityEngine;
using System.Collections;

public class AnimalRunAroundVillage : AnimalAction {

    public string runPositionsName = "VillageRunPositions";
	public float closeToTargetDistance = .3f;
    public AnimalActionType actionTypeOnDone = AnimalActionType.IDLE;
    
	private VillageRunPosition currentRunPosition;
	private VillageRunPosition[] allVillageRunPositions;
	
	private Room villageEntrance;
    private Player player;

	protected override void OnUpdate () {
		if(currentRunPosition) {

			if(MathUtils.GetDistance2D(currentRunPosition.transform.position, animalCompanion.transform.position) > closeToTargetDistance) {

				animalBodycontrol.DoMoveWithMinimumSpeed(currentRunPosition.transform.position, .05f);
			
			} else {
				if(currentRunPosition.GetRandomNeighbour() != null) {
					currentRunPosition = currentRunPosition.GetRandomNeighbour();

					animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(currentRunPosition.transform);
                    animalCompanion.GetComponent<CharacterToTargetTurner>().OnUpdate();
                    animalCompanion.GetAnimationControl().PlayAnimationByName("Walking", true);
				} else {

                    animalCompanion.transform.position = 
                        new Vector3(currentRunPosition.transform.position.x, 
                            animalCompanion.transform.position.y,
                            currentRunPosition.transform.position.z);

					FinishAction(actionTypeOnDone);
				}
			}
		}
	}
	
	protected override void OnStarted () {
		animalCompanion.DisableHealthbar();
        animalCompanion.StopHeartParticles();

		villageEntrance = animalCompanion.GetCurrentRoom();

		if(allVillageRunPositions == null || allVillageRunPositions.Length == 0) {
			allVillageRunPositions = villageEntrance.transform.Find ("AnimalRunPositions/"+runPositionsName).GetComponent<VillageRunPositions>().villageRunPositions;
		}

		currentRunPosition = allVillageRunPositions[0];

		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(currentRunPosition.transform);
        animalCompanion.GetComponent<CharacterToTargetTurner>().OnUpdate();
		animalCompanion.GetAnimationControl().PlayAnimationByName("Walking", true);

	}
}