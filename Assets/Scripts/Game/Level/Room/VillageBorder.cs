using UnityEngine;
using System.Collections;

public class VillageBorder : DispatchBehaviour {

    public Direction playerDirectionToCheck;

    private bool isActivated = false;
    private Player player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateBorder(Player playerToCheck) {
        this.player = playerToCheck;
        this.isActivated = true;
    }

    public void DeActivateBorder() {
        this.isActivated = false;
        this.player = null;
    }

    void FixedUpdate() {
        if(isActivated) {
            switch(playerDirectionToCheck) {
                case Direction.RIGHT:
                    
                    if(player.transform.position.x > this.transform.position.x) {
                        DispatchMessage("OnVillageBorderPassed", new Vector2(1f, 0f));
                    }
                break;

                case Direction.LEFT:
                    if(player.transform.position.x < this.transform.position.x) {
                        DispatchMessage("OnVillageBorderPassed", new Vector2(-1f, 0f));
                    }
                break;

                case Direction.UP:
                    if(player.transform.position.z > this.transform.position.z) {
                        DispatchMessage("OnVillageBorderPassed", new Vector2(0f, 1f));
                    }
                break;

                case Direction.DOWN:
                    if(player.transform.position.z < this.transform.position.z) {
                        DispatchMessage("OnVillageBorderPassed", new Vector2(0f, -1f));
                    }
                break;

            }
        }
    }
}
