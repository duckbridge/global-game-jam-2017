using UnityEngine;
using System.Collections;

public class TextBoxManagerWithSavedInputAsName : TextBoxManager {

    public string inputSaveName = "KiddoOneName";

    protected override void OnActivated() {

       PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
       string newName = playerSaveComponent.GetSavedInputByName(inputSaveName);
       npcPictureNames.transform.Find("NPCName").GetComponent<TextMesh>().text = newName;

        base.OnActivated();
    }
}
