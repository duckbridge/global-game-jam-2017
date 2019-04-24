using UnityEngine;
using System.Collections;

public class EmailTrigger : MonoBehaviour {

    public string subject; 

    [TextArea(3,10)]
    public string text;
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider coll) {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player) {
            SubmitEmail(player);
        }
    } 

    protected virtual void SubmitEmail(Player player) {
        player.GetComponent<EmailSubmitComponent>().SubmitEmail(subject, text);
    }
}
