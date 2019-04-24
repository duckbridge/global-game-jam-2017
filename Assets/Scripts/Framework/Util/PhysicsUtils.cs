using UnityEngine;
using System.Collections;

public class PhysicsUtils {

	public static void IgnoreCollisionBetween(GameObject gameObjectA, GameObject gameObjectB) {

		if(gameObjectA && gameObjectB) {
			IgnoreCollisionBetween(gameObjectA.GetComponent<Collider>(), gameObjectB.GetComponent<Collider>());
		}
	}

	public static void IgnoreCollisionBetween(Collider colliderA, Collider colliderB) {

		if(colliderA && colliderA.enabled && colliderA.gameObject.activeInHierarchy
		   && colliderB && colliderB.enabled && colliderB.gameObject.activeInHierarchy
		   && colliderA != colliderB) {

			Physics.IgnoreCollision(colliderA, colliderB);

		}
	}

	public static void IgnoreCollisionBetween(Collider colliderA, Collider[] collidersB) {
		
		if(colliderA && colliderA.enabled && colliderA.gameObject.activeInHierarchy) {
			foreach(Collider colliderB in collidersB) {
				if(colliderB.enabled && colliderB.gameObject.activeInHierarchy) {
					Physics.IgnoreCollision(colliderA, colliderB);
				}
			}
		}
	}

	public static void RestoreCollisionBetween(GameObject gameObjectA, GameObject gameObjectB) {

		if(gameObjectA && gameObjectB) {
			RestoreCollisionBetween(gameObjectA.GetComponent<Collider>(), gameObjectB.GetComponent<Collider>());
		}
	}
	
	public static void RestoreCollisionBetween(Collider colliderA, Collider colliderB) {

		if(colliderA && colliderA.enabled && colliderA.gameObject.activeInHierarchy
		   && colliderB && colliderB.enabled && colliderB.gameObject.activeInHierarchy) {
			
			Physics.IgnoreCollision(colliderA, colliderB, false);
			
		}
	}

	public static void RestoreCollisionBetween(Collider colliderA, Collider[] collidersB) {
		
		if(colliderA && colliderA.enabled && colliderA.gameObject.activeInHierarchy) {
			foreach(Collider colliderB in collidersB) {
				if(colliderB.enabled && colliderB.gameObject.activeInHierarchy) {
					Physics.IgnoreCollision(colliderA, colliderB, false);
				}
			}
		}
	}

	public static void IgnoreOrRestoreCollisionBetween(Collider colliderA, Collider colliderB, bool ignore) {

		if(colliderA && colliderA.enabled
		   && colliderB && colliderB.enabled) {
			
			Physics.IgnoreCollision(colliderA, colliderB, ignore);
			
		}
	}
}
