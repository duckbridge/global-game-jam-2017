using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class WaitForOnScreenKeyboardInput : CutSceneComponent {

        public bool saveInput = true;
        public string inputSaveName = "KiddoOneName";

        public OnScreenKeyboard onScreenKeyboard;

        public override void OnActivated () {
           onScreenKeyboard.gameObject.SetActive(true);
           SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, onScreenKeyboard.gameObject);
           onScreenKeyboard.EnableKeyboard();

           onScreenKeyboard.AddEventListener(this.gameObject);
        }

        public void OnSubmitPressed(string inputText) {
           if(saveInput) {
                PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
                if(playerSaveComponent) {            
                    playerSaveComponent.AddKeyboardInputToSave(inputSaveName, inputText);
                }
           }

           onScreenKeyboard.DisableKeyboard();
           onScreenKeyboard.gameObject.SetActive(false);
           
           DeActivate ();
        }
    }
}