using UnityEngine;
using System.Collections;

public class FirstDungeonDoor : DungeonDoor {

    public TextBoxManager textBoxOnDoorClosed;

    public override void Start () {

        if(isEntrance) {

            Invoke ("UnlockDoor", 1f);
        }
    }

    public override void OnInteract (Player player) {

        if(canInteract) {
            if(isEntrance) {
                
                if(isLocked) {
                    if(!textBoxOnDoorClosed.IsActivated()) {
                        textBoxOnDoorClosed.ResetShowAndActivate();  
                    }
                } else {
                    MapBuilder mapBuilder = SceneUtils.FindObject<MapBuilder>();
                    if(mapBuilder) {
						mapBuilder.SaveData(SpawnType.NORMAL);
                    }

                    player.GetComponent<PlayerInputComponent>().enabled = false;
					SceneUtils.FindObject<OnDieEffect> ().StartEffect ();
					Invoke("LoadDelayed", 1f);
                }
            }
        }
    }
}
