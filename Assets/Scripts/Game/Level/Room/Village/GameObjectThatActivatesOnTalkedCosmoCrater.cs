using UnityEngine;
using System.Collections;

public class GameObjectThatActivatesOnTalkedCosmoCrater : MonoBehaviour {

    public GameObject gameObjectToActivate;
	// Use this for initialization
	void Start () {
	   Invoke("LoadDelayed", 1f);
	}

    private void LoadDelayed() {
        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        if(playerSaveComponent) {
            if(playerSaveComponent.HasTalkedToBotInCosmoCrater()) {
                gameObjectToActivate.SetActive(true);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
