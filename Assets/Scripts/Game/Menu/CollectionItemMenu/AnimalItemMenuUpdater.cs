using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalItemMenuUpdater : CollectionItemMenuUpdater {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UpdateVisibleButtons(CollectionManager collectionManager, List<ButtonRow> buttonRows) {
        List<AnimalInfo> allAnimalInfo = collectionManager.GetAllAnimalInfo();
       
        for(int i = 0; i < buttonRows.Count ; i++) {
           ButtonRow buttonRow = buttonRows[i];

            for(int j = 0; j < buttonRow.buttons.Count; j++) {

                CollectionItemButton collectionItemButton = (CollectionItemButton) buttonRow.buttons[j];

                AnimalInfo foundAnimalInfo = allAnimalInfo.Find(animalInfo => animalInfo.name.Equals(collectionItemButton.name));
                bool showButton = (foundAnimalInfo != null);

                collectionItemButton.SetVisible(showButton);
            }
        }
    }
}
