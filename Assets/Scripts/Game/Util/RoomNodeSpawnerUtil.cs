using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeSpawnerUtil : MonoBehaviour {

	private int xMin, xMax, yMin, yMax;
	private int x, y;
	private Player player;
	private float timeBetweenRoomSpawn = 0.5f;
	private int spawnCount = 0;

	private void SkipToNextRoom() {
		CountUp ();
		SpawnNextRoom ();
	}

	private void CountUp() {
		spawnCount++;
		if (x < xMax) {
			x++;
		}

		if (x == xMax) {
			y++;
			x = xMin;
		}
	}

	private bool IsDoneSpawning() {
		return spawnCount >= 9;
	}

	private bool IsOutOfRange(RoomNode[,] roomNodes) {
		return (x < 0 || y < 0 || x >= roomNodes.GetLength (0) || y >= roomNodes.GetLength (1));
	}

	private void SpawnNextRoom() {

		if (IsDoneSpawning()) { //done spawning
			return;	
		}

		RoomNode[,] roomNodes = player.GetCurrentTileBlock ().roomNodes;

		if(IsOutOfRange(roomNodes)) {
			SkipToNextRoom ();
		} else {
			RoomNode roomToSpawn = roomNodes[x, y];

			string[] splittedRoomPrefix = roomToSpawn.roomPrefix.Split ('/');
			string roomNodeName = splittedRoomPrefix [splittedRoomPrefix.Length - 1];

			if (roomToSpawn.GetRoom () != null || roomNodeName.Equals ("default")) {
				SkipToNextRoom ();
			} else {
				RoomNodeSpawnerUtil.SpawnRoom (roomToSpawn);
				CountUp ();

				if (!IsDoneSpawning()) {
					if (timeBetweenRoomSpawn <= 0f) {
						SpawnNextRoom ();
					} else {
						Invoke ("SpawnNextRoom", timeBetweenRoomSpawn);
					}
				}
			}
		}
	}

	public void SpawnRoomsAroundPlayer(ref Player player, float timeBetweenRoomSpawn) {

		this.spawnCount = 0;
		this.timeBetweenRoomSpawn = timeBetweenRoomSpawn;
		this.player = player;

		Vector2 playerLocation = player.GetCurrentRoomNode ().gridLocation;
		Logger.Log (playerLocation);

		xMin = (int)playerLocation.x - 1;
		yMin = (int)playerLocation.y - 1;

		xMax = (int)playerLocation.x + 2;
		yMax = (int)playerLocation.y + 2;

		x = xMin;
		y = yMin;

		SpawnNextRoom ();
	}

	public static void SpawnRoom(RoomNode roomNode) {

		if (roomNode.GetRoom () != null) {
			Logger.Log ("roomNode already has room!");
			return;
		}

		if (roomNode.name.Equals ("default")) {
			Logger.Log ("dont spawn default room");
			return;
		}

		Room newRoom = (Room) GameObject.Instantiate(Resources.Load(roomNode.roomPrefix, typeof(Room)), roomNode.transform.position, Quaternion.identity);
		newRoom.name = roomNode.roomPrefix;
		newRoom.Initialize(roomNode);

		newRoom.transform.parent = roomNode.transform;

		newRoom.SpawnCorrectGroundTile();
		roomNode.SetRoom(newRoom);

	}
}
