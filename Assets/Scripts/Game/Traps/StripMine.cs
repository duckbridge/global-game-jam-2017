using UnityEngine;
using System.Collections;

public class StripMine : Mine {

    public float damageOnEnemy = 1f;

	public override void OnTriggerEnter(Collider coll) {
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
}
