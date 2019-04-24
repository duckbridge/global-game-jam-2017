using UnityEngine;
using System.Collections;

public class MinimapBuilder : MonoBehaviour {

	public float bigMapBlockScale = .5f;
	public Color blockColor = Color.white;

	public GameObject playerLocationGO;
	public bool enableFOW = true;

	public SmallMinimapManager smallMinimapManager;
	public float minimapBlockOffsetMultiplier = 1f;

	private MinimapBlock[,] minimapGrid;
	private MinimapBlock currentGridBlock;

	private Vector3 originalLocalPosition;
	private bool hasSetOriginalLocalPosition;

	// Use this for initialization
	void Start () {
		SetColorForObjects();
		playerLocationGO.transform.localScale = new Vector3(bigMapBlockScale, bigMapBlockScale, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	private string DecideMinimapBlockName(int x, int y, string roomName) {
		string minimapBlockName = "";

		string[] splittedName = roomName.Split ('/');

		if (roomName.Contains ("default")) {
			minimapBlockName += "default";
		} else {
			if (splittedName.Length == 4) {
				minimapBlockName += splittedName [3];
			}

			if (splittedName.Length == 5) {
				if (roomName.Contains ("villages")) {
					minimapBlockName += "villages/" + splittedName [2] + "/" + splittedName [4];
				} else if (roomName.Contains ("poi")) {
					minimapBlockName += "poi/" + splittedName [4];
				} else {
					minimapBlockName += "arrows/" + splittedName [4];
					if(x == 0 || y == 0) {
						minimapBlockName += "_inverted";
					}
				}
			}
		}

		return minimapBlockName;
	}

	public void CreateMiniMapForGrid(ref TileBlock tileBlock, Vector2 currentGridLocation) {

		if(!hasSetOriginalLocalPosition) {
			hasSetOriginalLocalPosition = true;
			originalLocalPosition = this.transform.localPosition;
		}

		if(minimapGrid != null) {
			for(int x = 0 ; x < minimapGrid.GetLength(0); x++) {
				for(int y = 0 ; y < minimapGrid.GetLength(1); y++) {
					Destroy(minimapGrid[x, y].gameObject);
				}
			}
		}

		minimapGrid = new MinimapBlock[tileBlock.roomNodes.GetLength(0), tileBlock.roomNodes.GetLength(1)];

		for(int x = 0 ; x < minimapGrid.GetLength(0) ; x++) {
			for(int y = 0 ; y < minimapGrid.GetLength(1) ; y++) {

				if(tileBlock.roomNodes[x,y].CanBeVisited()) {

					string minimapBlockName = "Rooms/UI/MinimapBlocks/";

					RoomNode roomNode = tileBlock.roomNodes [x, y];

					if(roomNode.GetTileType().ToString().Contains(TileType.dungeon.ToString())) {
						minimapBlockName += "dungeon/";
					} else {
						minimapBlockName += "outside/";
					}

					minimapBlockName += DecideMinimapBlockName (x, y, roomNode.roomPrefix);

					Logger.Log (minimapBlockName);

					MinimapBlock minimapBlock = (MinimapBlock) GameObject.Instantiate(Resources.Load(minimapBlockName, typeof(MinimapBlock)), 
					                                                                  this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));

					minimapBlock.SetFullBlockName(minimapBlockName);
					minimapBlock.GetComponent<SpriteRenderer>().color = blockColor;
					minimapBlock.MakeHidden();

					if(!enableFOW || tileBlock.roomNodes[x, y].isVisited) {
						minimapBlock.MakeVisible();
					}

					minimapBlock.transform.parent = this.transform;

					minimapBlock.transform.localPosition = new Vector3(x * (minimapBlockOffsetMultiplier * bigMapBlockScale), 0f, y * (minimapBlockOffsetMultiplier * bigMapBlockScale));
					minimapBlock.transform.localScale = new Vector3(bigMapBlockScale, bigMapBlockScale, 1f);

					minimapGrid[x, y] = minimapBlock;
			
				}
			}
		}

        if(currentGridLocation.x < minimapGrid.GetLength(0) && currentGridLocation.y < minimapGrid.GetLength(1)) {
    		currentGridBlock = minimapGrid[(int)currentGridLocation.x, (int)currentGridLocation.y];
    		playerLocationGO.transform.localPosition = new Vector3(currentGridBlock.transform.localPosition.x, currentGridBlock.transform.localPosition.y, currentGridBlock.transform.localPosition.z -0.2f);
        }   

		Vector2 minimapSize = new Vector2((minimapBlockOffsetMultiplier * bigMapBlockScale) * tileBlock.roomNodes.GetLength(0), (minimapBlockOffsetMultiplier  * bigMapBlockScale) * tileBlock.roomNodes.GetLength(1));

		this.transform.localPosition = originalLocalPosition - new Vector3(minimapSize.x/2, minimapSize.y/2, 0f);

	}

	public void SetCurrentGrid(Player player, Vector2 gridLocation) {

		currentGridBlock = minimapGrid[(int)gridLocation.x, (int)gridLocation.y];

		playerLocationGO.transform.localPosition = new Vector3(currentGridBlock.transform.localPosition.x, currentGridBlock.transform.localPosition.y, currentGridBlock.transform.localPosition.z -0.2f);

		smallMinimapManager.UpdateSmallMinimap(player, ref minimapGrid, gridLocation, blockColor);
	}

	public void PrepareSmallMinimap(Vector2 gridLocation) {
		smallMinimapManager.PrepareSmallMinimap(ref minimapGrid, gridLocation, minimapBlockOffsetMultiplier, blockColor);
	}

	public void SetColorForObjects() {
		SceneUtils.FindObjects<ObjectThatUsesMinimapColor>().ForEach(objThatUsesMinimapColor => objThatUsesMinimapColor.SetColor(blockColor));
	}
}
