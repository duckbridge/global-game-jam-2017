using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCircleGrowComponent : MonoBehaviour {

	public int[] amountsOfAnimalsRequiredForGrow;

	private RunPositions runCircle;
	
	private int currentIndex = 0;
	private List<AnimalCompanion> animalCompanions = new List<AnimalCompanion>();
    private List<string> animalCompanionsToSpawnInJunkyard = new List<string>();

    void Start () {
        runCircle = GetComponentInChildren<RunPositions>();
    }
    
    // Update is called once per frame
    void Update () {
    
    }

	public void OnAnimalAdded(AnimalCompanion animalCompanion) {
		animalCompanions.Add (animalCompanion);

		if(animalCompanions.Count > amountsOfAnimalsRequiredForGrow[currentIndex]) {

			if((currentIndex + 1) < amountsOfAnimalsRequiredForGrow.Length) {

				++currentIndex;
				runCircle.GrowCircleTo((int) (runCircle.transform.localScale.x + 1));
			
			}
		}
	}

	public void OnAnimalDied(AnimalCompanion animalCompanion) {
		animalCompanions.Remove(animalCompanion);

		if(currentIndex - 1 > 0) {

			if(animalCompanions.Count <= amountsOfAnimalsRequiredForGrow[currentIndex - 1]) {

				--currentIndex;
				runCircle.GrowCircleTo((int) (runCircle.transform.localScale.x - 1));
			}
		}
	}


	public void OnVillageEntered(RoomNode roomNode) {

        Room roomNodeRoom = roomNode.GetRoom();

        SaveAllAnimalsOfPlayer(roomNodeRoom);

        SpawnAllAnimalsInQueue(roomNodeRoom);
	}

    public void AddAnimalToJunkyardQueue(string animalName) {
        if(!this.animalCompanionsToSpawnInJunkyard.Contains(animalName)) {
            this.animalCompanionsToSpawnInJunkyard.Add(animalName);
        }
    }

    public List<string> GetAnimalCompanionsInJunkyardQueue() {
        return this.animalCompanionsToSpawnInJunkyard;
    }

    private void SaveAllAnimalsOfPlayer(Room room) {
        for(int i = 0 ; i < animalCompanions.Count ; i++) { 
    
            AnimalCompanion animalCompanion = animalCompanions[i];
            OnAnimalDied(animalCompanion);

            SceneUtils.FindObject<PlayerSaveComponent>().AddSavedAnimal(animalCompanion.GetOriginalName());
            SceneUtils.FindObject<CollectionManager>().AddAnimalAsInfo(animalCompanion);

            animalCompanion.SetCurrentRoom(room);
    
            if(room.GetComponent<JunkyardEntranceRoom>()) {
                animalCompanion.SwitchAnimalAction(AnimalActionType.RUN_AROUND_VILLAGE);
            } else {
                animalCompanion.SwitchAnimalAction(AnimalActionType.RUN_TO_TELEPORTER);
            }

            i--;
        }
    }

    private void SpawnAllAnimalsInQueue(Room room) {
        for(int i = 0 ; i < animalCompanionsToSpawnInJunkyard.Count ; i++) { 
            AnimalCompanion animalCompanion = (AnimalCompanion) GameObject.Instantiate(Resources.Load("Critters/" + animalCompanionsToSpawnInJunkyard[i], typeof(AnimalCompanion)), this.transform.position, Quaternion.identity);

            animalCompanion.DisableHealthbar();
        
            animalCompanion.transform.parent = this.transform;
            animalCompanion.transform.eulerAngles = new Vector3(90f, 0f, 0f);

            animalCompanion.SetCurrentRoom(room);

            animalCompanion.GetComponent<AnimalActionManager>().startAnimalActionType = AnimalActionType.RUN_AROUND_VILLAGE;
            animalCompanion.GetComponent<AnimalActionManager>().Initialize();
        
            animalCompanionsToSpawnInJunkyard.RemoveAt(i);
            i--;
        }
    }
}
