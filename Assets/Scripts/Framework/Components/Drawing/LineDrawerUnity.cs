using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawerUnity : LineDrawer {

	public LineSegment lineSegment;
	protected List<LineSegment> lineSegments;

	// Use this for initialization
	public override void Start () {
		base.Start();
		lineSegments = new List<LineSegment>();
	}

	// Update is called once per frame
	void Update () {}

	public override void ClearAll() {
		for(int i = 0; i < lineSegments.Count; i++) {
			Destroy(lineSegments[i].gameObject);
		}
		lineSegments.Clear();
	}

	void OnDraw() {
		ClearAll();
		for(int i = 0; i < lines.Count ; i++) {
			LineSegment newLineSegment = (LineSegment) GameObject.Instantiate(lineSegment, lines[i].end, Quaternion.identity);
			newLineSegment.transform.parent = this.transform.Find("Lines");
			lineSegments.Add(newLineSegment);
		}
	}

	private void ClearLine(int index) {

		LineSegment lineSeg = lineSegments[index];
		lineSegments.RemoveAt(index);

		Destroy(lineSeg.gameObject);
	}

	public override void OnLinePassed() {
		ClearLine(0);
	}

	public override void AddLine(Vector3 newLineEnd) {
		base.AddLine(newLineEnd);
		OnDraw();
	}
}
