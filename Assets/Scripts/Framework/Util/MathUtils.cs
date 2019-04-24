using UnityEngine;
using System.Collections;

public class MathUtils {

	public static float GetDistance2D(Vector3 target, Vector3 origin) {
		return Vector2.Distance(new Vector2(target.x, target.z), new Vector2(origin.x, origin.z));
	}

	public static Vector3 CalculateDirection(Vector3 target, Vector3 origin) {
		Vector3 direction = target - origin;
		direction.Normalize();
		return direction;
	}

	public static Vector2 CalculateDirection2D(Vector3 target, Vector3 origin) {

		Vector3 direction = new Vector3(target.x - origin.x, target.y - origin.y, 0f);
		direction.Normalize();

		return direction;
	}


	public static Direction GetDirection(Vector2 target, Vector2 origin) {
		
		bool isLookingUpDown = false;
		
		if(Mathf.Abs(target.x - origin.x) < Mathf.Abs(target.y - origin.y)) {
			isLookingUpDown = true;
		}
		
		if(!isLookingUpDown) {
			
			if(target.x >= origin.x) {
				
				return Direction.RIGHT;
				
			} else {
				
				return Direction.LEFT;
				
			}
			
		} else {
			
			if(target.y >= origin.y) {
				
				return Direction.UP;
				
			} else {
				
				return Direction.DOWN;
				
			}
		}
	}

	public static Direction GetHorizontalDirection(Vector3 target, Vector3 origin) {

		float direction = target.x - origin.x;

		if(direction > 0) {
			return Direction.RIGHT;
		} else {
			return Direction.LEFT;
		}
	}

	public static Direction GetVerticalDirection(Vector3 target, Vector3 origin) {
		
		float direction = target.y - origin.y;
		
		if(direction > 0) {
			return Direction.UP;
		} else {
			return Direction.DOWN;
		}
	}

	public static Vector2 CalculateDirection(Vector2 target, Vector2 origin) {
		Vector2 direction = target - origin;
		direction.Normalize();
		return direction;
	}

	public static float CalculcateWeightedDistance(Vector2 target, Vector2 origin, float weightX = 1f, float weightY = 1f) {
		return Vector2.Distance(new Vector2(target.x * weightX, target.y * weightY), origin);
	}

	public static bool IsWithinBounds(Bounds checkedBounds, Bounds withinBounds) {
		return checkedBounds.Intersects(withinBounds);
	}

	public static Direction GetInverse(Direction originalDirection) {
		
		if(originalDirection == Direction.RIGHT) {
			return Direction.LEFT;
		}
		
		if(originalDirection == Direction.LEFT) {
			return Direction.RIGHT;
		}
		
		if(originalDirection == Direction.UP) {
			return Direction.DOWN;
		}
		
		if(originalDirection == Direction.DOWN) {
			return Direction.UP;
		}

		return Direction.NONE;
	}
	
	public static Direction GetLeftOf(Direction originalDirection) {
		
		if(originalDirection == Direction.RIGHT) {
			return Direction.UP;
		}
		
		if(originalDirection == Direction.LEFT) {
			return Direction.DOWN;
		}
		
		if(originalDirection == Direction.UP) {
			return Direction.LEFT;
		}
		
		if(originalDirection == Direction.DOWN) {
			return Direction.RIGHT;
		}

		return Direction.NONE;
	}
	
	public static Direction GetRightOf(Direction originalDirection) {
		
		if(originalDirection == Direction.RIGHT) {
			return Direction.DOWN;
		}
		
		if(originalDirection == Direction.LEFT) {
			return Direction.UP;
		}
		
		if(originalDirection == Direction.UP) {
			return Direction.RIGHT;
		}
		
		if(originalDirection == Direction.DOWN) {
			return Direction.LEFT;
		}

		return Direction.NONE;
	}

	
	public static Bounds OrthographicBounds(Camera camera) {
		
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, 0, cameraHeight));
		
		return bounds;
	}

	public static Direction Vector2AsDirection(Vector2 direction) {

		Direction directionToReturn = Direction.NONE;

		if(direction == new Vector2(-1, 0)) {
			
			directionToReturn = Direction.LEFT;
			
		} else if(direction == new Vector2(1, 0)) {
			
			directionToReturn = Direction.RIGHT;
			
		} else if(direction == new Vector2(0, 1)) {
			
			directionToReturn = Direction.UP;
			
		} else if(direction == new Vector2(0, -1)) {
			
			directionToReturn = Direction.DOWN;
			
		}
		
		return directionToReturn;
	}

	public static Vector3 GetDirectionAsVector3(Direction direction) {

		Vector3 directionToReturn = Vector3.zero;

		if(direction == Direction.LEFT) {
			
			directionToReturn = new Vector3(-1f, 0f, 0f);
			
		} else if(direction == Direction.RIGHT) {
			
			directionToReturn = new Vector3(1f, 0f, 0f);
			
		} else if(direction == Direction.UP) {
			
			directionToReturn = new Vector3(0, 0f, 1f);
			
		} else if(direction == Direction.DOWN) {
			
			directionToReturn = new Vector3(0f, 0f, -1f);
			
		}

		return directionToReturn;
	}

	public static Vector2 GetDirectionAsVector2(Direction direction) {

		Vector2 directionToReturn = Vector2.zero;
		
		if(direction == Direction.LEFT) {
			
			directionToReturn = new Vector3(-1f, 0f);
			
		} else if(direction == Direction.RIGHT) {
			
			directionToReturn = new Vector3(1f, 0f);
			
		} else if(direction == Direction.UP) {
			
			directionToReturn = new Vector3(0, 1f);
			
		} else if(direction == Direction.DOWN) {
			
			directionToReturn = new Vector3(0f, -1f);
			
		}
		
		return directionToReturn;

	}

	public static int GetManhattanDistance(Vector2 positionB, Vector2 positionA) {
		int manhattanDistance = Mathf.Abs((int) (positionB.x - positionA.x)) + Mathf.Abs((int) (positionB.y - positionA.y));

		return manhattanDistance;
	}

	public static Direction GetRandomDirection() {

		int randomDir = Random.Range (0, 4);

		if(randomDir == 0) {
			return Direction.DOWN;
		} else if(randomDir == 1) {
			return Direction.UP;
		} else if(randomDir == 2) {
			return Direction.RIGHT;
		} else if(randomDir == 3) {
			return Direction.LEFT;
		}
		
		return Direction.RIGHT;
	}
}
