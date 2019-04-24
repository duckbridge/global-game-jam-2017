using UnityEngine;
using System.Collections;

public class DungeonBossRoom : Room {

    public RoomNode fakeRoomNode;

	public string[] versionToUnlockForTileBlockIds;
	public int bossId = 1;
	public int teleporterTileBlockIdToUnlock = 1;
    
	public CutSceneManager cutsceneToPlay, cutsceneToPlayOnSkip;
	public CutSceneManager cutsceneToPlayOnBossDead;

	public GameObject kiddoToMoveToBoss;

	private Player player;

	public override void Awake () {
		base.Awake ();
		player = SceneUtils.FindObject<Player>();

		this.roomNode = fakeRoomNode;
		fakeRoomNode.SetRoom(this);

		player.SetCurrentRoomNode(fakeRoomNode);
	}

	void Start() {
		OnEntered(0f, ref player);

        SceneUtils.FindObject<SettingsSaveComponent>().LoadSettingsData();
		SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX); 

        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        SerializablePlayerDataSummary playerDataSummary = playerSaveComponent.LoadPlayerData();

        if(playerDataSummary.skipBossCutscene) {
		    cutsceneToPlayOnSkip.StartCutScene(true);
        } else {
            playerSaveComponent.UpdateSkipBossCutscene(true);
            cutsceneToPlay.StartCutScene(true);
        }
		player.OnSpawned();
    }


	protected override void ActivateEnemiesDelayed () {}

	public override void OnEnemyDied(Enemy enemy) {
		base.OnEnemyDied(enemy);

		if(amountOfEnemies <= 0) {
			Logger.Log ("boss dead");

			PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
			
			foreach(string versionToUnlockForId in versionToUnlockForTileBlockIds) {
				int tileBlockId = System.Convert.ToInt32(versionToUnlockForId.Split(',')[0]);
				int version = System.Convert.ToInt32(versionToUnlockForId.Split(',')[1]);
				
				playerSaveComponent.AddVersionForTileBlock(tileBlockId, version);
			}

            player.SetInTown(true);
            player.GetAnimationControl().SwapAnimationGroup(AnimationGroup.NormalVillage);

			playerSaveComponent.AddSavedKiddo(kiddoToMoveToBoss.name);
			playerSaveComponent.SetLastBossBeatenId(bossId);
            player.GetComponent<VillageTeleporterComponent>().AddUnlockedTeleporter(teleporterTileBlockIdToUnlock);
			playerSaveComponent.SaveData(SpawnType.TELEPORTED, true);

			kiddoToMoveToBoss.transform.position = enemy.transform.Find("KiddoSpawnPosition").position;
			cutsceneToPlayOnBossDead.StartCutScene(true);
		}
	}

    protected virtual void OnHealthbarDepleted() {
    }
}
