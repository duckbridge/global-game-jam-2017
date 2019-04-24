using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class ShowBossCassette : CutSceneComponent {

        public SoundObject showSound;

        public float showTime = 1f;
        public GameObject cassette;

        public float moveSpeed = 2f;

        public Transform showTarget;
        public Transform hideTarget;

        public iTween.EaseType showEaseType, hideEaseType;

        public override void OnActivated () {

            showSound.Play();

            iTween.MoveTo(cassette, new ITweenBuilder()
                .SetPosition(showTarget.position)
                .SetSpeed(moveSpeed)
                .SetEaseType(showEaseType)
                .SetOnCompleteTarget(this.gameObject)
                .SetOnComplete("OnShown").Build());
        }

        public void OnShown() {
            Invoke("Hide", showTime);
        }
    
        public void Hide() {
             iTween.MoveTo(cassette, new ITweenBuilder()
                .SetPosition(hideTarget.position)
                .SetSpeed(moveSpeed)
                .SetEaseType(hideEaseType)
                .SetOnCompleteTarget(this.gameObject)
                .SetOnComplete("OnHidden").Build());
        }

        public void OnHidden() {
           DeActivate();
        }
    }
}
