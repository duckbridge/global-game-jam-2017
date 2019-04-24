using UnityEngine;
using System.Collections;
using InControl;

public class GameRoomMinigame : InteractionObject {

	public TextBoxManager textBoxBeforePlay, textBoxOnWin, textBoxOnLose, textBoxOnPlayAfterWin, textBoxOngrabGameBeforeWin;

	public GamePickup gamePickup;

	protected bool isLocked = false;
	protected bool hasWonMinigame = false;
	protected bool hasStarted = false;
	protected PlayerInputActions playerInputActions;
	protected Player player;

	protected bool isBusy = false;
	protected TextBoxManager currentTextBoxManager;

	// Use this for initialization
	public virtual void Start () {
		Invoke ("TryToLockMinigame", 3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInteract (Player player) {
		if(!isBusy) {
			isBusy = true;

			this.player = player;
			player.GetComponent<PlayerInputComponent>().enabled = false;

			base.OnInteract (player);

			textBoxOnWin.RemoveEventListener(this.gameObject);
			textBoxBeforePlay.RemoveEventListener(this.gameObject);
			textBoxOnPlayAfterWin.RemoveEventListener(this.gameObject);

			if(isLocked) {
				textBoxOnPlayAfterWin.AddEventListener(this.gameObject);
				textBoxOnPlayAfterWin.ResetShowAndActivate();
				currentTextBoxManager = textBoxOnPlayAfterWin;
			} else { 
				textBoxBeforePlay.AddEventListener(this.gameObject);
				textBoxBeforePlay.ResetShowAndActivate();
				currentTextBoxManager = textBoxBeforePlay;
			}
		}
	}

	public void OnTextDone() {
		if(currentTextBoxManager == textBoxBeforePlay || currentTextBoxManager == textBoxOnPlayAfterWin) {
			StartMinigame();
		}

		if(currentTextBoxManager == textBoxOnWin) {
			OnMinigameDone();
		}

		if(currentTextBoxManager == textBoxOnLose) {
			OnMinigameDone();
		}

        if(currentTextBoxManager == textBoxOngrabGameBeforeWin) {
            player.GetComponent<PlayerInputComponent>().enabled = true;
        }
	}

	protected virtual void OnMinigameDone() {
		player.GetCharacterControl().characterState = CharacterControl.CharacterState.Idle;
		player.GetComponent<PlayerInputComponent>().enabled = true;
		player.OnTalkingDone ();
		isBusy = false;
	}

	public virtual void OnRoomEntered() {
		playerInputActions = PlayerInputHelper.LoadData();
	}

	public virtual void OnRoomExitted() {
		
	}

	public virtual void StartMinigame() {
		player.GetComponent<PlayerInputComponent>().enabled = false;

		hasWonMinigame = false;
		hasStarted = true;
	}

	public virtual void StopMinigame() {
		hasStarted = false;

		ResetMinigame();

		if(hasWonMinigame) {

			textBoxOnWin.AddEventListener(this.gameObject);
			textBoxOnWin.ResetShowAndActivate();
			currentTextBoxManager = textBoxOnWin;
		} else {
			textBoxOnLose.AddEventListener(this.gameObject);
			textBoxOnLose.ResetShowAndActivate();
			currentTextBoxManager  = textBoxOnLose;
		}
	}

	public void OnWonMinigame(Player player) {
		hasWonMinigame = true;
		if(!isLocked) {
			if (gamePickup) {
				gamePickup.EnablePickingUp ();
			}
			TryToLockMinigame();
		}
	}
		
	protected virtual void ResetMinigame() {
	
	}

	protected virtual void DoLock() {
		isLocked = true;
		if (gamePickup) {
			gamePickup.gameObject.SetActive (false);
		}
	}

    public virtual void OnInteractListener(Player player) {

        this.player = player;

        player.GetComponent<PlayerInputComponent>().enabled = false;

        currentTextBoxManager = textBoxOngrabGameBeforeWin;
        textBoxOngrabGameBeforeWin.AddEventListener(this.gameObject);
        textBoxOngrabGameBeforeWin.ResetShowAndActivate();
    }

	private void TryToLockMinigame() {
		GameInfo foundGameInfo = 
			SceneUtils.FindObject<CollectionManager> ().GetAllGameInfo ().Find (gameInfo => gameInfo.name.Equals (gamePickup.minigameName));
		if(foundGameInfo != null) {
			DoLock();
		}
	}
}
