using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionItemMenuUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void UpdateVisibleButtons(CollectionManager collectionManager, List<ButtonRow> buttonRows) {
        List<MusicInfo> allMusicInfo = collectionManager.GetAllMusicInfo();
       
        for(int i = 0; i < buttonRows.Count ; i++) {
           ButtonRow buttonRow = buttonRows[i];

            for(int j = 0; j < buttonRow.buttons.Count; j++) {

                CollectionItemButton collectionItemButton = (CollectionItemButton) buttonRow.buttons[j];

                MusicInfo foundMusicInfo = allMusicInfo.Find(musicInfo => musicInfo.name.Equals(collectionItemButton.GetText()));
                bool showButton = (foundMusicInfo != null);

                collectionItemButton.SetVisible(showButton);
            }
        }
    }
}
