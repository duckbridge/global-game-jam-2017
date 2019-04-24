using UnityEngine;
using System.Collections;

public class SaveComputer : InteractionObject {

	public Animation2D saveAnimation2D;
	public Animation2D expandAnimation2D;

    public SoundObject onSaveSound;
    public SoundObject onExpandSound;

	public override void OnInteract (Player player) {

		if(player.GetAnimationControl().GetCurrentAnimationGroup() != AnimationGroup.Naked) {
			if(canInteract) {
				base.OnInteract (player);

				onSaveSound.Play();

                saveAnimation2D.Show();
                saveAnimation2D.Play();

				SceneUtils.FindObject<LevelBuilder>().SaveData(SpawnType.NORMAL);
			}
		}
	}

    public override void OnTriggerExit(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
            base.OnTriggerExit(coll);
            expandAnimation2D.Play(true, true);
            saveAnimation2D.StopAndHide();
        }
    }

    public override void OnTriggerEnter(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
            base.OnTriggerEnter(coll);
            expandAnimation2D.Play(true);
            onExpandSound.Play();
        }
    }
}
