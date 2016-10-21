using UnityEngine;
using System.Collections;

public class PlayerCharacterScript : HumanScript {
	//position on level grid
	private int posX;
	private int posY;
	private LevelControllerScript levelControllerScript;

	// Use this for initialization
	void Start () {
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
