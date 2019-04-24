using UnityEngine;
using System.Collections;

public class OnStartAnimation : Animation2D {

	public void PrepareAndPlay() {
		Awake ();
		Play (true);
	}
}
