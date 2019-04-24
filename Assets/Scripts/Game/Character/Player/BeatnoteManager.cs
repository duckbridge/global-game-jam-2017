using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatnoteManager : MonoBehaviour {

	public int amountOfNotes = 4;
	public float firstBeatSpawnTime = 1f;
	public float beatSpeed = 5f;

	public float minimumRotationForBeat = 260f;
	public float maximumRotationForBeat = 280f;

	public Beatnote[] beatnotePrefabs;

	private List<Beatnote> inactiveBeatnotes = new List<Beatnote>();
	private List<Beatnote> activeBeatnotes = new List<Beatnote>();

	private int amountOfNotesSpawned = 0;

	// Use this for initialization
	void Start () {
		Invoke ("StartSpawningBeatNotes", firstBeatSpawnTime);
	}

	public void MakeAllBeatNotesInactive() {
		for(int i = 0 ; i < activeBeatnotes.Count ; i++) {

			activeBeatnotes[i].StopRotating();
			activeBeatnotes[i].gameObject.SetActive(false);

			inactiveBeatnotes.Add (activeBeatnotes[i]);
			activeBeatnotes.RemoveAt(i);

			i--;
		}
	}

	public void StartSpawningBeatNotes() {

		amountOfNotesSpawned = 0;

		SpawnBeatNote();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnBeatNote() {
		if(inactiveBeatnotes.Count > 0) {

			inactiveBeatnotes[0].gameObject.SetActive(true);
			inactiveBeatnotes[0].StartRotating();
			activeBeatnotes.Add (inactiveBeatnotes[0]);

			inactiveBeatnotes.RemoveAt(0);
		
		} else {

			int randomIndex = Random.Range(0, beatnotePrefabs.Length);
			Beatnote beatballPrefab = beatnotePrefabs[randomIndex];
	
			Beatnote beatnote = (Beatnote) GameObject.Instantiate(beatballPrefab, this.transform.position, Quaternion.identity);
		
			beatnote.Initialize(beatSpeed, minimumRotationForBeat, maximumRotationForBeat);

			beatnote.transform.parent = this.transform;
			beatnote.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			beatnote.AddEventListener(this.gameObject);

			beatnote.StartRotating();
		
			activeBeatnotes.Add (beatnote);
		}

		amountOfNotesSpawned++;
		if(amountOfNotesSpawned < amountOfNotes) {
			Invoke ("SpawnBeatNote", 1.8f / beatSpeed);
		}
	}

	public void OnBallDone(Beatnote beatball) {
		beatball.ResetBall();

		activeBeatnotes.Remove (beatball);
		inactiveBeatnotes.Add (beatball);

		beatball.gameObject.SetActive(false);
	}

	public bool CanDoSoundBlast() {
		for(int i = 0 ; i < activeBeatnotes.Count ; i++) {
			if(activeBeatnotes[i].CanDoSoundBlast()) {
				return true;
			}
		}

		return false;
	}
}
