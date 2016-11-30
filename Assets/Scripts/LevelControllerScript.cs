using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LitJson;

public class LevelControllerScript : MonoBehaviour {
	//controls the generation and control of individual levels
	//number of tiles this level supports
	GameObject unusedTile;
	GameObject wall;
	GameObject playerCharacter;
	GameObject levelGenerator;
    GameObject dumbGen;
    List<GameObject> itemPool = new List<GameObject>();
	GameObject camera;

	public LevelGeneratorDefaultScript lgScript;

	SlumsLevelGeneratorScript slumsLevelGeneratorScript;

	float oddColAdjustment = 0;
	float oddRowAdjustment = 0;

	public int mapRows;
	public int mapCols;

	List<GameObject> npcList = new List<GameObject>();

	//grid stores a list at each location, which stores all matrixOccupants
	private GameObject [,] levelGrid; 

	public LevelGeneratorDefaultScript GetLGScript(){
		return lgScript;
	}

	public List<GameObject> GetNpcList(){
		return npcList;
	}

	public int GetMapRows(){
		return mapRows;
	}
	public int GetMapCols(){
		return mapCols;
	}
		
	void Start () {
		camera = GameObject.Find ("Main Camera");
		//create levelGrid
		levelGrid = new GameObject [mapCols,mapRows];

		//load prefabs
		unusedTile = Resources.Load ("Prefabs/Environment/Unused") as GameObject;
		wall = Resources.Load ("Prefabs/Environment/Wall") as GameObject;
		playerCharacter = Resources.Load ("Prefabs/PlayerCharacter") as GameObject;
		if (mapCols % 2 == 0) {oddColAdjustment = -.5f;}
		if (mapRows % 2 == 0) {oddRowAdjustment = -.5f;}
		SpawnTiles();

		SpawnPlayerCharacter (mapCols/2,mapRows/2);

		GameObject slumsGeneratorPref = Resources.Load ("Prefabs/LevelGenerators/SlumsLevelGenerator") as GameObject;
        dumbGen = Resources.Load("Prefabs/LevelGenerators/DungeonGenerator") as GameObject;
		levelGenerator = Resources.Load("Prefabs/LevelGenerators/LevelGeneratorDefault") as GameObject;
		GameObject lg = Instantiate (levelGenerator, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		lgScript = lg.GetComponent<LevelGeneratorDefaultScript>();





	}

	public void FillNPCList(){
		//ensure all living things are spawned FIRST
		npcList.AddRange(GameObject.FindGameObjectsWithTag ("NPC"));
		Debug.Log ("npcs found: " + npcList.Count);
	}

	private void SpawnTiles(){
		//prep work
	}
	//converts coordinate in grid to location in scene
	public Vector3 CoordToVector3(int x ,int y, int z){
		return new Vector3 (x - mapCols / 2 - oddColAdjustment, y - mapRows / 2 - oddRowAdjustment, z);
	}

	public void SpawnLivingThing(GameObject lt, int x, int y){
		GameObject spawnLt = Instantiate (lt, CoordToVector3(x,y,-1), Quaternion.identity) as GameObject;
		spawnLt.GetComponent<LivingThingScript>().SetPosVariables(x,y);
	}

    //fix this
	private void SpawnPlayerCharacter(int x, int y){
		GameObject spawnPlayerCharacter = Instantiate (playerCharacter, CoordToVector3(x,y,-1), Quaternion.identity) as GameObject;
		spawnPlayerCharacter.GetComponent<PlayerCharacterScript> ().SetPosVariables (x, y);
	}
	//replaces tile in level grid
	public void ReplaceTile(int x, int y, GameObject newTile){
		if (levelGrid [x, y] != null) {
			Destroy (levelGrid [x, y]);
		}
		levelGrid [x, y] = Instantiate (newTile, CoordToVector3(x,y, 0), Quaternion.identity) as GameObject;
	}

    public void SpawnItem(int x, int y, GameObject newItem)
    {
        Debug.Log("SpawneItem called");
       GameObject spawnedItem = Instantiate(newItem, CoordToVector3(x, y, -1), Quaternion.identity) as GameObject;
       spawnedItem.GetComponent<ItemScript>().SetPosition(x, y);
       itemPool.Add(spawnedItem);
    }

    public GameObject[,] GetLevelGrid()
    {
        return levelGrid;
        Debug.Log("got level grid");
    }
	public void SetLevelGrid(GameObject[,] lg){
		levelGrid = lg;
	}
    public List<GameObject> GetItemPool()
    {
        return itemPool;
    }
	public void ReplaceTileSprite(int x, int y, Sprite newSprite){
		levelGrid [x, y].GetComponent<SpriteRenderer> ().sprite = newSprite;
	}
}
