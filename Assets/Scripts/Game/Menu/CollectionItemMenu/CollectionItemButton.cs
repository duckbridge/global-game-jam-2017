using UnityEngine;
using System.Collections;

public class CollectionItemButton : MenuButtonWithColors {

    protected bool isVisible = true;

    public override void OnSelected() {
        this.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = true;
    }


    public override void OnUnSelected() {
        this.transform.Find("Background").GetComponent<SpriteRenderer>().enabled = false;
    }

    public virtual void SetVisible(bool isVisible) {
        this.isVisible = isVisible;
        this.transform.Find("HideLayer").GetComponent<SpriteRenderer>().enabled = !this.isVisible;
    }

    public bool IsVisible() {
        return isVisible;
    }
}
