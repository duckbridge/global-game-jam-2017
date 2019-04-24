using UnityEngine;
using System.Collections;

public class MusicMakeMusicInput : MonoBehaviour {

	public int pointsRewardedOnCorrect = 1;

	public enum MusicInputType { BlastOne, BlastTwo, BlastThree}
	public MusicInputType musicInputType;

	private SoundObject soundEffect;
	private bool canBeUsed = true;

	private int updatesRequired = 0;
	private float beatSpeed = 0f;
	private SpriteRenderer spriteToUse;

	// Use this for initialization
	void Start () {
		soundEffect = this.transform.Find("Sounds/OnCorrectSound").GetComponent<SoundObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.localPosition -= new Vector3(beatSpeed, 0f, 0f);
	}

	public void PlayOnCorrectSound() {
		soundEffect.PlayIndependent(true);
	}

	public void DisableUsage() {
		canBeUsed = false;
	}

	public bool CanBeUsed() {
		return canBeUsed;
	}

	public void Initialize(int ms, float beatSpeed, Transform beatContainer) {
		this.transform.parent = beatContainer;
		this.beatSpeed = beatSpeed;

		this.musicInputType = musicInputType;

		updatesRequired = (int) (ms / (Time.fixedDeltaTime * 1000));

		this.transform.localPosition = new Vector3(updatesRequired * beatSpeed, this.transform.localPosition.y, 0f);

		spriteToUse = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
		this.transform.localRotation = Quaternion.identity;
		spriteToUse.enabled = true;

		SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.gameObject);
	}
}
