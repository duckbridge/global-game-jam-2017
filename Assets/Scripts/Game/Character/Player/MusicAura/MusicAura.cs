using UnityEngine;
using System.Collections;

public class MusicAura : DispatchBehaviour {

    public MusicAuraTypes musicAuraType;
	public float tinySoundBlastPushForce = 0;
	public float pushForce = 0f;

	private float tinyDamageIncrementAmount = 20f;
	private float damageIncrementAmount = 0.5f;

	public Player player;

	private SoundBlastAnimation2D auraBlastAnimation, tinyAuraBlastAnimation;

	private bool isTinySoundBlast = false;

	private Transform followTarget;

	private SoundObject[] soundBlastSounds;
	private SoundObject[] tinySoundBlastSounds;

	void Awake() {

		soundBlastSounds = this.transform.Find("Sounds/SoundBlastSounds").GetComponentsInChildren<SoundObject>();
		tinySoundBlastSounds = this.transform.Find("Sounds/TinySoundBlastSounds").GetComponentsInChildren<SoundObject>();

		auraBlastAnimation = this.transform.Find("Animations/SoundBlastAnimation").GetComponent<SoundBlastAnimation2D>();
		tinyAuraBlastAnimation = this.transform.Find("Animations/TinySoundBlastAnimation").GetComponent<SoundBlastAnimation2D>();
	}

	void Start () {}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(followTarget) {
			this.transform.position = new Vector3(followTarget.position.x, this.transform.position.y, followTarget.position.z);
		}
	}

	public void DoSoundBlast() {

		isTinySoundBlast = false;

		if(auraBlastAnimation.GetComponent<CrossBlastAnimation2D>()) {
			auraBlastAnimation.GetComponent<CrossBlastAnimation2D>().SetPlayerDirection(player.GetComponent<BodyControl>().GetCurrentDirection());
		}

        if(auraBlastAnimation.GetComponent<BulletBlastAnimation2D>()) {
            auraBlastAnimation.GetComponent<BulletBlastAnimation2D>().SetPlayerDirection(player.GetComponent<BodyControl>().GetCurrentDirection());
        }

		int randomSoundIndex = Random.Range (0, soundBlastSounds.Length);
		soundBlastSounds[randomSoundIndex].Play();

		auraBlastAnimation.SetLastFrameOverride(-1);
		auraBlastAnimation.Play(true);
	
	}

	public void DoTinySoundBlast() {

		isTinySoundBlast = true;

		if(tinyAuraBlastAnimation.GetComponent<CrossBlastAnimation2D>()) {
			tinyAuraBlastAnimation.GetComponent<CrossBlastAnimation2D>().SetPlayerDirection(player.GetComponent<BodyControl>().GetCurrentDirection());
		}

        if(tinyAuraBlastAnimation.GetComponent<BulletBlastAnimation2D>()) {
            tinyAuraBlastAnimation.GetComponent<BulletBlastAnimation2D>().SetPlayerDirection(player.GetComponent<BodyControl>().GetCurrentDirection());
        }

		int randomSoundIndex = Random.Range (0, tinySoundBlastSounds.Length);
		tinySoundBlastSounds[randomSoundIndex].Play();

		if(tinyAuraBlastAnimation.GetLastFrameOverride() > 1) {
			tinyAuraBlastAnimation.SetLastFrameOverride(tinyAuraBlastAnimation.GetLastFrameOverride() - 1);
		}

		tinyAuraBlastAnimation.Play(true);
	}


	public void TrackWeapon(Transform weaponTransform) {

		this.transform.parent = null;
		followTarget = weaponTransform;
	}

	public void TrackPlayer() {

		followTarget = player.transform;
		this.transform.parent = player.transform;
		this.transform.position = new Vector3(followTarget.position.x, this.transform.position.y, followTarget.position.z);
		followTarget = null;

	}

	public void OnTriggerEnter(Collider coll) {
		Enemy enemy = coll.gameObject.GetComponent<Enemy>();
		if(enemy) {

            Vector3 directionToEnemy = enemy.transform.position - this.transform.position;
            directionToEnemy.Normalize();

			if(pushForce > 0) {
				enemy.PushInDirection(new Vector3(directionToEnemy.x, 0f, directionToEnemy.z) * (isTinySoundBlast ? tinySoundBlastPushForce : pushForce), isTinySoundBlast); 
			}

            enemy.SpawnOnHitParticles(directionToEnemy);
			enemy.DoDamage((isTinySoundBlast ? tinyDamageIncrementAmount : damageIncrementAmount), musicAuraType);

		}

		AnimalCompanion animalCompanion = coll.gameObject.GetComponent<AnimalCompanion>();
		if(animalCompanion) {
			animalCompanion.DoDamage(isTinySoundBlast ? tinyDamageIncrementAmount : damageIncrementAmount);
		}

		BorderWall borderWall = coll.gameObject.GetComponent<BorderWall>();
		if(borderWall) {

            Vector3 directionToWall = borderWall.transform.position - this.transform.position;
            directionToWall.Normalize();

			borderWall.DoDamage(isTinySoundBlast ? tinyDamageIncrementAmount : damageIncrementAmount);
            borderWall.SpawnOnHitParticles(directionToWall);

		}
	}

	public SoundBlastAnimation2D GetTinyBlastAnimation() {
		return tinyAuraBlastAnimation;
	}

	public void SetBlastDamageIncrementAmount(float damageIncrementAmount) {
		this.damageIncrementAmount = damageIncrementAmount;
	}
	
	public void SetTinyBlastDamageIncrementAmount(float tinyDamageIncrementAmount) {
		this.tinyDamageIncrementAmount = tinyDamageIncrementAmount;
	}
}
