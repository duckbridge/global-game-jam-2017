using UnityEngine;
using System.Collections;

public class InkspitTextBox : TextBox {

    protected override void PlayTalkingAnimation() {}

	protected override void ShowNextWord() {
		CancelInvoke("ShowNextWord");

		TextContainer currentTextContainer = textContainers[currentTextMeshIndex];

		if(currentTextContainer.CanDisplayNextWord()) {

			if(animationManagerToUseForTalking) {
				animationManagerToUseForTalking.PlayAnimationByName(animationNameOnTalk, true);
			}

			while (currentTextContainer.CanDisplayNextWord ()) {
				currentTextContainer.AppendNextWord ();
			}

			DispatchMessage("OnShowNextWord", null);

			if(textTimeout != -1f) {
				Invoke("ShowNextWord", textTimeout);
			}

		} else {

			if(currentTextMeshIndex < textContainers.Count - 1) {
				currentTextMeshIndex++;
				currentTextContainer = textContainers[currentTextMeshIndex];
				Invoke("ShowNextWord", textTimeout);
				return;
			}

			OnTextBoxDone();
			DispatchMessage("OnTextDone", this);
			return;
		}
	}
}
