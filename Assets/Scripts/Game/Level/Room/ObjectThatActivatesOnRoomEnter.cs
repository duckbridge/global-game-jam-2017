using UnityEngine;
using System.Collections;

public class ObjectThatActivatesOnRoomEnter : MonoBehaviour {

	protected bool isActivated = false;

	public virtual void Start () {
	
	}
	
	void Update () {
		if (!isActivated) {
			return;
		}

		OnUpdate ();
	}

	protected virtual void OnUpdate() {}

	public virtual void Activate() {
		isActivated = true;
	}

	public virtual void DeActivate() {
		isActivated = false;
	}
}