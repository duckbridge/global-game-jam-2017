using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBox: DispatchBehaviour {

	public bool showLetterByLetter = true;

	public List<TextMesh> textMeshesSetInEditor;

	protected List<TextContainer> textContainers = new List<TextContainer>();

	private TextMesh[] textMeshesUsed;
	private List<string> originalTextMeshText;

	public float textTimeout;
	private string totalString;

	protected int currentTextMeshIndex = 0;
	
	protected AnimationManager2D animationManagerToUseForTalking;
	protected string animationNameOnTalk;

	// Use this for initialization
	void Awake () {}

	public void Update() {

	}

	public void Start() {

	}

	public void Initialize() {

		textContainers = new List<TextContainer>();
		originalTextMeshText = new List<string>();

		if(textMeshesSetInEditor.Count == 0) {
			textMeshesUsed = this.GetComponentsInChildren<TextMesh>();
		} else {
			textMeshesUsed = textMeshesSetInEditor.ToArray();
		}
		
		for(int i = 0 ; i < textMeshesUsed.Length; i++) {
			originalTextMeshText.Add (textMeshesUsed[i].text);
			TextContainer textContainer = new TextContainer(textMeshesUsed[i], showLetterByLetter);
			textContainers.Add(textContainer);
		}
	}

	public void SetAnimationOnTalking(string animationNameOnTalk, AnimationManager2D animationManagerToUseForTalking) {
		this.animationManagerToUseForTalking = animationManagerToUseForTalking;
		this.animationNameOnTalk = animationNameOnTalk;
	}

	public virtual void OnStart() {
		ShowNextWord();
	}

	public void FinishTextBox() {
		CancelInvoke("ShowNextWord");

		for(int i = 0; i < textMeshesUsed.Length ;i++) {
			textMeshesUsed[i].text = originalTextMeshText[i];
		}
		DispatchMessage("OnTextDone", this);
	}

	protected virtual void ShowNextWord() {
		CancelInvoke("ShowNextWord");

		TextContainer currentTextContainer = textContainers[currentTextMeshIndex];

		if(currentTextContainer.CanDisplayNextWord()) {

			PlayTalkingAnimation();

			currentTextContainer.AppendNextWord();
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

    protected virtual void OnTextBoxDone() {
    
    }

	public void ResetTextBox() {
		Initialize();
		currentTextMeshIndex = 0;
	}

    protected virtual void PlayTalkingAnimation() {
        if(animationManagerToUseForTalking) {
            animationManagerToUseForTalking.ResumeAnimationByNameAndResetIfDone(animationNameOnTalk);
        }
    }
}
