using UnityEngine;
using System.Collections;

public class LivingThingScript : MonoBehaviour {
	private string name;
	private string gender;
    int posX;
    int posY;

    private LevelControllerScript levelControllerScript;
    private GameObject[,] levelGrid;

    // Use this for initialization
    protected void Start () {
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelControllerScript>();
        levelGrid = levelControllerScript.GetLevelGrid();
        Debug.Log(levelGrid.Length);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    protected void Move(int x, int y)
    {
		if (levelGrid [posX + x, posY + y].GetComponent<TileScript> ().IsPassable ()) {
			levelGrid [posX, posY].GetComponent<TileScript> ().RemoveOccupant ();
			posX += x;
			posY += y;
			transform.Translate (new Vector3 (x, y, -1));
			levelGrid [posX, posY].GetComponent<TileScript> ().SetOccupant (GetComponent<GameObject> ());
		}
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
