using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DanceMinigame : InteractionObject {

    public Vector3 cassetteThrowForce = new Vector3(165f, 0f, -165f);
	public ParticleSystem boomboxParticles;
	public int minigameId = 1;

	public BeatListener beatListener;
	public List<BeatObject> beatObjects;

	public TextMesh pointDisplay;
	public AnimationManager2D dancerAnimationManager;

	public TextBoxManager onWonTextBox, onLostTextBox, beforeGameTextBox, onAfterWinTextBox;

	public CassettePickup cassettePickup;
	public int pointsRequired = 10;
	public SoundObjectWithInfo trackToPlay;

	private bool playerHasWon;

	private Player player;
	private int points = 0;

	private enum GameState { None, DuringGame, OnWon, OnLost, AfterGameWin }
	private GameState gameState;

	private bool isBusy = false;

	public override void Start () {
		base.Start ();

		pointDisplay.text = "";

		if(SceneUtils.FindObject<PlayerSaveComponent>().HasFinishedMiniGame(minigameId)) {
			playerHasWon = true;
		}
	}

	public void OnPlayerEnteredAndDoesntHaveCassette(Player player) {

		SoundUtils.SetSoundVolumeToSavedValue(SoundType.BG);

		trackToPlay.GetSound().PlayScheduled(AudioSettings.dspTime);
		beatListener.Initialize(trackToPlay.GetSound().clip.name, trackToPlay.onBeatRange);
		beatObjects.ForEach(bo => beatListener.AddEventListener(bo.gameObject));
		player.GetComponent<PlayerDanceComponent> ().SetBeatListener (beatListener);

		this.player = player;

		beatListener.AddEventListener (boomboxParticles.gameObject);
		boomboxParticles.Play ();

		dancerAnimationManager.Initialize ();
		dancerAnimationManager.PlayAnimationByName ("Dancing", true);

		ResetPoints ();
		UpdatePointDisplay ();
	}

	public void OnPlayerEnteredAndHasCassette() {
		SetPlayerHasWon ();
		boomboxParticles.Stop ();
		dancerAnimationManager.Initialize ();
		dancerAnimationManager.PlayAnimationByName ("NotDancing", true);
	}

	public override void OnInteract (Player player) {
		if(!isBusy && canInteract) {
			isBusy = true;

			base.OnInteract (player);
			this.player = player;

			player.OnTalking ();
			player.GetComponent<BodyControl>().StopMoving();
			player.SetInDanceMinigame (false);

			if(playerHasWon) {
				ShowTextBox(onAfterWinTextBox, GameState.AfterGameWin);
			} else {
				ShowTextBox(beforeGameTextBox, GameState.DuringGame);
			}
		}
	}

	public void OnDanceOnBeat(Player player) {
		if (!playerHasWon) {
			points++;
			UpdatePointDisplay ();
			if (points >= pointsRequired) {
				OnPlayerWon ();
			}
		}
	}


	public void OnDanceOffBeat(Player player) {
		if (!playerHasWon) {
			ResetPoints ();
			UpdatePointDisplay ();
		}
	}
		
	private void OnPlayerWon() {

		player.OnTalking ();

		SetPlayerHasWon ();
		pointDisplay.text = "";
		SceneUtils.FindObject<PlayerSaveComponent>().OnMiniGameFinished(minigameId);

		ShowTextBox(onWonTextBox, GameState.OnWon);

		dancerAnimationManager.Initialize ();
		dancerAnimationManager.PlayAnimationByName ("NotDancing", true);
	}

	public void OnTextBoxDoneAndHidden() {
		switch(gameState) {
			case GameState.OnWon:
				cassettePickup.gameObject.SetActive (true);
				cassettePickup.transform.parent = this.transform.parent;
				cassettePickup.GetComponent<Collider> ().enabled = true;

				cassettePickup.GetComponent<Rigidbody> ().velocity = cassetteThrowForce;
				cassettePickup.transform.Find ("Casette").GetComponent<Animation> ().Play ();

				boomboxParticles.Stop ();	

				player.OnTalkingDone ();
				player.SetInDanceMinigame (true);

				DispatchMessage ("MuteMusic", null);
				trackToPlay.Stop ();
				Invoke ("ResetBusy", .5f);
					
			break;

			default:
				player.OnTalkingDone ();
				player.SetInDanceMinigame (true);

				isBusy = false;
			break;
		}

		gameState = GameState.None;
	}

	private void ResetBusy() {
		isBusy = false;
	}

	public void ResetPoints() {
		points = 0;
	}

	private void ShowTextBox(TextBoxManager textBoxManager, GameState newGameState) {
		gameState = newGameState;

		isBusy = true;

		textBoxManager.AddEventListener(this.gameObject);
		textBoxManager.ResetShowAndActivate();
	}

	private void UpdatePointDisplay() {
		pointDisplay.text = points + " / " + pointsRequired;
	}

	public void SetPlayerHasWon() {
		this.playerHasWon = true;
	}

	public override void EnableInteraction(Player player) {
		canInteract = true;
		ShowInput(player);
		player.GetComponent<PlayerInputComponent>().AddEventListener(this.gameObject);
	}

	public override void DisableInteraction(Player player) {
		canInteract = false;
		HideInput(player);
		player.GetComponent<PlayerInputComponent>().RemoveEventListener(this.gameObject);
	}
}
