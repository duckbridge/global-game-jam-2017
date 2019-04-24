using UnityEngine;

public struct GridPosition { //change later!	
	
	public static GridPosition Zero = new GridPosition {x = 0, y = 0};

	public int x;
	public int y;
	
	public GridPosition(int x, int y) {
		this.x = x;
		this.y = y;
	}
	
	//Manhattan distance to another cell
	public int Distance(GridPosition other) {
		return Mathf.Abs(other.x - x) + Mathf.Abs(other.y - y);
	}
	
	//Add two grid positions
	public static GridPosition operator + (GridPosition p1, GridPosition position) {
		return new GridPosition { x = p1.x + position.x, y = p1.y + position.y };
	}

	//Check if two grid positions are equal
	public override bool Equals (object obj) {
		if(!(obj is GridPosition))
			return false;
		var gp = (GridPosition)obj;
		return gp.x == x && gp.y == y;
	}
} 