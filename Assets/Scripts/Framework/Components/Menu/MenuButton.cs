using UnityEngine;
using System.Collections;

public class MenuButton : DispatchBehaviour {

	public MenuButtonType menuButtonType;
	private bool isMovable = true;
	protected bool isSelected = false;

	public virtual void Start() {
	}
	
	public virtual void OnPressed() {
		DispatchMessage("OnMenuButtonPressed", this);
	}

	public virtual void OnObjectClicked() {
		DispatchMessage("OnMenuButtonPressed", this);
	}

	public virtual void Disable() {
		isMovable = false;
		if(GetComponent<TextMesh>()) {
			GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
		}
	}

	public virtual void Enable() {
		isMovable = true;
		if(GetComponent<TextMesh>()) {
			GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
		}
	}	

	public bool IsMovable() {
		return isMovable;
	}

	public virtual void OnSelected() {
		isSelected = true;
	}
	public virtual void OnUnSelected() {
		isSelected = false;
	}

    protected void DoExtraActionOnSelection() {
        if(GetComponent<ExtraButtonAction>()) {
            GetComponent<ExtraButtonAction>().DoActionOnSelection();
        }
    }

    protected void DoExtraActionOnDeSelection() {
        if(GetComponent<ExtraButtonAction>()) {
            GetComponent<ExtraButtonAction>().DoActionOnDeSelection();
        }
    }
}
