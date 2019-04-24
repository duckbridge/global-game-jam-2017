using UnityEngine;
using System.Collections;

public class BeatObject : MonoBehaviour {

	public bool activateOnStart = false;

	protected BeatListener beatListener;

	public virtual void Initialize() {
	
	}

	// Use this for initialization
	public virtual void Start () {
		if(activateOnStart) {
			Activate ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnBeatEvent() {
	}

	public void FindBeatListener() {
		beatListener = SceneUtils.FindObject<BeatListener>();
	}

	public virtual void Activate() {
		if(!beatListener) {
			FindBeatListener();
		}

		beatListener.AddEventListener(this.gameObject);
	}

	public virtual void Deactivate() {
		if(!beatListener) {
			FindBeatListener();
		}
		
		beatListener.RemoveEventListener(this.gameObject);
	}
}
