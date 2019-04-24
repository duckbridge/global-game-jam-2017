using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MoveAndRotationComponent))]
public class PathFollowComponent : DispatchBehaviour {

	protected Transform target;
	protected Vector3 lastDestination;

	protected List<Vector3> pathToFollow = new List<Vector3>();
	protected bool isFollowingPath = false;

	protected MoveAndRotationComponent moveAndRotationComponent;
	
	protected GridMap gridMap; //temporary
	public bool drawPath = true;
	
	public float closeToNodeDistance = .5f;
	public float minimalPositionChangeForRePathing = 5f;

	// Use this for initialization
	public virtual void Start() {
		moveAndRotationComponent = this.GetComponent<MoveAndRotationComponent>();
		gridMap = SceneUtils.FindObject<GridMap>(); //temp!
	}

	public void OnPathFound(List<Vector3> path) {
		if(path == null) {
			//add more?
			return;
		}
		gridMap.RemoveEventListener(this.gameObject);
		pathToFollow.Clear();
		this.pathToFollow.AddRange(path);
		isFollowingPath = true;
	}


	public virtual void SetTarget(Transform target) {
		this.target = target;
	}

	private void LookForNewTarget() {

	}

	// Update is called once per frame
	void Update () { //tweakytweak!
		if(target == null) {
			return;
		}

		//check for closest target every x interval!
		
		Vector3 targetPosition = target.position;

		if((targetPosition - lastDestination).magnitude > minimalPositionChangeForRePathing) {
			lastDestination = targetPosition;

			if(GridMap.isBusy) { Debug.Log("still busy"); return; } //is busy try again later
			gridMap.AddEventListener(this.gameObject);
			gridMap.FindAStarPathBetween(this.transform.position, targetPosition);
		}

		if(pathToFollow.Count > 1) { 
			/*
				Note : 1 is used because when you use 0, it will take the first Vector3 in the path, however
				Because the pathFollower caluclates and moves at the same time, more or less. the 0'th Vector is most likely behind the follower
				which means that he has to go 'back' and then follow the path, this leads to strange behaviour!
			*/
			Vector3 targetPos = new Vector3(pathToFollow[1].x, this.transform.position.y, pathToFollow[1].z);

			moveAndRotationComponent.MoveITween(targetPos);
			moveAndRotationComponent.LookAt(targetPos);
			float distance = Vector3.Distance(targetPos, transform.position);  

            if(distance < closeToNodeDistance) {
                pathToFollow.RemoveAt(0);
                if(drawPath) {
	            	GameObject gcOverlay = (GameObject) GameObject.Instantiate(gridMap.gridCellOverlay, targetPos, Quaternion.identity);
					gcOverlay.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
				}
            }
		}
	}
}
