using UnityEngine;
using System.Collections;

public class ChargeShotAction : EnemyAction {

    public GameObject upLaser, downLaser;
    public string actionOnDone = "MoveAligned";
	
	public SoundObject shootSound;

	private SecondBossEnemy bossEnemy;
    private Direction shootDirection;

    private enum ShotState { Normal, Charging, Shooting }
    private ShotState shotState = ShotState.Normal;

	protected override void OnActionStarted () {
        
        shotState = ShotState.Normal;

		bossEnemy = controllingEnemy.GetComponent<SecondBossEnemy>();	
		Invoke("PrepareShooting", .5f);
		
		base.OnActionStarted ();
	}

    protected override void OnUpdate() {
        base.OnUpdate();
       
        switch(shotState) {

            case ShotState.Charging:
                if(bossEnemy.GetRideDirection() == Direction.RIGHT || bossEnemy.GetRideDirection() == Direction.LEFT) {
                    
                    string animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Shoot";
                    
                    shotState = ShotState.Shooting;
                    DoShoot ();
                    controllingEnemy.GetAnimationManager().PlayAnimationByName(animationName, true);
                
                }
            break;

            case ShotState.Shooting:
                if(bossEnemy.GetRideDirection() == Direction.UP || bossEnemy.GetRideDirection() == Direction.DOWN) {
                    DisableLasers();
                    DeActivate(actionOnDone);
                    
                }
            break;

        }

        
    }

	private void PrepareShooting() {

		shootDirection = Direction.RIGHT;
        string animationName = "";

		switch(bossEnemy.GetRideDirection()) {
			case Direction.RIGHT:
				
				shootDirection = Direction.UP;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.DOWN;
				}
                animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Shoot";
                shotState = ShotState.Shooting;
                DoShoot ();
			break;

			case Direction.LEFT:

				shootDirection = Direction.DOWN;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.UP;
				}
                animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Shoot";
                shotState = ShotState.Shooting;
                DoShoot ();
			break;
		
			case Direction.UP:

				shootDirection = Direction.DOWN;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.UP;
                }
                animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Charge";
                shotState = ShotState.Charging;
			break;

			case Direction.DOWN:

                shootDirection = Direction.UP;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.DOWN;
				}
                animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Charge";
                shotState = ShotState.Charging;
			break;

		}

        Logger.Log(animationName);
		controllingEnemy.GetAnimationManager().PlayAnimationByName(animationName, true);
	}
	
	private void DoShoot() {
		shootSound.Play();
		
        DisableLasers();

        if(shootDirection == Direction.DOWN) {
            downLaser.GetComponent<Animation2D>().Show();
            downLaser.GetComponent<Animation2D>().Play(true);
            downLaser.GetComponent<Collider>().enabled = true;
        } else {
            upLaser.GetComponent<Animation2D>().Show();
            upLaser.GetComponent<Animation2D>().Play(true);
            upLaser.GetComponent<Collider>().enabled = true;
        }

        shotState = ShotState.Shooting;
	}
	
    protected override void OnActionFinished() {
        CancelInvoke("PrepareShooting");
        CancelInvoke("OnActionDone");

        DisableLasers();

        base.OnActionFinished();
    }

    private void DisableLasers() {
        downLaser.GetComponent<Animation2D>().StopAndHide();
        downLaser.GetComponent<Collider>().enabled = false;

        upLaser.GetComponent<Animation2D>().StopAndHide();
        upLaser.GetComponent<Collider>().enabled = false;
    }
}
