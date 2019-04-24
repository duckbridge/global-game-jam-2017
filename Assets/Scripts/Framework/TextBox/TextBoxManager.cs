using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBoxManager : DispatchBehaviour {
	
	public AnimationManager2D animationManagerToUseForTalking;
	public string animationNameOnTalk;

	public float nextTextBaloonTimeout = .5f;
	public bool playerCanControlTextBox = true;
	public SoundObject onShowSound;

	public List<TextBox> textBoxes;
	public List<SoundObject> talkSounds;

	public bool activateOnAwake = true;
	
	private int currentTextBox = 0;
	private bool textBoxIsDone = false;
	protected bool isPaused = false;
	protected bool isBusy = false;

	protected bool isActivated = false;

	private SoundObject currentTalkSound;

	private Animation2D onShowAnimation;
	private Animation2D onHideAnimation;

	protected InControl.PlayerInputActions playerInputActions;
	protected Transform npcPictureNames;

	// Use this for initialization
	public void Awake () {
		playerInputActions = PlayerInputHelper.LoadData();
		npcPictureNames = this.transform.Find("NPCPictureName");

		if(activateOnAwake) {
			OnActivated();
		}
	}

	protected virtual void OnActivated() {
		foreach(TextBox textBox in textBoxes) {
			textBox.gameObject.SetActive(false);
			SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.gameObject);
		}

		Transform onShowAnimationsTransform = this.transform.Find("Animations/OnShowAnimation");
		Transform onHideAnimationsTransform = this.transform.Find("Animations/OnHideAnimation");

		if(onShowAnimationsTransform && onHideAnimationsTransform) {
		
			onShowAnimation = this.transform.Find("Animations/OnShowAnimation").GetComponent<Animation2D>();
			onHideAnimation = this.transform.Find("Animations/OnHideAnimation").GetComponent<Animation2D>();

			onShowAnimation.AddEventListener(this.gameObject);
			onHideAnimation.AddEventListener(this.gameObject);

			onHideAnimation.Stop ();
			onHideAnimation.Hide ();

			onShowAnimation.Show ();
			onShowAnimation.Awake ();
			onShowAnimation.Play (true);

			isBusy = true;

		} else {

			ShowNextTextBalloon();
		}

		if(onShowSound) {
			SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.gameObject);
			onShowSound.Play();
		}

		isActivated = true;
	}

	private void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "OnShowAnimation") {
			if(npcPictureNames) {
				npcPictureNames.gameObject.SetActive(true);
			}

			ShowNextTextBalloon();
			isBusy = false;
		}

		if(animation2D.name == "OnHideAnimation") {
			OnHideAnimationDone ();
			isBusy = false;
			this.gameObject.SetActive(false);
		}
	}

	protected virtual void OnHideAnimationDone() {
		DispatchMessage("OnTextBoxDoneAndHidden", null);
	}

	public virtual void Update () {
        
		if(!isPaused && !isBusy && playerCanControlTextBox && isActivated) {
			if(playerInputActions.interact.WasReleased) {

				if(!textBoxIsDone) {
					textBoxes[currentTextBox - 1].FinishTextBox();
					textBoxIsDone = true;

					return;
				}

				if(textBoxIsDone) {
					textBoxIsDone = false;
					
					ShowNextTextBalloon();	
				}
			}
		}
	}

	public void OnTextDone(TextBox textBox) {
		textBoxIsDone = true;
		if(playerCanControlTextBox) {
			//SceneUtils.FindObject<PressActionDisplay>().Show("Continue", PressActionDisplay.ActionNames.TextBox);
		} else {
			if(nextTextBaloonTimeout > 0f) {
				Invoke ("ShowNextTextBalloon", nextTextBaloonTimeout);
			}
		}

		DispatchMessage("OnTextPartDone", null);
	}

	public void OnShowNextWord() {
		if(talkSounds.Count > 0) {
			int randomTalkIndex = Random.Range(0, talkSounds.Count);
			if(currentTalkSound) {
				currentTalkSound.Stop();
			}
			currentTalkSound = talkSounds[randomTalkIndex];
			currentTalkSound.Play();
		}

		DispatchMessage("OnShowNextWord", null);
	}
	
	private void ShowNextTextBalloon() {

		if(currentTextBox < textBoxes.Count) {
			if(playerCanControlTextBox) {
				//SceneUtils.FindObject<PressActionDisplay>().Show("Skip", PressActionDisplay.ActionNames.TextBox);
			}

			DispatchMessage("OnShowNextTextBalloon", null);

			if(currentTextBox > 0) {
				textBoxes[currentTextBox - 1].RemoveEventListener(this.gameObject);
				textBoxes[currentTextBox - 1].gameObject.SetActive(false);
			}

			textBoxes[currentTextBox].AddEventListener(this.gameObject);
			textBoxes[currentTextBox].gameObject.SetActive(true);
 			textBoxes[currentTextBox].SetAnimationOnTalking(animationNameOnTalk, animationManagerToUseForTalking);

			textBoxes[currentTextBox].OnStart();

			currentTextBox++;

		} else {
			OnBeforeTextBoxManagerDone();
		}
	}

	protected virtual void OnBeforeTextBoxManagerDone() {
		if(playerCanControlTextBox) {
			//SceneUtils.FindObject<PressActionDisplay>().Hide ();
		}
		OnTextBoxManagerDone();
	}

	protected virtual void OnTextBoxManagerDone() {

		isActivated = false;

		if(onHideAnimation) {
			isBusy = true;
			textBoxes[currentTextBox - 1].RemoveEventListener(this.gameObject);
			textBoxes[currentTextBox - 1].gameObject.SetActive(false);
		}
		
		DispatchMessage("OnTextDone", null);
		
		Hide();
	}

	public void Show() {
		this.gameObject.SetActive(true);
	}

	public void Hide() {

		if(npcPictureNames) {
			npcPictureNames.gameObject.SetActive(false);
		}

		if(this.onHideAnimation) {
			onShowAnimation.Hide ();
			onShowAnimation.Stop();

			onHideAnimation.AddEventListener(this.gameObject);

			onHideAnimation.Show ();
			onHideAnimation.Play(true);

		} else {
			this.gameObject.SetActive(false);
		}
	}

	public void ResetShowAndActivate() {
		ResetTextBoxes ();
		Show();
		OnActivated();
	}

	public void ResetTextBoxes() {
		foreach(TextBox textBox in textBoxes) {
			textBox.RemoveEventListener(this.gameObject);
			textBox.ResetTextBox ();
			textBox.gameObject.SetActive(false);
		}
		currentTextBox = 0;
	}

	public override void OnPauseGame() {
		isPaused = true;
	}
	public override void OnResumeGame() {
		isPaused = false;
	}

	public bool IsBusy() {
		return isBusy;
	}

    public bool IsActivated() {
        return isActivated;
    }
}
