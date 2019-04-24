using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour {
	
	public float timeout;
	public Animation2D animation2D;

	// Use this for initialization
	void Start () {
		Cursor.visible = false; 
		animation2D.AddEventListener (this.gameObject);
	}

	public void OnAnimationDone(Animation2D animation2D) {
		Invoke ("LoadLevel", timeout);
			
	}
	
	private void LoadLevel() {
		SceneManager.LoadScene ("MenuScene", LoadSceneMode.Single);
	}

}
