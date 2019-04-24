using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class ToggleFollowCamera : CutSceneComponent {
        
        public bool enableFollowCamera = true;

        public override void OnActivated () {

            SceneUtils.FindObject<FollowCamera2D>().enabled = enableFollowCamera;

            DeActivate();
        }
    }
}