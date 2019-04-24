using UnityEngine;
using System.Collections;

public class GameObjectThatActivatesOnbatterTaken : MonoBehaviour {

    public GameObject gameObjectToActivate;
	// Use this for initialization
	void Awake () {
	    PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        if(playerSaveComponent) {
            if(!playerSaveComponent.HasFreeBatteryInDetroit()) {
                gameObjectToActivate.SetActive(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
