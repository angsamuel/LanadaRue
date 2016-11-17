using UnityEngine;
using System.Collections;

public class LivingThingScript : MonoBehaviour {
	private string name;
	private string gender;
    protected int posX;
    protected int posY;
	protected int mapCols;
	protected int mapRows;

    private LevelControllerScript levelControllerScript;
    protected GameObject[,] levelGrid;

    // Use this for initialization
    protected void Start () {
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelControllerScript>();
        levelGrid = levelControllerScript.GetLevelGrid();
        Debug.Log(levelGrid.Length);
		mapCols = levelControllerScript.GetMapCols();
		mapRows = levelControllerScript.GetMapRows ();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    protected void Move(int x, int y)
    {

		levelGrid = levelControllerScript.GetLevelGrid();
		if (posX + x >= 0 && posX + x <= mapCols && posY + y >= 0 && posY + y <= mapRows && levelGrid [posX + x, posY + y] != null) {
			if (levelGrid [posX + x, posY + y].GetComponent<TileScript> ().IsPassable ()) {
				levelGrid [posX, posY].GetComponent<TileScript> ().RemoveOccupant ();
				posX += x;
				posY += y;
				transform.Translate (new Vector3 (x, y, 0));
				levelGrid [posX, posY].GetComponent<TileScript> ().SetOccupant (GetComponent<GameObject> ());
			}
		}
	}
	public Vector3 CoordToVector3(int x ,int y, int z){
		return new Vector3 (x - mapCols / 2 + .5f, y - mapRows / 2 + .5f, z);
	}
	protected void Teleport(int x, int y){
		levelGrid [posX, posY].GetComponent<TileScript> ().RemoveOccupant ();
		posX = x;
		posY = y;
		transform.position =  (CoordToVector3(posX, posY, -1));
		levelGrid [posX, posY].GetComponent<TileScript> ().SetOccupant (GetComponent<GameObject> ());
	}


    public void SetPosVariables(int x, int y)
    {
        posX = x;
        posY = y;
    }
	//GET FUNCTIONS
	public string GetName(){return name;}
	public string GetGender(){return gender;}
}
