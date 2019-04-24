using UnityEngine;
using System.Collections;

public class ObjectAboveUnderPlayer : MonoBehaviour {

	public Transform centerPosition;

	public float moveUpOffset = 1f;
	private MainPlayer mainPlayer;
	private float originalPosition;
	private bool isInitialized = false;

	public void Start () {
		if (!isInitialized) {
			mainPlayer = SceneUtils.FindObject<MainPlayer> ();
			originalPosition = this.transform.position.y;
			isInitialized = true;

			if (!centerPosition) {
				centerPosition = this.transform;
			}
		}
	}

	public void Update () {
		if (mainPlayer.GetFeet().position.z < centerPosition.position.z) { //move down
			this.transform.position = new Vector3(this.transform.position.x, originalPosition - moveUpOffset, this.transform.position.z);
		} else if (mainPlayer.GetFeet().position.z >= centerPosition.position.z) { //move up
			this.transform.position = new Vector3(this.transform.position.x, originalPosition + moveUpOffset, this.transform.position.z);
		}
	}
}
