using UnityEngine;
using System.Collections;

public class SecondDungeonBossRoom : DungeonBossRoom {

    public Collider colliderToEnable;
    public Animation2D animation2dToPlay;

    protected override void OnHealthbarDepleted() {
		if (colliderToEnable) {
			colliderToEnable.enabled = true;
		}
        animation2dToPlay.Play(true);
    }
}
