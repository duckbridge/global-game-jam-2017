using UnityEngine;
using System.Collections;

public class LAShopKeeper : VillageShopKeeper {

    public float shoutTimeout = 0f;
    public TextBoxManager textBoxOnBoxExploded;
    public GameObject shoutBox;

    public void OnBoxExploded() {
        textManager = textBoxOnBoxExploded;
    }

    public void ShoutNumber(int numberToShout) {
        shoutBox.GetComponent<TextMesh>().text = "#"+numberToShout;
        shoutBox.SetActive(true);

        //GetAnimationManager().PlayAnimationByName("Talking", true);
        //CancelInvoke("HideShoutBox");
        //Invoke("HideShoutBox", shoutTimeout);
    }

    private void HideShoutBox() {
        
        shoutBox.SetActive(false);
    }
}
