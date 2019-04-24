using UnityEngine;
using System.Collections;

public class OnHitParticleSpawner : MonoBehaviour {
    
    public float particleDestroyTimeout = 1f;

    public OnHitParticle[] onHitParticles;

    public int minParticles = 5;
    public int maxParticles = 10;

    public float minForce, maxForce;
    public float minSpread, maxSpread;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnParticles(Vector3 direction) {

        int amountOfParticles = Random.Range(minParticles, maxParticles);
        
        for(int i = 0 ; i < amountOfParticles ; i++) {

            OnHitParticle spawnedParticle = (OnHitParticle) GameObject.Instantiate(onHitParticles[Random.Range(0, onHitParticles.Length)], this.transform.position, Quaternion.identity);
            
            float force = Random.Range(minForce, maxForce);
            float spread = Random.Range(minSpread, maxSpread);

            Vector3 forceDirection = new Vector3(Mathf.Abs(force * direction.x), Mathf.Abs(force * direction.y), Mathf.Abs(force * direction.z));

            spawnedParticle.transform.right = new Vector3(direction.x, 0f, direction.z);
            spawnedParticle.transform.Rotate(new Vector3(0f, spread, 0f));

            Vector3 forceDirectionAfterRotation = 
                new Vector3(forceDirection.x * spawnedParticle.transform.right.x, 
                            0f, 
                            forceDirection.z * spawnedParticle.transform.right.z
                );

            spawnedParticle.GetComponent<Rigidbody>().AddForce(forceDirectionAfterRotation, ForceMode.Impulse);            

            spawnedParticle.DestroyDelayed(particleDestroyTimeout);
        }
    }
}
