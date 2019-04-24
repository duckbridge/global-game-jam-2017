using UnityEngine;
using System.Collections;

public class GameItemDisplay : CollectionItemDisplay {

    public override void Show(CollectionItemButton collectionItemButton) {
		GameItemButton gameItemButton = collectionItemButton.GetComponent<GameItemButton>();
        
        this.transform.Find("Name").GetComponent<TextMesh>().text = gameItemButton.gameName;
        this.transform.Find("Description").GetComponent<TextMesh>().text = gameItemButton.gameDecription;
        this.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = gameItemButton.GetSpriteRenderer().sprite;
        
		this.isShown = true;
    }
}
