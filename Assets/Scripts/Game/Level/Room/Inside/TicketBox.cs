using UnityEngine;
using System.Collections;

public class TicketBox : InteractionObject {
    
    public LAShopKeeper laShopKeeper;
    private int currentNumber = 22;
  
    private SoundObject ticketDrawSound;
    private TextMesh numberOutput;
    private Animation2D regularTicketBox, explodedTicketBox;

    public override void Start() {
        base.Start();
        numberOutput = this.transform.Find("NumberOutput").GetComponent<TextMesh>();
        ticketDrawSound = this.transform.Find("Sounds/TicketDrawSound").GetComponent<SoundObject>();

        regularTicketBox = this.transform.Find("Regular").GetComponent<Animation2D>();
        explodedTicketBox = this.transform.Find("Exploded").GetComponent<Animation2D>();

        numberOutput.text = "#"+currentNumber;
    }

    public override void OnInteract(Player player) {
        base.OnInteract(player);
            
        if(currentNumber < 99) {
            currentNumber++;

            string numberAsString = currentNumber+"";

            numberOutput.text = "#" + numberAsString;
            laShopKeeper.ShoutNumber(currentNumber + 1);
            ticketDrawSound.Play();

        } else {
            ExplodeBox();
            numberOutput.text = "";
            laShopKeeper.OnBoxExploded();
        
        }
    }

    private void ExplodeBox() {
        regularTicketBox.StopAndHide();
        explodedTicketBox.Play(true);
        DisableInteraction(SceneUtils.FindObject<Player>());
    }
}
