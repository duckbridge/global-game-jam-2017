using UnityEngine;
using System.Collections;

public class VolumeMenuButton : MenuButtonWithColors {

   	public TextMesh textOutput;
	public SoundType soundType;
	private float soundVolume;
    
    private SettingsSaveComponent settingsSaveComponent;

    public override void Start() {
        base.Start();
        
         SetVolumeSoundDelayed();
    }

    private void SetVolumeSoundDelayed() {
        soundVolume = SoundUtils.GetVolume(soundType);
        textOutput.text = System.Convert.ToInt32(soundVolume * 100)+"";
    }

	public void IncrementVolumeBy(float incrementAmount) {
		if(soundVolume + incrementAmount <= 0) {
			soundVolume = 0;
		} else if(soundVolume + incrementAmount >= 1) {
			soundVolume = 1f;
		} else {
			soundVolume += incrementAmount;
		}

        FindSettingsSaveComponent();
    
        if(soundType == SoundType.BG) {
            settingsSaveComponent.SetBgVolume(soundVolume);
            SoundUtils.SetSoundVolumeToSavedValue(SoundType.BG);
        } else {
            settingsSaveComponent.SetFxVolume(soundVolume);
            SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);
        }

		settingsSaveComponent.SaveSettingsData();
		textOutput.text = System.Convert.ToInt32(soundVolume * 100)+"";
	
	}

    private void FindSettingsSaveComponent() {
        if(!settingsSaveComponent) {
            settingsSaveComponent = SceneUtils.FindObject<SettingsSaveComponent>();    
        }
    }

	public override void SetOriginalColor (Color color) {
		base.SetOriginalColor (color);
		if (!this.isSelected) {
			textOutput.color = color;
		}
	}

	public override void OnSelected () {
		base.OnSelected ();
		textOutput.color = selectColor;
	}

	public override void OnUnSelected () {
		base.OnUnSelected ();
		textOutput.color = originalColor;
	}

}
