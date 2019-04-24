using UnityEngine;
using System.Collections;

public class ShieldEnemy : Enemy {

    public Animation2D shieldShowAnimation, shieldBreakAnimation;

    public MusicAuraTypes shieldAuraType = MusicAuraTypes.Sphere;
    public float shieldResetTimeout = 1f;

    private bool isUsingShield = true;
    
    public override void OnActivate() {
        base.OnActivate();
        ToggleShield(true);
    }

    public override void DoDamage(float amountOfDamage, MusicAuraTypes musicAuraType) {
        if(isUsingShield) {
        
            if(musicAuraType == shieldAuraType) {

                ToggleShield(false);

                PlayAnimationByName("Walking", true);
                Invoke("ResetUsingShield", shieldResetTimeout);
            }
        
        } else {
            base.DoDamage(amountOfDamage, musicAuraType);
        }
    }

    public override void SpawnOnHitParticles(Vector3 direction) {
         if(!isUsingShield) {
            base.SpawnOnHitParticles(direction);
        }
    }

    private void ResetUsingShield() {
        ToggleShield(true);
    }

    protected override void OnDie() {
        CancelInvoke("ResetUsingShield");
        base.OnDie();
    }

    private void ToggleShield(bool isUsingShield) {
        this.isUsingShield = isUsingShield;
        
        shieldShowAnimation.StopAndHide();
        shieldBreakAnimation.StopAndHide();
        
        if(isUsingShield) {
            shieldShowAnimation.Show();
            shieldShowAnimation.Play(true);
        }

        if(!isUsingShield) {
            shieldBreakAnimation.Show();
            shieldBreakAnimation.Play(true);
        }
    }
}
