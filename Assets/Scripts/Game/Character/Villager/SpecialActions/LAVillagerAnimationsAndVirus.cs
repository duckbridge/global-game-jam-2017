using UnityEngine;
using System.Collections;

public class LAVillagerAnimationsAndVirus : VillagerSpecialAction {

    public float padMoveSpeed = 2f;
    public GameObject fakePad;
    public string installVirusAnimation = "installVirus";

    private Player player;
    private Villager villager;
    private bool hasInstalledVirus = false;

    public override void DoAction(Villager villager) {
        base.DoAction(villager);
        
        if(!hasInstalledVirus) {

            player = SceneUtils.FindObject<Player>();
            player.GetComponent<PlayerInputComponent>().enabled  = false;
            this.villager = villager;
    
            fakePad.SetActive(true);
            fakePad.GetComponent<Animation2D>().SetCurrentFrame(0);
                    
            MovePadToHacker();
        } else {
            player = SceneUtils.FindObject<Player>();
            player.GetComponent<PlayerInputComponent>().enabled  = true;
            player.OnTalkingDone();
        }

    }

    private void OnAnimationDone(Animation2D animation2D) {
        if(animation2D.name == installVirusAnimation) {

            MovePadToPlayer();
        
        }
    }

    private void MovePadToHacker() {
        fakePad.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
        iTween.MoveTo(fakePad, new ITweenBuilder().SetPosition(this.transform.position)
            .SetSpeed(padMoveSpeed).SetEaseType(iTween.EaseType.linear).SetOnCompleteTarget(this.gameObject).SetOnComplete("OnPadMovedToHacker").Build());
    }

    private void OnPadMovedToHacker() {
        fakePad.SetActive(false);
        PlayAnimation(installVirusAnimation);
    }

    private void MovePadToPlayer() {
        
        fakePad.GetComponent<Animation2D>().SetCurrentFrame(1);
        fakePad.SetActive(true);

        SceneUtils.FindObject<PauseScreenManager>().SwitchPauseScreen(PauseScreenTypes.Broken);
        hasInstalledVirus = true;

        PlayAnimation("Idle");

        iTween.MoveTo(fakePad, 
            new ITweenBuilder()
                .SetPosition(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z))
                .SetSpeed(padMoveSpeed).SetEaseType(iTween.EaseType.linear)
                .SetOnCompleteTarget(this.gameObject).SetOnComplete("OnPadMovedToPlayer").Build());
    }


    private void OnPadMovedToPlayer() {
        fakePad.SetActive(false);

        this.villager.DisableInteraction(player);
        player.GetComponent<PlayerInputComponent>().enabled  = true;
    }

    private void PlayAnimation(string animationName) {
        villager.GetAnimationManager().AddEventListenerTo(animationName, this.gameObject);
        villager.GetAnimationManager().PlayAnimationByName(animationName, true);
    }
}
