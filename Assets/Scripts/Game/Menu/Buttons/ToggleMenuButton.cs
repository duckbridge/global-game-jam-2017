using UnityEngine;
using System.Collections;

public class ToggleMenuButton : MenuButtonWithColors {

    public GameObject toggleOnGo, toggleOffGO;
    protected SettingsSaveComponent settingsSaveComponent;
    protected bool isToggledOn = false;

    public override void Awake () {
        base.Awake ();

        if(!settingsSaveComponent) {
            settingsSaveComponent = SceneUtils.FindObject<SettingsSaveComponent>();
        }

        LoadSetting();
    }

    public override void OnPressed() {
        DoToggle();
    }

    public void DoToggle() {
        if(!settingsSaveComponent) {
            settingsSaveComponent = SceneUtils.FindObject<SettingsSaveComponent>();
        }

        isToggledOn = !isToggledOn;
        
        SaveSetting();    
    }

    protected virtual void LoadSetting() {

    }

    protected virtual void SaveSetting() {

    }

    protected virtual void UpdateOnToggle(bool showToggledOn) {
        toggleOnGo.SetActive(showToggledOn);
        toggleOffGO.SetActive(!showToggledOn);
    }

	public override void SetOriginalColor (Color color) {
		base.SetOriginalColor (color);
		if (!this.isSelected) {
			SetColorForToggle (color);
		}
	}

	public override void OnSelected () {
		base.OnSelected ();
		SetColorForToggle (selectColor);
	}

	public override void OnUnSelected () {
		base.OnUnSelected ();
		SetColorForToggle (originalColor);
	}

	private void SetColorForToggle(Color color) {
		if (toggleOnGo.GetComponent<SpriteRenderer> () && toggleOffGO.GetComponent<SpriteRenderer> ()) {
			toggleOnGo.GetComponent<SpriteRenderer> ().color = color;
			toggleOffGO.GetComponent<SpriteRenderer> ().color = color;
		}
	}

}
