using UnityEngine;

public class Node  {
	public Vector3 position;
	public bool isOccupied;
	public float Hcost;
	public float Gcost = float.MaxValue;
	public float Fcost;

	public GridPosition parentGridPosition = new GridPosition();
}
