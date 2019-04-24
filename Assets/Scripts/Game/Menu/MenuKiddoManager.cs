using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuKiddoManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(List<SerializablePlayerDataSummary> allSaveFiles) {
		
		SerializablePlayerDataSummary mostRecentSave = SaveUtils.FindMostRecentSaveFile(allSaveFiles);

		if(!mostRecentSave.isCorrupt) {
			List<string> kiddoNames = mostRecentSave.savedKiddoNames;

			foreach(string kiddoName in kiddoNames) {
				Transform foundKiddo = this.transform.Find ("kiddos/"+kiddoName);
				if(foundKiddo) {
					foundKiddo.gameObject.SetActive(true);
				}
			}

		}

		//test!!!
		this.transform.Find("kiddos/exampleKiddo").gameObject.SetActive(true);
	}
}
