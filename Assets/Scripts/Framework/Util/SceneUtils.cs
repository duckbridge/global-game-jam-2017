using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneUtils : MonoBehaviour {

	public static List<T> FindObjects<T>() {
		T[] foundObjects = GameObject.FindObjectsOfType(typeof(T)) as T[];
		List<T> returnList = new List<T>(foundObjects);
		return returnList;
	}

	public static T FindObject<T>() {
		List<T> foundObjects = SceneUtils.FindObjects<T>();
		if(foundObjects.Count > 0) {
			return foundObjects[0];
		}
		Debug.Log("[SceneUtils] {WARN} could not find the object!");
		return default(T);
	}
}
