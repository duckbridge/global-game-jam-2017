using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserAttack : EnemyAction {

	public SoundObject laserBlastSound, glassBreakSound;

	public string actionOnDone = "";
	public float minimumLaserAttackDuration = 1f;
	public float maximumLaserAttackDuration = 2f;

	public float minimumLaserAttackTimeout = 1f;
	public float maximumLaserAttackTimeout = 2f;

	private SpriteRenderer straightLaserLeft, straightLaserRight, sideLaserLeft, sideLaserRight;
	private GameObject straightBlastLeft, straightBlastRight, sideBlastLeft, sideBlastRight;

	private enum ShootType { STRAIGHT, LEFT, RIGHT, SIDE }
	private ShootType shootType;

    private DungeonBossRoom bossRoom;

	public void Awake() {
		straightLaserLeft = this.transform.Find("StraightLaserLeft").GetComponent<SpriteRenderer>();
		straightLaserRight = this.transform.Find("StraightLaserRight").GetComponent<SpriteRenderer>();

		sideLaserLeft = this.transform.Find("SideLaserLeft").GetComponent<SpriteRenderer>();
		sideLaserRight = this.transform.Find("SideLaserRight").GetComponent<SpriteRenderer>();
		
		straightBlastLeft = this.transform.Find("StraightBlastLeft").gameObject;
		straightBlastRight = this.transform.Find("StraightBlastRight").gameObject;

		sideBlastLeft = this.transform.Find("SideBlastLeft").gameObject;
		sideBlastRight = this.transform.Find("SideBlastRight").gameObject;

        bossRoom = SceneUtils.FindObject<DungeonBossRoom>();
	}

	protected override void OnActionStarted () {
		int shootTypeDecider = Random.Range (0, 4);
		switch(shootTypeDecider) {
			case 0:
				shootType = ShootType.LEFT;
			break;

			case 1:
				shootType = ShootType.RIGHT;
			break;

			case 2:
				shootType = ShootType.STRAIGHT;
			break;

			case 3:
				shootType = ShootType.SIDE;
			break;
		}

		DisableAllLasers();

		switch(shootType) {
			case ShootType.STRAIGHT:

				controllingEnemy.PlayAnimationByName("ShootStraight-Prep", true);
				
				straightLaserLeft.enabled = true;
				straightLaserRight.enabled = true;
				
			break;

			case ShootType.SIDE:
				controllingEnemy.PlayAnimationByName("ShootSides-Prep", true);
				
				sideLaserLeft.enabled = true;
				sideLaserRight.enabled = true;
			break;

			case ShootType.LEFT:

				controllingEnemy.PlayAnimationByName("ShootLeft-Prep", true);
				
				straightLaserLeft.enabled = true;
				
			break;

			case ShootType.RIGHT:

				controllingEnemy.PlayAnimationByName("ShootRight-Prep", true);
				
				straightLaserRight.enabled = true;

				
			break;
		}

		Invoke ("DoLaserAttack", Random.Range(minimumLaserAttackTimeout, maximumLaserAttackTimeout));
	}

	private void DoLaserAttack() {

		DisableAllLasers();

		laserBlastSound.Play(true);

        List<Animation2D> glassBreakAnimations = new List<Animation2D>();
    
		switch(shootType) {
			case ShootType.STRAIGHT:
				straightBlastLeft.GetComponent<Animation2D>().Play(true);
				straightBlastLeft.GetComponent<Collider>().enabled = true;
				
				straightBlastRight.GetComponent<Animation2D>().Play(true);
				straightBlastRight.GetComponent<Collider>().enabled = true;
				
				controllingEnemy.PlayAnimationByName("ShootStraight", true);

                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/midleft").GetComponent<Animation2D>());
                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/midright").GetComponent<Animation2D>());
            
            break;

			case ShootType.SIDE:
				sideBlastLeft.GetComponent<Animation2D>().Play(true);
				sideBlastRight.GetComponent<Animation2D>().Play(true);
				
				sideBlastLeft.GetComponent<Collider>().enabled = true;
				sideBlastRight.GetComponent<Collider>().enabled = true;
				
				controllingEnemy.PlayAnimationByName("ShootSides", true);

                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/right").GetComponent<Animation2D>());
                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/left").GetComponent<Animation2D>());
			break;

			case ShootType.LEFT:

				straightBlastLeft.GetComponent<Animation2D>().Play(true);
				straightBlastLeft.GetComponent<Collider>().enabled = true;

				controllingEnemy.PlayAnimationByName("ShootLeft", true);
                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/midleft").GetComponent<Animation2D>());
                

           	break;

			case ShootType.RIGHT:

				straightBlastRight.GetComponent<Animation2D>().Play(true);
				straightBlastRight.GetComponent<Collider>().enabled = true;
				
				controllingEnemy.PlayAnimationByName("ShootRight", true);
            
                glassBreakAnimations.Add(bossRoom.transform.Find("Breakable_Glass/midright").GetComponent<Animation2D>());
                
			break;
		}

        foreach(Animation2D glassBreakAnimation in glassBreakAnimations) {
            if(glassBreakAnimation.GetCurrentFrame() == 0) {
                if(glassBreakSound != null) {
                    glassBreakSound.Play();
                }
            }
    
            glassBreakAnimation.Play();
        }

		Invoke ("StopLaserAttack", Random.Range(minimumLaserAttackDuration, maximumLaserAttackDuration));
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();

		DisableAllLasers();

		CancelInvoke("DoLaserAttack");
		CancelInvoke("StopLaserAttack");

	}

	private void StopLaserAttack() {
		laserBlastSound.Stop();

		DisableAllLasers();
		DeActivate(actionOnDone);
	}

	private void DisableAllLasers() {
		straightLaserLeft.enabled = false;
		straightLaserRight.enabled = false;

		sideLaserLeft.enabled = false;
		sideLaserRight.enabled = false;

		sideBlastLeft.GetComponent<Collider>().enabled = false;
		sideBlastRight.GetComponent<Collider>().enabled = false;

		straightBlastLeft.GetComponent<Collider>().enabled = false;
		straightBlastRight.GetComponent<Collider>().enabled = false;

		sideBlastLeft.GetComponent<Animation2D>().StopAndHide();
		sideBlastRight.GetComponent<Animation2D>().StopAndHide();

		straightBlastLeft.GetComponent<Animation2D>().StopAndHide();
		straightBlastRight.GetComponent<Animation2D>().StopAndHide();

	}
}
