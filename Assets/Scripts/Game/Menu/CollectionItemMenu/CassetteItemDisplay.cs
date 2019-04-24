using UnityEngine;
using System.Collections;

public class CassetteItemDisplay : CollectionItemDisplay {

    public override void Show(CollectionItemButton collectionItemButton) {
        
        this.transform.Find("Name").GetComponent<TextMesh>().text = "TEST";
        this.transform.Find("Description").GetComponent<TextMesh>().text = "TEST DESCR";
        //this.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = animalItemButton.GetSpriteRenderer().sprite;

        base.Show(collectionItemButton);
    }
}
