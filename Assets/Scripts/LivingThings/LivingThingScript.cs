using UnityEngine;
using System.Collections;

public class LivingThingScript : MonoBehaviour {
	protected int ap;


	//stats
	protected int speed; 


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
		mapCols = levelControllerScript.GetMapCols();
		mapRows = levelControllerScript.GetMapRows ();
    }

	// Update is called once per frame
	void Update () {
	
	}

	public void GiveAp(int elapsedTime, int actionCost){
		ap += speed * (elapsedTime / actionCost);
	}

    protected void Move(int x, int y)
    {
		levelGrid = levelControllerScript.GetLevelGrid();
		if (posX + x >= 0 && posX + x <= mapCols && posY + y >= 0 && posY + y <= mapRows && levelGrid [posX + x, posY + y] != null) {
		Debug.Log( (posX)  + ", " + (posY));
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

	virtual public void AIMove(){}
    public void SetPosVariables(int x, int y)
    {
        posX = x;
        posY = y;
    }
	//GET FUNCTIONS
	public string GetName(){return name;}
	public string GetGender(){return gender;}
}
