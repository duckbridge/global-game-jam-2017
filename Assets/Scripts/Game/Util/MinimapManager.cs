using UnityEngine;
using System.Collections;

public class MinimapManager : MonoBehaviour {

    public GameObject minimapContent;
   
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	
	}


    public void SwitchMinimap(PauseScreenTypes currentType, PauseScreenTypes newPauseScreenType) { 
        minimapContent.transform.parent = this.transform;

        Transform currentMinimap = this.transform.Find(currentType.ToString());
        Transform newMinimap = this.transform.Find(newPauseScreenType.ToString());

        SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, newMinimap.gameObject);

        currentMinimap.gameObject.SetActive(false);
        newMinimap.gameObject.SetActive(true);

        minimapContent.transform.parent = newMinimap;

        minimapContent.transform.localPosition = Vector3.zero;

        
    }
}
