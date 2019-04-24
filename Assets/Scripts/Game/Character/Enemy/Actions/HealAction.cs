using UnityEngine;
using System.Collections;

public class HealAction : EnemyAction {

    public float healAmount = 5f;
	public SoundObject healSound;

	public string actionNameOnDone = "PatrolAction";

	public float actionDuration = 1f;
	public string healAnimationName;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

        controllingEnemy.PlayAnimationByName(healAnimationName, true);
        healSound.Play();
    
        Enemy[] enemies = controllingEnemy.GetRoom().GetComponentsInChildren<Enemy>();
        foreach(Enemy enemy in enemies) {
            if(enemy != controllingEnemy) {
                enemy.DecreaseDamage(healAmount);
            }
        }

		Invoke ("OnDone", actionDuration); 
	}

	private void OnDone() {
		DeActivate(actionNameOnDone);
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();
	}
}
