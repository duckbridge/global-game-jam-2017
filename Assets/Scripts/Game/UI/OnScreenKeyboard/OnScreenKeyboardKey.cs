using UnityEngine;
using System.Collections;

public class OnScreenKeyboardKey : MenuButtonWithColors {

    private string key;
    private SoundObject onPressedSound;

	// Use this for initialization
    public override void Awake() {
        base.Awake();
        onPressedSound = GetComponentInChildren<SoundObject>();
        key = GetComponent<TextMesh>().text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string GetKey() {
        return key;
    }

    public void ToggleCaps(bool showCaps) {
        key = showCaps ? key.ToUpper() : key.ToLower();
        GetComponent<TextMesh>().text = key;
    }

    public SoundObject GetOnPressedSound() {
        return this.onPressedSound;
    }
}
