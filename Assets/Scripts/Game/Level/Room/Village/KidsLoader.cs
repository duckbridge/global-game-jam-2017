using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KidsLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("LoadSavedKiddos", .5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadSavedKiddos() {
		List<string> savedKiddos = SceneUtils.FindObject<PlayerSaveComponent>().GetSavedKiddoNames();
		foreach(string savedKiddo in savedKiddos) {
            this.transform.Find("Kids/"+savedKiddo).gameObject.SetActive(true);
		}
	}
}
