using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class VillageTeleporterComponent : MonoBehaviour {
	
	public float teleportTime = 1.5f;

	private List<int> tileBlockIdsUnlocked = new List<int>();
	private Player player;

	private bool isAtTeleporter = false;
    private Teleporter currentTeleporter;

	// Use this for initialization
	void Start () {
	}

	public void Initialize() {
		player = GetComponent<Player>();
		tileBlockIdsUnlocked.Add (0);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void UnlockTileblockTeleporter(ref TileBlock tileblock) {

		if(!tileBlockIdsUnlocked.Contains(tileblock.id)) {
			tileBlockIdsUnlocked.Add (tileblock.id);
			SceneUtils.FindObject<PlayerSaveComponent>().SaveData(SpawnType.TELEPORTED);
		}
	}

	public void AddUnlockedTeleporter(int id) {
		if(!tileBlockIdsUnlocked.Contains(id)) {
			tileBlockIdsUnlocked.Add (id);
		}
	}

	public List<int> GetTileBlockIdsUnlocked() {
		return tileBlockIdsUnlocked;
	}

	public bool IsAtTeleporter() {
		return isAtTeleporter;
	}

    public void SetCurrentTeleporter(Teleporter teleporter) {
        this.currentTeleporter = teleporter;
    }
	
	public void SetAtTeleporter(bool isAtTeleporter) {
		this.isAtTeleporter = isAtTeleporter;
	}

    public Teleporter GetCurrentTeleporter() {
        return this.currentTeleporter;
    }
}
