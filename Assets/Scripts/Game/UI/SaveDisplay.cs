using UnityEngine;
using System.Collections;

public class SaveDisplay : MonoBehaviour {

    public float saveDisplayTimeout = 2f;
    private PlayerCharacterName characterName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowAnimationForCharacter(PlayerCharacterName playerCharacterName) {
        this.characterName = playerCharacterName;

        Transform saveDisplayTransform = this.transform.Find(characterName.ToString());

        SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, saveDisplayTransform.Find("SaveSound").gameObject);

        saveDisplayTransform.gameObject.SetActive(true);
        saveDisplayTransform.Find("Display").GetComponent<Animation2D>().Play(true);
        saveDisplayTransform.Find("SaveSound").GetComponent<SoundObject>().Play(true);

        Invoke("HideSaveDisplay", saveDisplayTimeout);

    }

    private void HideSaveDisplay() {
        Transform saveDisplayTransform = this.transform.Find(characterName.ToString());
        saveDisplayTransform.gameObject.SetActive(false);

    }
}
