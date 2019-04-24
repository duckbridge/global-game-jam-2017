using UnityEngine;
using System.Collections;

public class Fading2D : DispatchBehaviour {

	private bool isFading = false;
	public SpriteRenderer targetSprite;
	public TextMesh targetTextMesh;

	private float fadeTime;
	private Color targetColor;

	private Vector4 colorIncrement;
	private FadeType fadeType;

	public bool canBePaused = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isFading) {
			if(targetSprite) {

				if(targetSprite.color != targetColor) {
					targetSprite.color += new Color(colorIncrement.x, colorIncrement.y, colorIncrement.z, colorIncrement.w);
				} else {
					OnFadingDone();
				}

			} else if(targetTextMesh) {
				if(targetTextMesh.color != targetColor) {
					targetTextMesh.color += new Color(colorIncrement.x, colorIncrement.y, colorIncrement.z, colorIncrement.w);
				} else {
					OnFadingDone();
				}

			}
		}
	}

	public void Initialize() {
		if(!targetSprite)
			targetSprite = GetComponent<SpriteRenderer>();

		if(!targetTextMesh)
			targetTextMesh = GetComponent<TextMesh>();
	}

	private void OnFadingDone() {
		if(isFading) {
			isFading = false;
			Logger.Log ("OnFadingDone");
			DispatchMessage("OnFadingDone", fadeType);
		}
	}

	public void SetTarget(SpriteRenderer targetSprite) {
		this.targetSprite = targetSprite;
	}

	public void SetTarget(TextMesh targetTextMesh) {
		this.targetTextMesh = targetTextMesh;
	}
	
	public void FadeInto(Color newColor, float time = 30f, FadeType fadeType = FadeType.DEFAULT) {

		Initialize();

		if(targetSprite && !targetSprite.enabled) targetSprite.enabled = true;

		targetColor = newColor;
		fadeTime = time;
		isFading = true;
		this.fadeType = fadeType;

		Vector4 colorDifference = Vector4.zero;
		if(targetSprite) {
			colorDifference = (targetColor - targetSprite.color);
		} else if(targetTextMesh) {
			colorDifference = (targetColor - targetTextMesh.color);
		}

		colorIncrement = colorDifference / time;
	}

	public override void OnPauseGame() {
		if(canBePaused)
			this.enabled = false;
	}
	
	public override void OnResumeGame() {
		if(canBePaused)
			this.enabled = true;
	}
}
