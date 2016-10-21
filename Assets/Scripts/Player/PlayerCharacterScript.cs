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
    private bool canMoveUpRight = true;
    private bool canMoveUpLeft = true;

    private bool canMoveDownRight = true;
    private bool canMoveDownLeft = true;
    private LevelControllerScript levelControllerScript;

	// Use this for initialization
	void Start () {
        base.Start();
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

            if (Input.GetAxisRaw("MoveUpRight") != 0 && canMoveUpRight) { Move(1, 1); canMoveUpRight = false; }
            if (Input.GetAxisRaw("MoveUpLeft") != 0 && canMoveUpLeft) { Move(-1, 1); canMoveUpLeft = false; }
            if (Input.GetAxisRaw("MoveDownRight") != 0 && canMoveDownRight) { Move(1, -1); canMoveDownRight = false; }
            if (Input.GetAxisRaw("MoveDownLeft") != 0 && canMoveDownLeft) { Move(-1, -1); canMoveDownLeft = false; }

            if (Input.GetAxisRaw("MoveUp") == 0) { canMoveUp = true; }
            if (Input.GetAxisRaw("MoveDown") == 0) {canMoveDown = true; }
            if (Input.GetAxisRaw("MoveLeft") == 0) {canMoveLeft = true; }
            if (Input.GetAxisRaw("MoveRight") == 0) {canMoveRight = true; }
            if (Input.GetAxisRaw("MoveUpRight") == 0) { canMoveUpRight = true; }
            if (Input.GetAxisRaw("MoveDownRight") == 0) { canMoveDownRight = true; }
            if (Input.GetAxisRaw("MoveUpLeft") == 0) { canMoveUpLeft = true; }
            if (Input.GetAxisRaw("MoveDownLeft") == 0) { canMoveDownLeft = true; }
    }
}
