using UnityEngine;
using System.Collections;

public class PlayerFishingComponent : MonoBehaviour {

	private Player player;
	private PlayerInputComponent playerInputComponent;

	private bool canFish = false;
	private bool isFishing = false;
	private bool canReelIn = false;

	private FishCollider fishColliderToUse;
	private FishDobber dobberToUse;
	private SwimmingFish fishToReelIn;

	private string basePrefix = "Right";

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		playerInputComponent = GetComponent<PlayerInputComponent>();
	}
	
	// Update is called once per frame
	public void Update() {
	
		if(isFishing) {

            float moveX = playerInputComponent.GetInputActions().moveHorizontally.Value;
            float moveZ = playerInputComponent.GetInputActions().moveVertically.Value;

            dobberToUse.DoMove(moveX, moveZ);
            PlayCorrectAnimation(moveX, moveZ);

			if(playerInputComponent.GetInputActions().interact.WasPressed) {
				if(canReelIn && fishToReelIn) {
                    isFishing = false;

					string animationToPlay = "Fishing-" + basePrefix + "-Pull-Back-Hard";
					player.GetAnimationManager().PlayAnimationByName(animationToPlay, true);	

					fishToReelIn.OnPlayerAttemptToCatchFish();
                    Invoke("CatchFishDelayed", .5f);
					
                } else {
                    if(!fishToReelIn) {
                        OnDoneWithFishing();
                    }
                }
			}
		}

		if(canFish) {
			if(playerInputComponent.GetInputActions().interact.WasPressed) {
				DoFish();
			}
		}
	}

    private void CatchFishDelayed() {
        fishToReelIn.OnCaught(player);

        ShowTextForBoombox("OnFishCaught");

        OnDoneWithFishing();
        Destroy (fishToReelIn.gameObject);
    }

	public void OnCollisionEnter(Collision coll) {
		FishCollider fishCollider = coll.gameObject.GetComponent<FishCollider>();
		if(fishCollider) {

			fishColliderToUse = fishCollider;
            
            Direction playerDirection = player.GetComponent<BodyControl>().GetCurrentDirection();

			if(fishCollider.requiredDirection == Direction.NONE || playerDirection == fishCollider.requiredDirection) {

                basePrefix = playerDirection.ToString();
				basePrefix = basePrefix[0] + basePrefix.Substring(1).ToLower();

				Logger.Log ("can fish! " + basePrefix);
				canFish = true;
			} else {
				DisableFishing();
			}
		}
	}

	public void OnCollisionExit(Collision coll) {
		FishCollider fishCollider = coll.gameObject.GetComponent<FishCollider>();
		if(fishCollider) {
			DisableFishing();
		}
	}

	private void DisableFishing() {
		Logger.Log ("cannot fish!");
		canFish = false;
		canReelIn = false;
	}
		
	private void OnDoneWithFishing() {
		isFishing = false;
		playerInputComponent.enabled = true;
		canReelIn = false;
		player.GetCharacterControl().characterState = CharacterControl.CharacterState.Idle;
		Destroy(dobberToUse.gameObject);
	}

	private void PlayCorrectAnimation(float moveX, float moveZ) {

		string correctAnimationName = "Fishing-" + basePrefix;

		switch(player.GetComponent<BodyControl>().GetCurrentDirection()) {

			case Direction.RIGHT:

				if(moveZ > 0) {
					correctAnimationName += "-Pull-Right";
				} else if(moveZ < 0) {
					correctAnimationName += "-Pull-Left";
				} else if(moveX < 0) {
					correctAnimationName += "-Pull-Back";
				}
			break;

			case Direction.LEFT:

				if(moveZ > 0) {
					correctAnimationName += "-Pull-Right";
				} else if(moveZ < 0) {
					correctAnimationName += "-Pull-Left";
				} else if(moveX > 0) {
					correctAnimationName += "-Pull-Back";
				}
			break;

			case Direction.UP:

				if(moveX > 0) {
					correctAnimationName += "-Pull-Right";
				} else if(moveX < 0) {
					correctAnimationName += "-Pull-Left";
				} else if(moveZ < 0) {
					correctAnimationName += "-Pull-Back";
				}
			break;

			case Direction.DOWN:

				if(moveX > 0) {
					correctAnimationName += "-Pull-Right";
				} else if(moveX < 0) {
					correctAnimationName += "-Pull-Left";
				} else if(moveZ > 0) {
					correctAnimationName += "-Pull-Back";
				}
			break;
			
		}

		player.GetAnimationManager().PlayAnimationByName(correctAnimationName, true);	
	}

	public void DoFish() {
		if(canFish) {
			canReelIn = false;
			canFish = false;
			FishDobber spawnedDobber = null;

            Direction playerDirection = player.GetComponent<BodyControl>().GetCurrentDirection();

			if(playerDirection == Direction.DOWN || playerDirection == Direction.UP) {
				spawnedDobber = SpawnFishingDobber(player.transform.position.x, fishColliderToUse.transform.position.z);
			}

			if(playerDirection == Direction.LEFT || playerDirection == Direction.RIGHT) {
				spawnedDobber = SpawnFishingDobber(fishColliderToUse.transform.position.x, player.transform.position.z);
			}

			spawnedDobber.transform.parent = fishColliderToUse.transform;

			spawnedDobber.SetMoveBounds(fishColliderToUse.GetComponent<Collider>().bounds);
			spawnedDobber.AddEventListener(this.gameObject);

			playerInputComponent.enabled = false;
			player.GetCharacterControl().characterState = CharacterControl.CharacterState.FISHING;
			player.GetAnimationManager().PlayAnimationByName("Fishing-" + basePrefix);

			this.dobberToUse = spawnedDobber;
			isFishing = true;
		}
	}

	private void StopFishing() {
		isFishing = false;
		canReelIn = false;
	}

	private FishDobber SpawnFishingDobber(float positionX, float positionZ) {
		FishDobber dobber = (FishDobber) GameObject.Instantiate(Resources.Load("Fishing/dobber", typeof(FishDobber)), new Vector3(positionX, this.transform.position.y, positionZ), Quaternion.identity);

		return dobber;
	}

	public void SetFishThatCanBeReeledIn(SwimmingFish fishToReelIn) {
		this.fishToReelIn = fishToReelIn;
	}

	public void UnSetFishThatCanBeReeledIn() {
		this.fishToReelIn = null;
	}
	
	public void ToggleReelingIn(bool canReelIn) {
		this.canReelIn = canReelIn;
	}

	private void ShowTextForBoombox(string textBoxName) {
		BoomBox boomboxOnBack = player.GetComponentInChildren<BoomBox>();
		if(boomboxOnBack) {
			boomboxOnBack.ShowTextBox(textBoxName);
		}
	}
}
