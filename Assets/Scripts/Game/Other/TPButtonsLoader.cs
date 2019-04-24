using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPButtonsLoader : MonoBehaviour {

    private List<MenuButton> originalMenuButtons;
    private bool hasSavedOriginalButtons = false;

    private Menu menu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SaveOriginalButtons() {
        if(!hasSavedOriginalButtons) {
            hasSavedOriginalButtons = true;
            originalMenuButtons = menu.menuButtons;
        }
    }

    public void Initialize() {

        menu = GetComponent<Menu>();
        SaveOriginalButtons();

        Player player = SceneUtils.FindObject<Player>();

        List<int> unlockedTileBlockIds = player.GetComponent<VillageTeleporterComponent>().GetTileBlockIdsUnlocked();
        
        for(int i = 0 ; i < menu.menuButtons.Count ;i++) {
            if(menu.menuButtons[i].menuButtonType != MenuButtonType.EXIT) {
                
                int menuButtonTileBlockId = System.Convert.ToInt32(menu.menuButtons[i].name);
                if(!unlockedTileBlockIds.Contains(menuButtonTileBlockId)) {

                    menu.menuButtons[i].Disable();
                    menu.menuButtons[i].gameObject.SetActive(false);
                    menu.menuButtons.RemoveAt(i);
                    i--;
                } else {
                    menu.menuButtons[i].Enable();
                    menu.menuButtons[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
