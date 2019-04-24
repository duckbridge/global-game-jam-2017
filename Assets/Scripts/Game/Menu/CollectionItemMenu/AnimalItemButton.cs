using UnityEngine;
using System.Collections;

public class AnimalItemButton : CollectionItemButton {

    public string animalName;

    [TextArea(3,10)]
    public string animalDescription;
    private SoundObject onPressedSound;

	// Use this for initialization
	void Start () {
        if(this.transform.Find("AnimalSound")) {
            onPressedSound = this.transform.Find("AnimalSound").GetComponent<SoundObject>();
        }
	}

	// Update is called once per frame
	void Update () {

	}

    public override void OnSelected() {
        this.transform.Find("Animal").GetComponent<SimpleTimeScaleIndependentAnimation2D>().Play(true);
    }


    public override void OnUnSelected() {
        this.transform.Find("Animal").GetComponent<SimpleTimeScaleIndependentAnimation2D>().Stop();
    }

    public override void SetVisible(bool isVisible) {
        this.isVisible = isVisible;
        if(isVisible) {
          this.transform.Find("Animal").GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public override void OnPressed() {
        if(IsVisible()) {
            if(onPressedSound) {
                SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, onPressedSound.gameObject);
                onPressedSound.Play(true);
            }
            base.OnPressed();
        }
    }

    public SpriteRenderer GetSpriteRenderer() {
        return this.transform.Find("Animal").GetComponent<SpriteRenderer>();
    }

}
