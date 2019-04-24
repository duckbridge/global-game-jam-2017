using UnityEngine;
using System.Collections;

public class GateRoom : VillageRoom {

	public int gateId = 1;
    public bool gateIsSpriteAnimation = false;
    public TextBoxManager textBoxToOpenGate;

	public float gateOpenSpeed = 5f;
	public GameObject gate;
	public Transform gateMoveTarget;

	private bool gateIsOpen = false;

    public override void Start() {
        base.Start();
        
        if(textBoxToOpenGate) {
            textBoxToOpenGate.AddEventListener(this.gameObject);
        }

		Invoke ("OpenGateIfIdIsFound", .5f);
    }

	private void OpenGateIfIdIsFound() {
		if (SceneUtils.FindObject<PlayerSaveComponent> ().GetBrokenGateIds ().Contains (gateId)) {
			OpenGate();
		}
	}

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		base.OnEntered (enemyActivationDelay, ref playerEntered);
        
        if(!textBoxToOpenGate) {
            TryToOpenGate();
        }
	}

    public void OnTextBoxDoneAndHidden() {
        TryToOpenGate();    
    }

    private void TryToOpenGate() {
        if(gate) {

			if (!gateIsOpen) {
				SceneUtils.FindObject<PlayerSaveComponent> ().AddBrokenGate (gateId);
			}

			OpenGate();
        }
    }

	private void OpenGate() {
		gateIsOpen = true;

		gate.GetComponent<Collider>().enabled = false;

		if(!gateIsSpriteAnimation) {
			iTween.MoveTo(gate, new ITweenBuilder().SetPosition(gateMoveTarget.position).SetEaseType(iTween.EaseType.linear).SetSpeed(gateOpenSpeed).Build());
		} else {
			gate.transform.Find("OpenAnimation").GetComponent<Animation2D>().Play();
		}
	}
}
