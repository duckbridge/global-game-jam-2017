using UnityEngine;
using System.Collections;

public class GameItemButton : CollectionItemButton {

    public string gameName;

    [TextArea(3,10)]
    public string gameDecription;
  
	// Use this for initialization
	void Start () {
  	}

	// Update is called once per frame
	void Update () {

	}

    public SpriteRenderer GetSpriteRenderer() {
        return this.transform.Find("Game").GetComponent<SpriteRenderer>();
    }

}
