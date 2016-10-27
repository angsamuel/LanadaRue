using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//spawned in by level controller
public class SlumsLevelGeneratorScript : MonoBehaviour {
	GameObject levelController;

	private LevelControllerScript levelControllerScript; 
	private int mapRows;
	private int mapCols;

	//size of object grid
	public int numberOfHouses = 1;
	public int minPlotSize = 10;
	public int maxPlotSize = 15;

	private GameObject woodTile;


	// Use this for initialization
	void Start () {
		woodTile = Resources.Load ("Prefabs/Environment/WoodWall") as GameObject;
		levelController = GameObject.Find ("LevelController") as GameObject;
		levelControllerScript = levelController.GetComponent<LevelControllerScript>();
		mapRows = levelControllerScript.GetMapRows();
		mapCols = levelControllerScript.GetMapCols();
		levelControllerScript.ReplaceTile (0, 0, woodTile);
		GenerateLevel ();
	}

	public void GenerateLevel(){
		Debug.Log ("Generating level...");
		for (int i = 0; i < numberOfHouses; ++i) {
			SpawnHouseInLevel ();
		}
	}

	private void SpawnHouseInLevel(){
		levelController = GameObject.Find ("LevelController") as GameObject;
		levelControllerScript = levelController.GetComponent<LevelControllerScript>();

		Debug.Log (mapRows);
		int houseWidth = Random.Range (10, 15);
		int houseHeight = Random.Range (10, 15);

		int plotTopLeftCornerX = Random.Range (0, mapCols - (houseWidth + 1));
		int plotTopLeftCornerY = Random.Range (0+houseHeight, mapRows);

		int orientation = Random.Range (0, 4);

		int enterWidth = Random.Range (3, 5);
		int enterHeight = Random.Range (3, 5);
		/*
		for (int w = 0; w < enterHeight; ++w) {
			for (int h = 0; h < enterHeight; ++h) {
				levelControllerScript.ReplaceTile (w, h, woodTile);
			}
		}*/
		for (int i = 0; i < 100; ++i) {
			levelControllerScript.ReplaceTile (Random.Range(0,mapCols), Random.Range(0,mapRows), woodTile);
		}

		levelControllerScript.ReplaceTile (5, 5, woodTile);


	}
		
}