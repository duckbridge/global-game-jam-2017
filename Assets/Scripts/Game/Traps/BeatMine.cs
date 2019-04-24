using UnityEngine;
using System.Collections;

public class BeatMine : ToggleBeatObject {

	public float onBeatMoveTime = .5f;
	public Transform[] randomPositions;
	private Transform chosenPosition;

	public void Awake() {
		int positionIndex = Random.Range (0, randomPositions.Length);
		chosenPosition = randomPositions[positionIndex];
	}

	protected override void OnFirstStateEntered () {
		iTween.MoveTo(this.gameObject, new ITweenBuilder().SetLocal().SetPosition(Vector3.zero).SetTime(onBeatMoveTime).SetEaseType(iTween.EaseType.linear).Build());
	}

	protected override void OnSecondStateEntered () {
		iTween.MoveTo(this.gameObject, new ITweenBuilder().SetLocal().SetPosition(chosenPosition.localPosition).SetTime(onBeatMoveTime).SetEaseType(iTween.EaseType.linear).Build());
	}
}
