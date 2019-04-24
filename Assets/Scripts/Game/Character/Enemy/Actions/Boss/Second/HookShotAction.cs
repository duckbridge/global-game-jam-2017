using UnityEngine;
using System.Collections;

public class HookShotAction : EnemyAction {

    public BossPickaxe bossPickaxe;
    public float hookShotActionTimeout = 2f;
    public float axeThrowSpeed = .5f;

    public Transform bossRopeTarget;
    public BossRope bossRope;

    public float curtailingSpeed = 0.1f;

    public string actionOnDone = "MoveAligned";
	
	public SoundObject shootSound;

	private SecondBossEnemy bossEnemy;
   
    private Direction shootDirection;
    private Transform leftShootSource, rightShootSource, upShootSource, downShootSource;

    private bool isCurtailingRope = false;
    private bool isThrowingRope = false;

    void Awake() {
        leftShootSource = this.transform.Find("LeftShootSource");
        rightShootSource = this.transform.Find("RightShootSource");
        upShootSource = this.transform.Find("UpShootSource");
        downShootSource = this.transform.Find("DownShootSource");
    }

   	protected override void OnActionStarted () {
        
       	bossEnemy = controllingEnemy.GetComponent<SecondBossEnemy>();
        
        bossRope.gameObject.SetActive(true);	
		ExtendRopeToTarget();
		    
		base.OnActionStarted ();
	}

    protected override void OnUpdate() {
        base.OnUpdate();
       
        switch(bossEnemy.GetRideDirection()) {

            case Direction.RIGHT:
                shootDirection = Direction.UP;
                bossRope.transform.position = upShootSource.position;

                if(bossEnemy.IsRidingBackwards()) {
                    shootDirection = Direction.DOWN;
                    bossRope.transform.position = downShootSource.position;
                }

            break;

            case Direction.LEFT:
                shootDirection = Direction.DOWN;
                bossRope.transform.position = downShootSource.position;

                if(bossEnemy.IsRidingBackwards()) {
                    shootDirection = Direction.UP;
                    bossRope.transform.position = upShootSource.position;

                }

                break;

            case Direction.UP:
                shootDirection = Direction.LEFT;
                bossRope.transform.position = leftShootSource.position;

                if(bossEnemy.IsRidingBackwards()) {
                    shootDirection = Direction.RIGHT;
                    bossRope.transform.position = rightShootSource.position;

                }

            break;

            case Direction.DOWN:
                shootDirection = Direction.RIGHT;
                bossRope.transform.position = rightShootSource.position;

                if(bossEnemy.IsRidingBackwards()) {
                    shootDirection = Direction.LEFT;
                    bossRope.transform.position = leftShootSource.position;

                }

            break;

        }

        string animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Shoot";
        controllingEnemy.GetAnimationManager().ResumeAnimationByNameAndResetIfDone(animationName);

        if(isCurtailingRope) {
            CurtailRope();
        }

        if(bossRope.GetEndOfRope()) {
            bossPickaxe.transform.position = new Vector3(bossRope.GetEndOfRope().position.x, bossRope.GetEndOfRope().position.y + 0.3f, bossRope.GetEndOfRope().position.z);
        }
   }
	
    protected override void OnActionFinished() {
        CancelInvoke("PrepareShooting");
        CancelInvoke("OnActionDone");
        CancelInvoke("OnBeforeCurtailingRope");

        if(bossRope.GetFakeTarget()) {
            bossRope.GetFakeTarget().parent = bossEnemy.transform;
            bossRope.GetFakeTarget().localPosition = Vector3.zero;
        }

        bossRope.gameObject.SetActive(false);
        bossPickaxe.gameObject.SetActive(false);

        base.OnActionFinished();
    }
    
    private void ExtendRopeToTarget() {

        bossPickaxe.gameObject.SetActive(true);
        bossPickaxe.OnThrown();
        isThrowingRope = true;

        bossRope.GetFakeTarget().parent = bossEnemy.transform;
        bossRope.GetFakeTarget().localPosition = Vector3.zero;
        bossRope.GetFakeTarget().parent = null;

        iTween.MoveTo(bossRope.GetFakeTarget().gameObject, 
            new ITweenBuilder().SetPosition(bossRopeTarget.position)
                               .SetEaseType(iTween.EaseType.linear)
                               .SetSpeed(axeThrowSpeed)
                               .SetOnCompleteTarget(this.gameObject)
                               .SetOnComplete("OnRopeExtended").Build());
    }

    private void CurtailRope() {
        Vector3 movePosition = this.transform.position;

        if(Vector2.Distance(new Vector2(movePosition.x, movePosition.z), 
                            new Vector2(bossRope.GetFakeTarget().position.x, bossRope.GetFakeTarget().position.z)) > .5f) {

            Vector3 moveDirection = MathUtils.CalculateDirection(movePosition, bossRope.GetFakeTarget().position);
            bossRope.GetFakeTarget().transform.position += new Vector3(moveDirection.x * curtailingSpeed, 0f, moveDirection.z * curtailingSpeed);
    
        } else {
            OnRopeCurtailed();
        }
        
    }

    public void OnRopeCurtailed() {

        bossPickaxe.gameObject.SetActive(false);

        isCurtailingRope = false;

        bossRope.GetFakeTarget().parent = bossEnemy.transform;
        bossRope.GetFakeTarget().localPosition = Vector3.zero;
        bossRope.gameObject.SetActive(false);

        DeActivate(actionOnDone);
    }

    public void OnRopeExtended() {
        isThrowingRope = false;

        bossPickaxe.gameObject.SetActive(true);
        bossPickaxe.OnHitTheGround();

        Invoke("OnBeforeCurtailingRope", hookShotActionTimeout);
    }

    public void OnBeforeCurtailingRope() {

        bossPickaxe.gameObject.SetActive(true);
        bossPickaxe.OnThrown();

        isCurtailingRope = true;
    }
}
