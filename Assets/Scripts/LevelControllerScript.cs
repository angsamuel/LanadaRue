using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelControllerScript : MonoBehaviour {
	//controls the generation and control of individual levels
	//number of tiles this level supports
	GameObject tile;

	int mapRows = 30;
	int mapCols = 30;

	//grid stores a list at each location, which stores all matrixOccupants
	private List<GameObject> [,] levelGrid; 

	void Start () {
		//create levelGrid
		levelGrid = new List<GameObject> [mapRows,mapCols];
		tile = Resources.Load ("Prefabs/Environment/Tile") as GameObject;
		spawnTiles();
	}

	public void spawnTiles(){
		float oddColAdjustment = 0;
		float oddRowAdjustment = 0;
		if (mapCols % 2 == 0) {oddColAdjustment = -.5f;}
		if (mapRows % 2 == 0) {oddRowAdjustment = -.5f;}

		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapCols; c++) {
				GameObject spawnTile = Instantiate (tile, new Vector3 (c - mapCols/2 - oddColAdjustment,r - mapRows/2 - oddRowAdjustment, 0), Quaternion.identity) as GameObject;
					//Instantiate (forrest, freeCoordinates [0], Quaternion.identity) as GameObject;
			}
		}
		//GameObject spawnTile = Instantiate (tile, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
