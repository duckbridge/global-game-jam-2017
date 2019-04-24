using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class EnableCutsceneManagerWithMinDistance : CutSceneComponent {

        public CutSceneManagerStartsOnMinimumDistance cutsceneManagerStartsOnMinimumDistance;

        public override void OnActivated () {

            cutsceneManagerStartsOnMinimumDistance.EnableDistanceCheck(SceneUtils.FindObject<Player>());

            DeActivate();
        }

    }
}