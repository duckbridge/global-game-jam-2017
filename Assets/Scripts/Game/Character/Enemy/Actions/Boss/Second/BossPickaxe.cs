using UnityEngine;
using System.Collections;

public class BossPickaxe : MonoBehaviour {

    public float rotationSpeed = 5f;
    
    private bool isInGround = false;
    private SpriteRenderer onThrowSprite, onInGroundSprite;

    void Awake() {
        onThrowSprite = this.transform.Find("Throwing").GetComponent<SpriteRenderer>();
        onInGroundSprite = this.transform.Find("InGround").GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {

        if(!isInGround && rotationSpeed != 0) {
            this.transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
        }
    }

    public void OnHitTheGround() {
        isInGround = true;
        this.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        
        onInGroundSprite.enabled = true;
        onThrowSprite.enabled = false;
    }

    public void OnThrown() {

        onInGroundSprite.enabled = false;
        onThrowSprite.enabled = true;

        isInGround = false;
    }
}
