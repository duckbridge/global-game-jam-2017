
using UnityEngine;
using System.Collections;

public class WeaponOnBack : MonoBehaviour {

    public enum DBMoods { Neutral, Happy, Angry, Sad }

	public AnimationManager2D animationManager;
	public float offsetY;

	private float originalRotation;

	private Animation2D leftAnimation, rightAnimation, upAnimation, downAnimation;

	private Direction currentDirection = Direction.LEFT;
    private string moodAnimationName = "Neutral";

	private void Initialize() {

		animationManager.Initialize();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetOriginalRotation(float originalRotation) {
		this.originalRotation = originalRotation;
	}

	public float GetOriginalRotation() {
		return originalRotation;
	}

	public void SwapSpriteForDirection(Direction direction) {
		Initialize ();
		HideAll();

		switch(direction) {

			case Direction.UP:
				animationManager.PlayAnimationByName("Up-" + moodAnimationName);	
			break;

			case Direction.DOWN:
				animationManager.PlayAnimationByName("Down");	
			break;

			case Direction.RIGHT:
				animationManager.PlayAnimationByName("Right");	
			break;

			case Direction.LEFT:
				animationManager.PlayAnimationByName("Left");	
			break;

		}

		this.currentDirection = direction;
	}

    public void SetMood(DBMoods dbMood) {
        moodAnimationName = dbMood.ToString();
        SwapSpriteForDirection(currentDirection);
    }

	private void HideAll() {
		animationManager.StopHideAllAnimations();
	}

	public Direction GetDirection() {
		return currentDirection;
	}

	public AnimationManager2D GetAnimationManager() {
		return animationManager;
	}
    
    public string GetTalkAnimationName() {
        return "Talking-" + moodAnimationName;
    }
}
