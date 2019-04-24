using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicMakeMinigame : GameRoomMinigame {

	public MusicMakeBoard musicMakeBoard;
	private TextMesh pointDisplay;

	// Use this for initialization
	void Start () {
		pointDisplay = this.transform.Find("PointDisplay").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void StartMinigame () {
		base.StartMinigame ();

		musicMakeBoard.gameObject.SetActive(true);
		musicMakeBoard.AddEventListener(this.gameObject);

		Invoke("DoStartMinigame", 1f);
	}

	private void DoStartMinigame() {
		musicMakeBoard.StartBoard(playerInputActions, pointDisplay);
	}

	public void OnMusicBoardDone() {
		if(musicMakeBoard.HasEnoughScore()) {
			OnWonMinigame(player);
		}

		this.musicMakeBoard.gameObject.SetActive(false);
		StopMinigame();
	}

	protected override void OnMinigameDone () {
		musicMakeBoard.HidePointsDisplay();
		base.OnMinigameDone ();
	}
}
