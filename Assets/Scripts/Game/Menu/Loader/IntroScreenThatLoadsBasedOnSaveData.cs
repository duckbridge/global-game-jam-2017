using UnityEngine;
using System.Collections;

public class IntroScreenThatLoadsBasedOnSaveData : MonoBehaviour {

    public Scene defaultSceneToLoad, sceneToLoadOnNoSaveData;
    public float timeout;

    private bool hasSavedData = false;

    // Use this for initialization
    void Start () {
        Cursor.visible = false; 
        Invoke ("LoadLevel", timeout);
        hasSavedData = GetComponent<PlayerSaveComponent>().HasSavedData();
    }
    
    private void LoadLevel() {
        Scene sceneToLoad = hasSavedData ? defaultSceneToLoad : sceneToLoadOnNoSaveData;
        Application.LoadLevel(sceneToLoad.ToString());
    }
}
