using UnityEngine;
using System.Collections;

public class CassetteItemButton : CollectionItemButton {

    [TextArea(3,10)]
    public string cassetteDescription;
    private SoundObject onPressedSound;

	// Use this for initialization
	void Start () {
        if(this.transform.Find("CassetteSound")) {
            onPressedSound = this.transform.Find("CassetteSound").GetComponent<SoundObject>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnUnSelected() {
        base.OnUnSelected();
        if(onPressedSound) {
            onPressedSound.Stop();
        }
    }

    public override void OnPressed() {
		if (IsVisible ()) {
			if (onPressedSound) {
				SoundUtils.SetSoundVolumeToSavedValueForGameObject (SoundType.FX, onPressedSound.gameObject);
				if (onPressedSound.IsPlaying ()) {
					onPressedSound.Stop ();
				} else {
					onPressedSound.Play (true);
				}
			}
		}
        base.OnPressed();
    }

    public SpriteRenderer GetSpriteRenderer() {
        return this.transform.Find("Cassette").GetComponent<SpriteRenderer>();
    }

    public string getName() {
        return GetComponent<TextMesh>().text;
    }

}
