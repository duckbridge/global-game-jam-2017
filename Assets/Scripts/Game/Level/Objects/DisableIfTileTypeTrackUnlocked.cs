using UnityEngine;
using System.Collections;

public class DisableIfTileTypeTrackUnlocked : MonoBehaviour {

    public float disableTimeout = 2f;
	public TileType tileType = TileType.one;

	// Use this for initialization
	void Start () {
        if(disableTimeout == 0) {
            TryDisable(true);
        } else {
		    Invoke ("TryDisableOnStartup", disableTimeout);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void TryDisableOnStartup() {
        TryDisable(true);    
    }

    public void TryDisable(bool disablingAtStartup) {
		if(this.gameObject.activeInHierarchy && 
		   SceneUtils.FindObject<PlayerSaveComponent>().GetUnlockedTileTypeTracks().Contains(tileType)) {
			DoDisable(disablingAtStartup);
		}
	}

    protected virtual void DoDisable(bool disablingAtStartup) {
		this.gameObject.SetActive(false);

		ActionOnCassetteRoomDisable actionOnCassetteRoomDisable = GetComponent<ActionOnCassetteRoomDisable>();
		if(actionOnCassetteRoomDisable) {
			actionOnCassetteRoomDisable.DoAction(disablingAtStartup);
		}
	}
}
