using UnityEngine;
using System.Collections;

public class SmallMinimapManager : MonoBehaviour {

	public int maxSize = 5;

	private MinimapBlock[,] smallMinimapGrid;
	private float minimapBlockOffsetMultiplier;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PrepareSmallMinimap(ref MinimapBlock[,] minimapGrid, Vector2 currentGridLocation, float minimapBlockOffsetMultiplier, Color blockColor) {

		this.minimapBlockOffsetMultiplier = minimapBlockOffsetMultiplier;
		smallMinimapGrid = new MinimapBlock[(maxSize*2)+1, (maxSize*2)+1];

		int yIndex = 0;
		int xIndex = 0;

		for(int x = -maxSize; x < maxSize+1; x++) {
			for(int y = -maxSize; y < maxSize+1; y++) {

				MinimapBlock minimapBlockOnGrid = null;

				if((currentGridLocation.x + x) < minimapGrid.GetLength(0) 
				   && (currentGridLocation.y + y) < minimapGrid.GetLength(1) 
				   && currentGridLocation.x + x >= 0
				   && currentGridLocation.y + y >= 0) {

					minimapBlockOnGrid = minimapGrid[(int)currentGridLocation.x + x, (int)currentGridLocation.y + y];
				
				}

				if(minimapBlockOnGrid) {

					SpawnMinimapOnGrid(minimapBlockOnGrid, xIndex, yIndex, x, y, blockColor);

				}
				yIndex++;
			}

			yIndex = 0;
			xIndex++;
		}
	}

	public void UpdateSmallMinimap(Player player, ref MinimapBlock[,] minimapGrid, Vector2 currentGridLocation, Color blockColor) {
		
		int yIndex = 0;
		int xIndex = 0;

		for(int x = -maxSize; x < maxSize+1; x++) {
			for(int y = -maxSize; y < maxSize+1; y++) {

				MinimapBlock minimapBlockOnSmallGrid = smallMinimapGrid[xIndex, yIndex];
				
				if(minimapBlockOnSmallGrid != null) {

					if(currentGridLocation.x + x < minimapGrid.GetLength(0) 
					   && currentGridLocation.y + y < minimapGrid.GetLength(1) 
					   && currentGridLocation.x + x >= 0
					   && currentGridLocation.y + y >= 0) {
						
						MinimapBlock minimapBlockOnGrid = minimapGrid[(int)currentGridLocation.x + x, (int)currentGridLocation.y + y];
						RoomNode roomNodeOfBlockOnGrid = RoomNodeHelper.GetRoomNodeAt(player.GetCurrentTileBlock(), new Vector2(currentGridLocation.x + x, currentGridLocation.y + y));

						if(roomNodeOfBlockOnGrid && roomNodeOfBlockOnGrid.isVisited) {
							if(minimapBlockOnGrid) {
								minimapBlockOnSmallGrid.MakeVisible();
								minimapBlockOnSmallGrid.SetSprite(minimapBlockOnGrid.GetSprite());

								minimapBlockOnGrid.MakeVisible();
							} else {
								minimapBlockOnSmallGrid.MakeHidden();
							}
						} else {
							minimapBlockOnSmallGrid.MakeHidden();
							minimapBlockOnGrid.MakeHidden();
						}
					} else {

						minimapBlockOnSmallGrid.MakeHidden();
					
					}
				
				} else {

					if(currentGridLocation.x + x < minimapGrid.GetLength(0) 
					   && currentGridLocation.y + y < minimapGrid.GetLength(1) 
					   && currentGridLocation.x + x >= 0
					   && currentGridLocation.y + y >= 0) {

						MinimapBlock minimapBlockOnGrid = minimapGrid[(int)currentGridLocation.x + x, (int)currentGridLocation.y + y];
						if(minimapBlockOnGrid) {
						
							SpawnMinimapOnGrid(minimapBlockOnGrid, xIndex, yIndex, x, y, blockColor);

							MinimapBlock spawnedMinimapBlockOnSmallGrid = smallMinimapGrid[xIndex, yIndex];

							spawnedMinimapBlockOnSmallGrid.SetSprite(minimapBlockOnGrid.GetSprite());
						
							spawnedMinimapBlockOnSmallGrid.MakeHidden();
							minimapBlockOnGrid.MakeHidden();

							RoomNode roomNodeOfBlockOnGrid = RoomNodeHelper.GetRoomNodeAt(player.GetCurrentTileBlock(), new Vector2(currentGridLocation.x + x, currentGridLocation.y + y));
							if(roomNodeOfBlockOnGrid && roomNodeOfBlockOnGrid.isVisited) {
								spawnedMinimapBlockOnSmallGrid.MakeVisible();
								minimapBlockOnGrid.MakeVisible();
							}
						}

					}
				}

				yIndex++;
			}

			yIndex = 0;
			xIndex++;
		}
	}

	private void SpawnMinimapOnGrid(MinimapBlock minimapBlockOnGrid,int xIndex, int yIndex, int x, int y, Color blockColor) {

		MinimapBlock minimapBlock = (MinimapBlock) GameObject.Instantiate(Resources.Load(minimapBlockOnGrid.GetFullBlockName(), typeof(MinimapBlock)), 
		                                                                  this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));

		minimapBlock.transform.parent = this.transform;
		minimapBlock.GetComponent<SpriteRenderer>().color = blockColor;
		minimapBlock.transform.localPosition = new Vector3(x * minimapBlockOffsetMultiplier, y * minimapBlockOffsetMultiplier, 0f);
		
		smallMinimapGrid[xIndex, yIndex] = minimapBlock;
	}
}
