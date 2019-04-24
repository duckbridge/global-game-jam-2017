using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInputComponent : DispatchBehaviour {

	private BodyControl bodyControl;
	private Player player;

	private CharacterControl characterControl;
	private PlayerInputActions playerInputActions;
    
	private bool isHoldingDirection = false;
	private AnimationManager2D animationManager;

	public void Awake() {
		player = GetComponent<Player>();
		this.bodyControl = GetComponent<BodyControl>();	

    	PlayerInputHelper.ResetInputHelper ();
		playerInputActions = PlayerInputHelper.LoadData();
	}

	// Use this for initialization
	void Start () {
	}
		
	public void Update() {
		if (bodyControl) {
			bodyControl.DoMove (playerInputActions.moveHorizontally.Value, playerInputActions.moveVertically.Value, true);
			this.transform.Find("CakParticles").position = 
				new Vector3 (this.transform.Find("CakParticles").position.x, 
					4, 
					this.transform.Find("CakParticles").position.z);

		}
	}
		
	public override void OnPauseGame () {}

	public override void OnResumeGame() {}

	public PlayerInputActions GetInputActions() {
		return playerInputActions;
	}
}
