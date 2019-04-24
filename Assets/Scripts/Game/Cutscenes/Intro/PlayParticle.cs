using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class PlayParticle : CutSceneComponent {

        public float deactivateTimeout = 0f;
        public ParticleSystem[] particleSystemsToPlay;

        public override void OnActivated () {
            foreach(ParticleSystem particleSystem in particleSystemsToPlay) {
                
                particleSystem.Clear();
                particleSystem.Stop();
                
                particleSystem.Play();
            }

            if(deactivateTimeout > 0) {
                Invoke("DeActivate", deactivateTimeout );
            } else {
                DeActivate();
            }
        }
    }
}

