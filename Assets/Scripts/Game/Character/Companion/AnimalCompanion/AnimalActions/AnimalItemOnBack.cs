using UnityEngine;
using System.Collections;

public class AnimalItemOnBack : MonoBehaviour {

	public float originalItemOnBackRotation = 55f;
	private Transform itemContainer;
	private Transform itemOnBack;

	// Use this for initialization
	void Start () {
		itemContainer = this.transform.Find("ItemContainer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetItemOnBack(string itemName) {

		itemOnBack = itemContainer.transform.Find(itemName);
		itemOnBack.gameObject.SetActive(true);

	}

	public void RemoveItemOnBack() {
		if(itemOnBack) {
			itemOnBack.gameObject.SetActive(false);
		}
	}

	public void UpdateItemOnBackPosition(Direction newDirection) {

		if(newDirection == Direction.UP) {
			
			itemContainer.transform.localPosition = new Vector3(0f, -0.2f, 0f);
			
			if(itemOnBack) {
				itemOnBack.transform.localEulerAngles = new Vector3(270f, originalItemOnBackRotation + 65, 0f);
			}
		}
		
		if(newDirection == Direction.DOWN) {
			
			itemContainer.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			
			if(itemOnBack) {
				itemOnBack.transform.localEulerAngles = new Vector3(90f, originalItemOnBackRotation, 0f);
			}
		}
		
		if(newDirection == Direction.LEFT) {
			
			itemContainer.transform.localPosition = new Vector3(.5f, 0.2f, 0f);
			
			if(itemOnBack) {
				itemOnBack.transform.localEulerAngles = new Vector3(90f, originalItemOnBackRotation+40, 0f);
			}
		}
		
		if(newDirection == Direction.RIGHT) {
			
			itemContainer.transform.localPosition = new Vector3(-.5f, 0.2f, 0f);
			
			if(itemOnBack) {
				itemOnBack.transform.localEulerAngles = new Vector3(270f, -(originalItemOnBackRotation+220), 0f);
			}
		}
	}

}
