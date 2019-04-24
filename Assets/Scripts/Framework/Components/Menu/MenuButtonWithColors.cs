using UnityEngine;
using System.Collections;

public class MenuButtonWithColors : MenuButton {
	
	public Color selectColor;
	protected Color originalColor;

    protected string originalText;
    protected bool hasSetOriginalText = false;

	public virtual void Awake() {
		if(this.GetComponent<TextMesh>()) {
			originalColor = GetComponent<TextMesh>().color;
            
            SaveOriginalText();
		}
		
		if(this.GetComponent<SpriteRenderer>()) {
			originalColor = GetComponent<SpriteRenderer>().color;
		}
	}

	public override void Start () {

	}

	public override void OnSelected() {
		base.OnSelected ();

		if(this.GetComponent<TextMesh>()) {
			this.GetComponent<TextMesh>().color = selectColor;
		}

		if(this.GetComponent<SpriteRenderer>()) {
			this.GetComponent<SpriteRenderer>().color = selectColor;
		}

        DoExtraActionOnSelection();
	}
	
	public override void OnUnSelected() {
		base.OnUnSelected ();

		if(this.GetComponent<TextMesh>()) {
			GetComponent<TextMesh>().color = originalColor;	
		}

		if(this.GetComponent<SpriteRenderer>()) {
			this.GetComponent<SpriteRenderer>().color = originalColor;
		}

        DoExtraActionOnDeSelection();
	}

	public virtual void SetText(string text) {
		if(GetComponent<TextMesh>()) {
			GetComponent<TextMesh>().text = text;
		}
	}

    public void ResetText() {
        if(GetComponent<TextMesh>()) {
            GetComponent<TextMesh>().text = originalText;
        }
    }

    public string GetText() {
       return GetComponent<TextMesh>().text; 
    }

    public string GetOriginalText() {

        SaveOriginalText();
    
        return originalText;
    }

    private void SaveOriginalText() {
          if(!hasSetOriginalText && GetComponent<TextMesh>()) {
            hasSetOriginalText = true;
            originalText = GetComponent<TextMesh>().text;
        }
    }

	public virtual void SetOriginalColor(Color color) {
		originalColor = color;
		if (!this.isSelected) {
			if(this.GetComponent<TextMesh>()) {
				this.GetComponent<TextMesh>().color = color;
			}

			if(this.GetComponent<SpriteRenderer>()) {
				this.GetComponent<SpriteRenderer>().color = color;
			}
		}
	}
}
