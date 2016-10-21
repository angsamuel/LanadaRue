using UnityEngine;
using System.Collections;

public class PlayerCharacterScript : HumanScript {
	//position on level grid
	private int posX;
	private int posY;

    private bool canMoveUp = true;
    private bool canMoveDown = true;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private LevelControllerScript levelControllerScript;

	// Use this for initialization
	void Start () {
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
        ScanForMovementInputs();
    }

    void ScanForMovementInputs()
    {

            if (Input.GetAxisRaw("MoveUp") != 0 && canMoveUp) { Move(0, 1); canMoveUp = false; }
            if (Input.GetAxisRaw("MoveDown") != 0 && canMoveDown) { Move(0, -1); canMoveDown = false; }
            if (Input.GetAxisRaw("MoveLeft") != 0 && canMoveLeft) { Move(-1, 0); canMoveLeft = false; }
            if (Input.GetAxisRaw("MoveRight") != 0 && canMoveRight) { Move(1, 0); canMoveRight = false; }

            if (Input.GetAxisRaw("MoveUp") == 0) { canMoveUp = true; }
            if (Input.GetAxisRaw("MoveDown") == 0) {canMoveDown = true; }
            if (Input.GetAxisRaw("MoveLeft") == 0) {canMoveLeft = true; }
            if (Input.GetAxisRaw("MoveRight") == 0) {canMoveRight = true; }

      
    }
}
