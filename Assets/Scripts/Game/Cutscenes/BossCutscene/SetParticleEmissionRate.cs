using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class SetParticleEmissionRate : CutSceneComponent {

        public float newEmissionRate = 50f;
        public ParticleSystem particleSystem;

        public override void OnActivated () {

            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.rate = new ParticleSystem.MinMaxCurve(newEmissionRate);

            DeActivate();
        }
    }
}
