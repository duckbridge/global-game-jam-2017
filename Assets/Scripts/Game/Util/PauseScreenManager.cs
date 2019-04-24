using UnityEngine;
using System.Collections;

public class PauseScreenManager : MonoBehaviour {

    public GameObject map;
    private PauseScreenTypes currentType = PauseScreenTypes.Regular;
    private PauseScreen pauseScreen;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void SwitchPauseScreen(PauseScreenTypes newPauseScreenType) { 
        map.transform.parent = this.transform;

        Transform currentPauseScreen = this.transform.Find(currentType.ToString());
        Transform newPauseScreen = this.transform.Find(newPauseScreenType.ToString());
        
        SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, newPauseScreen.gameObject);

        currentPauseScreen.gameObject.SetActive(false);
        newPauseScreen.gameObject.SetActive(true);

        map.transform.parent = newPauseScreen.Find("PauseScreenContainer");

        map.transform.localPosition = new Vector3(0.8f, 0f, 0f);

        SceneUtils.FindObject<MinimapManager>().SwitchMinimap(currentType, newPauseScreenType);
        
        pauseScreen = newPauseScreen.GetComponent<PauseScreen>();
    
        currentType = newPauseScreenType;
    }

    public PauseScreenTypes GetPauseScreenType() {
        return currentType;
    }

    public PauseScreen GetPauseScreen() {
        return pauseScreen;
    }
}
