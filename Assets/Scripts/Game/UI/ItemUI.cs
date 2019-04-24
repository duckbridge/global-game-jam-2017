using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUI : MonoBehaviour {

	public bool isWeaponUI = true;

	private List<GameObject> uiItems = new List<GameObject>();
	public Transform[] itemPositions;
	public Transform onSpawnedItemPosition;

	// Use this for initialization
	void Awake () {
	
	}

	public void Initialize() {
		if(isWeaponUI) {
			SceneUtils.FindObject<PlayerWeaponManager>().AddEventListener(this.gameObject);
		} else {
			SceneUtils.FindObject<Player>().AddEventListener(this.gameObject);
		}
		OnItemUsed();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnItemUsed() {
		if(uiItems.Count > 0) {
			GameObject itemUsed = uiItems[0];
			uiItems.RemoveAt(0);
			Destroy (itemUsed);
			
			MoveAllItems();
		}
	}

	public void OnItemPickedUp(string itemPath) {
		GameObject uiItem = GameObject.Instantiate(Resources.Load(itemPath, typeof(GameObject)), onSpawnedItemPosition.position , Quaternion.identity) as GameObject;
		uiItem.transform.parent = this.transform;

		if(uiItems.Count < itemPositions.Length) {
			uiItem.transform.localPosition = itemPositions[uiItems.Count].localPosition;		
		}

		uiItems.Add (uiItem);

	}

	private void MoveAllItems() {
		for(int i = 0 ; i < itemPositions.Length; i++) {
			if(i < uiItems.Count) {
				uiItems[i].transform.localPosition = itemPositions[i].localPosition;		
			}
		}
	}
}
