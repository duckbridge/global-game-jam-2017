using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameUtils {

	public static GameObject FindObjectInBetween(Vector3 positionA, Vector3 positionB) {
		
		float distance = Vector3.Distance(positionB, positionA);
		Vector3 direction = positionB - positionA;

		Debug.DrawLine(positionA, positionA + direction, Color.red, distance);
		
		RaycastHit hit;

		if (Physics.Raycast(positionA, direction, out hit, distance)) {
			if(hit.collider) {
				Debug.Log("found!" + hit.collider.gameObject);
				return hit.collider.gameObject;
			}
		}
		return null;
	}

	public static MonoBehaviour FindClosestObjTo(Vector3 position, MonoBehaviour[] sourceObjs, float maxClosestDistance) {
		MonoBehaviour closestGameObject = null;
		float closestDistance = maxClosestDistance;

		foreach(MonoBehaviour mb in sourceObjs) {
			GameObject gameObjectInBetween = GameUtils.FindObjectInBetween(position, mb.transform.position);

			if(gameObjectInBetween && gameObjectInBetween.GetComponent<Wall>()) {
				Debug.Log("cannot!");
				continue;
			}

			float distance = Vector3.Distance(mb.transform.position, position);
			if(distance < closestDistance) {
				closestDistance = distance;
				closestGameObject = mb;
			}
		}

		return closestGameObject;
	}
}