using UnityEngine;
using System.Collections;

public class CollderThatShowsTextBoxForTime : MonoBehaviour {

	public TextBoxManager textBoxManagerToActivate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter(Collision coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			textBoxManagerToActivate.ResetShowAndActivate();
		}
	}
}
