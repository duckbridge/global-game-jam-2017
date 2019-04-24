using UnityEngine;
using System.Collections;

public class MovingCloud : MonoBehaviour {
	
	public float minimumMoveSpeed, maximumMoveSpeed;

	public Transform target;

	public bool repeatsOnDone = false;

	private float moveSpeed;
	private Vector3 originalLocalPosition;
	private float originalColorAlpha;
	private SpriteRenderer cloudSprite;

	// Use this for initialization
	void Start () {
		originalLocalPosition = this.transform.localPosition;

		cloudSprite = GetComponent<SpriteRenderer>();

		originalColorAlpha = cloudSprite.color.a;
		ChangeOnlyAlphaTo(0f);

		MoveToTarget();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnMovingDone() {
		if(repeatsOnDone) {
			MoveToTarget();
		}
	}

	private void MoveToTarget() {
		this.transform.localPosition = originalLocalPosition;

		moveSpeed = Random.Range (minimumMoveSpeed, maximumMoveSpeed);

		ChangeOnlyAlphaTo(0f);

      	iTween.ValueTo(this.gameObject, new ITweenBuilder().SetFromAndTo(0f, originalColorAlpha).SetTime(.5f).SetOnUpdate("OnColorUpdated").Build());
		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder().SetPosition(target.localPosition).SetLocal().SetSpeed(moveSpeed).SetEaseType (iTween.EaseType.linear).SetOnComplete("OnMovingDone").SetOnCompleteTarget(this.gameObject).Build());
	}

	private void OnColorUpdated(float newAlphaValue) {
		ChangeOnlyAlphaTo(newAlphaValue);
	}
	
	private void ChangeOnlyAlphaTo(float newAlphaColor) {
		cloudSprite.color = new Color(cloudSprite.color.r, cloudSprite.color.g, cloudSprite.color.b, newAlphaColor);
	}
}
