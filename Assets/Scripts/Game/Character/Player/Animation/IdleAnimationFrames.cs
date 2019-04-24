using UnityEngine;
using System.Collections;

public class IdleAnimationFrames : MonoBehaviour {

	public float FPS = 5;
	public bool doLoop = false;
	public Sprite[] frames;
	public float animationStartTimeout;

	private Sprite[] savedFrames;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public Sprite[] GetFramesAndSaveCopyOfIt() {
		savedFrames = frames;
		return frames;
	}
}
