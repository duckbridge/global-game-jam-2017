using UnityEngine;
using System.Collections;

public class TransformUtils : MonoBehaviour {

	public static void CopyXY(Transform source, Transform target, bool copyLocal = false) {
		if(source && target) {
			if(copyLocal) {
				target.localPosition = new Vector3(source.localPosition.x, source.localPosition.y, target.localPosition.z);
			} else {
				target.position = new Vector3(source.position.x, source.position.y, target.position.z);
			}
		}
	}

	public static bool IsInScreenX(Camera cameraToUse, Transform target) {
		Vector3 viewPoint = cameraToUse.WorldToViewportPoint(target.position);
		if(viewPoint.x > 0 && viewPoint.x < 1) {
			return true;
		} else {
			return false;
		}
	}

	public static bool IsInScreenXY(Camera cameraToUse, Transform target) {
		Vector3 viewPoint = cameraToUse.WorldToViewportPoint(target.position);
		if(viewPoint.x > 0 && viewPoint.x < 1 && viewPoint.y > 0 && viewPoint.y < 1) {
			return true;
		} else {
			return false;
		}
	}

	public static Vector3 ModifyX(Transform transformToModify, float newX, bool modifyLocal = false) {
		Vector3 newVector = modifyLocal ? transformToModify.localPosition : transformToModify.position;
		newVector.x = newX;

		return newVector;
	}

	public static Vector3 ModifyY(Transform transformToModify, float newY, bool modifyLocal = false) {
		Vector3 newVector = modifyLocal ? transformToModify.localPosition : transformToModify.position;
		newVector.y = newY;
		
		return newVector;
	}
		
	public static Vector3 ModifyXY(Transform transformToModify, Vector2 newXY, bool modifyLocal = false) {
		Vector3 newVector = modifyLocal ? transformToModify.localPosition : transformToModify.position;

		newVector.x = newXY.x;
		newVector.y = newXY.y;
		
		return newVector;
	}

	public static Vector3 ModifyZ(Transform transformToModify, float newZ, bool modifyLocal = false) {
		Vector3 newVector = modifyLocal ? transformToModify.localPosition : transformToModify.position;
		newVector.z = newZ;
		
		return newVector;
	}

	public static Vector3 InvertScaleY(Transform sourceTransform) {
		Vector3 scale = new Vector3(sourceTransform.localScale.x, -sourceTransform.localScale.y, sourceTransform.localScale.z);
		return scale;
	}

	public static Vector3 InvertScaleX(Transform sourceTransform) {
		Vector3 scale = new Vector3(-sourceTransform.localScale.x, sourceTransform.localScale.y, sourceTransform.localScale.z);
		return scale;
	}
}
