using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour {
	
	public Material mat;

	public float minimalLineDistance = .5f;

	protected List<Line> lines = new List<Line>();
	protected Line line_segment;
	
	protected GameObject drawingGO;
	protected bool isDrawing = false;
	public bool isLogging = true;

	public virtual void Start() {

	}

	public void StartDrawingFrom(GameObject gameObj) {
		if(isLogging)
			Debug.Log("[LD] starting drawing..");
		if(isDrawing) {
			Debug.Log("[ERR] Already drawing!");
			return;
		}

		lines.Clear();
		isDrawing = true;
		drawingGO = gameObj;
		line_segment.start = gameObj.transform.position;
	}

	public List<Line> GetFullLine() {
		List<Line> copyOfLines = new List<Line>(lines);
		return copyOfLines;
	}

	public void StopDrawing() {
		if(isLogging)
			Debug.Log("[LD] stopping drawing..");
		this.isDrawing = false;
		drawingGO = null;
		lines.Clear();
	}

	public virtual void ClearAll() {}

	public virtual void OnLinePassed() {}

	public virtual void AddLine(Vector3 newLineEnd) {
		if(isDrawing) {
			if(lines.Count > 0) {
				if(lines[lines.Count - 1].end != Vector3.zero) {
					Vector3 distance = lines[lines.Count - 1].end - newLineEnd;
					if(distance.magnitude < minimalLineDistance) return;
					if(isLogging)
						Debug.Log("adding new line" + newLineEnd);
				}
			}
			line_segment.end = newLineEnd;
			lines.Add(line_segment);
			line_segment.start = line_segment.end;
		}
	}

	public bool HasAnObjectInBetweenLastLinePoint(Vector3 newPosition) {
		if(lines.Count > 0) {
			if(lines[lines.Count - 1].start != Vector3.zero) { 
				Vector3 lastPosition = lines[lines.Count - 1].start;

				GameObject gameObjectInBetween = GameUtils.FindObjectInBetween(lastPosition, newPosition);

				if(gameObjectInBetween) {
					return true;
				}
			}
		}
		return false;
	}
}
