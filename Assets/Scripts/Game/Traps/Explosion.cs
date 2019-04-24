using UnityEngine;
using System.Collections;

public class Explosion : DispatchBehaviour {

    public float damageOnEnemy = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoExplode() {
		GetComponent<Collider>().enabled = true;
		this.transform.Find("ExplosionAnimation").GetComponent<Animation2D>().AddEventListener(this.gameObject);
		this.transform.Find("ExplosionAnimation").GetComponent<Animation2D>().Play(true);
	}

	public void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			player.OnHit(this.transform.position);
		}

        Enemy enemy = coll.gameObject.GetComponent<Enemy>();
        if(enemy) {

            Vector3 directionToEnemy = enemy.transform.position - this.transform.position;
            directionToEnemy.Normalize();

            enemy.DoDamage(damageOnEnemy, MusicAuraTypes.None);
            enemy.SpawnOnHitParticles(directionToEnemy);
            enemy.PushInDirection(directionToEnemy, true);

        }
	}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			player.OnHit(this.transform.position);
		}

        Enemy enemy = coll.gameObject.GetComponent<Enemy>();
        if(enemy) {

            Vector3 directionToEnemy = enemy.transform.position - this.transform.position;
            directionToEnemy.Normalize();

            enemy.DoDamage(damageOnEnemy, MusicAuraTypes.None);
            enemy.SpawnOnHitParticles(directionToEnemy);
            enemy.PushInDirection(directionToEnemy, true);

        }
	}

	public void OnAnimationDone(Animation2D animation2D) {
		GetComponent<Collider>().enabled = false;
		DispatchMessage("OnExplosionDone", null);
	}
}
