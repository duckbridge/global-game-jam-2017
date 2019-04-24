using UnityEngine;
using System.Collections;

public class CameraShakeToggleButton : ToggleMenuButton {

    bool saveOnToggle = true;
        
    protected override void LoadSetting() {
        
        if(settingsSaveComponent.HasCameraShakeEnabled()) {
            isToggledOn = true;
            UpdateOnToggle(true);
        } else {
            isToggledOn = false;
            UpdateOnToggle(false);
        }
    }

    protected override void SaveSetting() {

        settingsSaveComponent.ToggleCameraShake(isToggledOn);
        UpdateOnToggle(isToggledOn);
        
        if(saveOnToggle) {
            settingsSaveComponent.SaveSettingsData();
        }

        CameraShaker cameraShaker = SceneUtils.FindObject<CameraShaker>();
        if(cameraShaker) {
            cameraShaker.canShakeCamera = isToggledOn;
            cameraShaker.ShakeCamera(new Vector3(5f, 5f, 5f), true);
        }
    }
}
