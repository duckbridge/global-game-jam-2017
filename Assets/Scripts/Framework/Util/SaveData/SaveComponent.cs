using UnityEngine;
using System.Collections;

public abstract class SaveComponent : MonoBehaviour {
	
	public string objectName;
	public int villageNumber;

	public bool saveTeporary = false;

	// Use this for initialization
	public virtual void Awake () {
		Load ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void Load(bool loadTmpData = false);
	public abstract void Save(bool saveTmpData = false);
}
