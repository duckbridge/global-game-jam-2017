using UnityEngine;
using System.Collections;

public abstract class GameManager : DispatchBehaviour {

	public virtual void Start() {
		if(!Loader.HAS_DONE_FULL_RELOAD || !Loader.IS_USING_LOADER) {	
			OnStart();
		}
	}

	public abstract void OnStart();
}
