using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class TogglePlayerRoomBorders : CutSceneComponent {

        public bool enablePlayerRoomBorders = true;
        
        public override void OnActivated () {

            Player player = SceneUtils.FindObject<Player>();
            
            if(enablePlayerRoomBorders) {
                player.GetCurrentRoomNode().GetRoom().GetComponent<VillageRoom>().ActivateAndListenToAllBorders(player);
            } else {
                player.GetCurrentRoomNode().GetRoom().GetComponent<VillageRoom>().DeActivateAndStopListeningToAllBorders();
            }

            DeActivate();
        }
    }
}
