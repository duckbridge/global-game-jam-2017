using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	private Explosion explosion;
	private SoundObject explosionSound;

	// Use this for initialization
	void Start () {
		explosion = GetComponentInChildren<Explosion>();
		explosionSound = this.transform.Find("Sounds/ExplosionSound").GetComponent<SoundObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			explosionSound.PlayIndependent();
			explosion.AddEventListener(this.gameObject);
			explosion.DoExplode();
		}

        Enemy enemy = coll.gameObject.GetComponent<Enemy>();
        if(enemy) {
            explosionSound.PlayIndependent();
            explosion.AddEventListener(this.gameObject);
            explosion.DoExplode();
        }
	}

	public void OnExplosionDone() {
		Destroy (this.gameObject);
	}
}
