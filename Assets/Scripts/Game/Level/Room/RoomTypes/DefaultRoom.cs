using UnityEngine;
using System.Collections;

public class DefaultRoom : Room {

	public override void Initialize (RoomNode roomNode) {
		base.Initialize (roomNode);
		isDefaultRoom = true;
	}

}
