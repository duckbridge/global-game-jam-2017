using UnityEngine;
using System.Collections;

public class TextBoxControlledByBeat : TextBox {

    private BeatListener beatListener;

    public override void OnStart() {
        beatListener = SceneUtils.FindObject<BeatListener>();
        beatListener.AddEventListener(this.gameObject);

        base.OnStart();
    }

    public void OnBeatEvent() {
        ShowNextWord();
    }

    protected override void OnTextBoxDone() {
        if(!beatListener) {
           beatListener = SceneUtils.FindObject<BeatListener>(); 
        }
        
        beatListener.RemoveEventListener(this.gameObject);
    }
}
