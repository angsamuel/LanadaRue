using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelControllerScript : MonoBehaviour {
	//controls the generation and control of individual levels
	//number of tiles this level supports
	GameObject tile;
	GameObject playerCharacter;

	float oddColAdjustment = 0;
	float oddRowAdjustment = 0;

	public int mapRows;
	public int mapCols;

	//grid stores a list at each location, which stores all matrixOccupants
	private List<GameObject> [,] levelGrid; 

	public int GetMapRows(){
		return mapRows;
	}
	public int GetMapCols(){
		return mapCols;
	}
		
	void Start () {
		//create levelGrid
		levelGrid = new List<GameObject> [mapRows,mapCols];
		tile = Resources.Load ("Prefabs/Tile") as GameObject;
		playerCharacter = Resources.Load ("Prefabs/PlayerCharacter") as GameObject;
		SpawnTiles();
		SpawnPlayerCharacter ();
	}

	private void SpawnTiles(){
		if (mapCols % 2 == 0) {oddColAdjustment = -.5f;}
		if (mapRows % 2 == 0) {oddRowAdjustment = -.5f;}

		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapCols; c++) {
				GameObject spawnTile = Instantiate (tile, new Vector3 (c - mapCols/2 - oddColAdjustment,r - mapRows/2 - oddRowAdjustment, 0), Quaternion.identity) as GameObject;
			}
		}
	}
	private void SpawnPlayerCharacter(){
		GameObject spawnPlayerCharacter = Instantiate (playerCharacter, new Vector3 (.5f, .5f, -1), Quaternion.identity) as GameObject;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
