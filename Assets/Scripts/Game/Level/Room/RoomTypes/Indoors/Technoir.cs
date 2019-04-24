using UnityEngine;
using System.Collections;

public class Technoir : IndoorsContainer {

	public CassettePickup cassettePickup;
    private MusicManager musicManager;

    private SoundObjectWithInfo savedMusic;

	public override void OnPlayerEntered (Player player, GameObject gameCamera, Vector3 playerSpawnPositionOnExit) {
        
        musicManager = SceneUtils.FindObject<MusicManager>();
        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();

		base.OnPlayerEntered (player, gameCamera, playerSpawnPositionOnExit);

        musicManager.StopCurrentMusic();
        savedMusic = musicManager.GetCurrentMusic();

        if(cassettePickup && !playerSaveComponent.GetUnlockedTileTypeTracks().Contains(cassettePickup.tileType)) {
            cassettePickup.AddEventListener(this.gameObject);
            musicManager.GetMusicByTileType(cassettePickup.tileType).Play();
        }
	}

    public override void OnPlayerExitted() {
        base.OnPlayerExitted();

        if(cassettePickup) {
            musicManager.GetMusicByTileType(cassettePickup.tileType).Stop();
        }

        musicManager.GetCurrentMusic().Play();
        
    }

    public void OnCassettePickedUp() {
        musicManager.GetMusicByTileType(cassettePickup.tileType).Stop();
    }
}
