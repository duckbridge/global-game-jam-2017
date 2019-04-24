using UnityEngine;
using System.Collections;

public class SavedAnimalSpawner : MonoBehaviour {

    public Room room;

	// Use this for initialization
	void Start () {
		Invoke ("SpawnSaved", 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SpawnSaved() {
		PlayerSaveComponent saveComponent = SceneUtils.FindObject<PlayerSaveComponent>();

		foreach(string animalName in saveComponent.GetSavedAnimals()) {

            Logger.Log(animalName);

			if(animalName != null) {
				
                AnimalCompanion animalCompanion = (AnimalCompanion) GameObject.Instantiate(Resources.Load("Critters/" + animalName, typeof(AnimalCompanion)), this.transform.position, Quaternion.identity);

                animalCompanion.DisableHealthbar();
            
                animalCompanion.transform.parent = this.transform;
                animalCompanion.transform.eulerAngles = new Vector3(90f, 0f, 0f);

                animalCompanion.SetCurrentRoom(room);

                animalCompanion.GetComponent<AnimalActionManager>().startAnimalActionType = AnimalActionType.RUN_AROUND_VILLAGE;
                animalCompanion.GetComponent<AnimalActionManager>().Initialize();
			}
		}
	}
}
