using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelControllerScript : MonoBehaviour {
	//controls the generation and control of individual levels
	//number of tiles this level supports
	GameObject tile;
	GameObject wall;
	GameObject playerCharacter;

	float oddColAdjustment = 0;
	float oddRowAdjustment = 0;

	public int mapRows;
	public int mapCols;

	//grid stores a list at each location, which stores all matrixOccupants
	private GameObject [,] levelGrid; 

	public int GetMapRows(){
		return mapRows;
	}
	public int GetMapCols(){
		return mapCols;
	}
		
	void Start () {
		//create levelGrid
		levelGrid = new GameObject [mapCols,mapRows];
		tile = Resources.Load ("Prefabs/Environment/Tile") as GameObject;
		wall = Resources.Load ("Prefabs/Environment/Wall") as GameObject;
		playerCharacter = Resources.Load ("Prefabs/PlayerCharacter") as GameObject;
		if (mapCols % 2 == 0) {oddColAdjustment = -.5f;}
		if (mapRows % 2 == 0) {oddRowAdjustment = -.5f;}
		SpawnTiles();
		SpawnPlayerCharacter ();
	}

	private void SpawnTiles(){
		

		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapCols; c++) {
				GameObject spawnTile = Instantiate (tile, CoordToVector3(c, r, 0), Quaternion.identity) as GameObject;
                levelGrid[c, r] = spawnTile;

            }
		}
		GameObject spawnWall = Instantiate (wall, new Vector3 (0 - mapCols/2 - oddColAdjustment,0 - mapRows/2 - oddRowAdjustment, 0), Quaternion.identity) as GameObject;
		Destroy (levelGrid [0, 0]);
		levelGrid[0, 0] = spawnWall;
	}
	//converts coordinate in grid to location in scene
	public Vector3 CoordToVector3(int x ,int y, int z){
		return new Vector3 (x - mapCols / 2 - oddColAdjustment, y - mapRows / 2 - oddRowAdjustment, z);
	}
		
    //fix this
	private void SpawnPlayerCharacter(){
		GameObject spawnPlayerCharacter = Instantiate (playerCharacter, new Vector3 (.5f, .5f, -1), Quaternion.identity) as GameObject;
        spawnPlayerCharacter.GetComponent<PlayerCharacterScript>().SetPosVariables(mapCols/2, mapRows/2);
        
	}
	//replaces tile in level grid
	public void ReplaceTile(int x, int y, GameObject newTile){
		Destroy (levelGrid [x, y]);
		levelGrid [x, y] = Instantiate (newTile, new Vector3 (0 - mapCols/2 - oddColAdjustment,0 - mapRows/2 - oddRowAdjustment, 0), Quaternion.identity) as GameObject;
		;
	}
    public GameObject[,] GetLevelGrid()
    {
        return levelGrid;
        Debug.Log("got level grid");
    }

	// Update is called once per frame
	void Update () {
	
	}
}
