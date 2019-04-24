using UnityEngine;
using System.Collections;

public class VibrationBeatObject : BeatObject {

    private VibrationComponent vibrationComponent;

    public void Awake() {
        vibrationComponent = GetComponent<VibrationComponent>();
    }

    public override void OnBeatEvent() {
        vibrationComponent.Vibrate();
    }
}
