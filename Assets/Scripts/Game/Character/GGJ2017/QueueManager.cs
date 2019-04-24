using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour {

	public CurrencyModifyParticle currencyModifyParticle;

	public QueuePosition exitPosition;
	public Transform spawnPosition;

	public List<AnimalWithInputPattern> spawnableAnimals;
	private List<AnimalWithInputPattern> animalsInQueue = new List<AnimalWithInputPattern>();
	private List<AnimalWithInputPattern> animalsWaitingForQueue = new List<AnimalWithInputPattern>();

	public SoundObject onSpawnSound;

	public List<QueuePosition> queue;
	private FirstSon firstSon;

	// Use this for initialization
	void Start () {
		firstSon = SceneUtils.FindObject<FirstSon> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnRandomAnimal() {
		int randomSpawnIndex = Random.Range (0, spawnableAnimals.Count);
		AnimalWithInputPattern spawnedAnimal = (AnimalWithInputPattern) GameObject.Instantiate<AnimalWithInputPattern> (spawnableAnimals[randomSpawnIndex], spawnPosition.position, Quaternion.identity);
		spawnedAnimal.AddEventListener (this.gameObject);

		onSpawnSound.Play (true);
		QueuePosition emptyQueuePosition = FindFirstEmptyQueuePosition ();

		if (emptyQueuePosition) {
			animalsInQueue.Add (spawnedAnimal);
			spawnedAnimal.MoveTo (emptyQueuePosition);
		} else {
			Logger.Log ("queue is full...");
			animalsWaitingForQueue.Add (spawnedAnimal);
		}
	}

	public void OnFirstCustomerHelped(AnimalWithInputPattern animalWithInputPattern) {

		UpdateCurrency (animalWithInputPattern);

		//animalWithInputPattern.MoveTo (exitPosition);
		firstSon.DoSpit(animalWithInputPattern);
		animalWithInputPattern.AddEventListener (this.gameObject);
		animalsInQueue.Remove (animalWithInputPattern);

		if (animalsWaitingForQueue.Count > 0) {
			animalsInQueue.RemoveAt(animalsWaitingForQueue.Count - 1);
			animalsWaitingForQueue.Add (animalWithInputPattern);
			Logger.Log ("animal left waiting Queue");
		}


		//MoveAllAnimals ();
	}

	public void UpdateCurrency(AnimalWithInputPattern animalWithInputPattern) {
		CurrencyModifyParticle particle = GameObject.Instantiate (currencyModifyParticle, animalWithInputPattern.GetHead ().position, Quaternion.identity);

		int currencyModified = 0;

		if (animalWithInputPattern.IsHappy ()) {
			currencyModified = animalWithInputPattern.currencyGainOnCorrect;
		} else {
			currencyModified = animalWithInputPattern.currencyLossOnCorrect;
		}

		SceneUtils.FindObject<CurrencyContainer> ().IncreaseCurrency (currencyModified);
		particle.Show (currencyModified);
	}


	private void MoveAllAnimals() {
		foreach (AnimalWithInputPattern animal in animalsInQueue) {
			if (animal.GetCurrentQueuePosition () != null) {
				QueuePosition newQueuePos = FindQueuePositionByNumber (animal.GetCurrentQueuePosition ().position - 1);
				if (newQueuePos) {
					animal.MoveTo (newQueuePos);
				} else {
					Logger.Log ("no queue pos found???");
				}
			}
		}
	}

	public bool QueueHasSpace() {
		return FindFirstEmptyQueuePosition() != null;
	}

	private QueuePosition FindQueuePositionByNumber(int position) {
		for (int i = 0; i < queue.Count; i++) {
			if (queue [i].position == position) {
				return queue [i];
			}
		}

		return null;
	}

	private QueuePosition FindFirstEmptyQueuePosition() {
		for (int i = 0; i < queue.Count; i++) {
			if (!queue [i].IsOccupied ()) {
				return queue [i];
			}
		}

		return null;
	}
}
