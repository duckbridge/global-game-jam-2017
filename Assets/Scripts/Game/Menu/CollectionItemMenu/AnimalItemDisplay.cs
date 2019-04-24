using UnityEngine;
using System.Collections;

public class AnimalItemDisplay : CollectionItemDisplay {

    public override void Show(CollectionItemButton collectionItemButton) {
        AnimalItemButton animalItemButton = collectionItemButton.GetComponent<AnimalItemButton>();
        
        this.transform.Find("Name").GetComponent<TextMesh>().text = animalItemButton.animalName;
        this.transform.Find("Description").GetComponent<TextMesh>().text = animalItemButton.animalDescription;
        this.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = animalItemButton.GetSpriteRenderer().sprite;
        
		this.isShown = true;
    }
}
