using UnityEngine;
using System.Collections;

public class PlayerCharacterScript : HumanScript {

    private bool canMoveUp = true;
    private bool canMoveDown = true;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private bool canMoveUpRight = true;
    private bool canMoveUpLeft = true;
    private bool canMoveDownRight = true;
    private bool canMoveDownLeft = true;

	public static int levelX = 0;
	public static int levelY = 0;

	private string cameraMode;

	GameObject camera;

    private LevelControllerScript levelControllerScript;

	// Use this for initialization
	void Start () {
		cameraMode = "locked_camera";
        base.Start();
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		camera = GameObject.Find ("Main Camera");
		camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        ScanForMovementInputs();
    }

    void ScanForMovementInputs()
    {
        if (Input.GetAxisRaw("MoveUp") != 0 && canMoveUp) { PlayerMove(0, 1); canMoveUp = false; }
		if (Input.GetAxisRaw("MoveDown") != 0 && canMoveDown) { PlayerMove(0, -1); canMoveDown = false; }
		if (Input.GetAxisRaw("MoveLeft") != 0 && canMoveLeft) { PlayerMove(-1, 0); canMoveLeft = false; }
		if (Input.GetAxisRaw("MoveRight") != 0 && canMoveRight) { PlayerMove(1, 0); canMoveRight = false; }

		if (Input.GetAxisRaw("MoveUpRight") != 0 && canMoveUpRight) { PlayerMove(1, 1); canMoveUpRight = false; }
		if (Input.GetAxisRaw("MoveUpLeft") != 0 && canMoveUpLeft) { PlayerMove(-1, 1); canMoveUpLeft = false; }
		if (Input.GetAxisRaw("MoveDownRight") != 0 && canMoveDownRight) { PlayerMove(1, -1); canMoveDownRight = false; }
		if (Input.GetAxisRaw("MoveDownLeft") != 0 && canMoveDownLeft) { PlayerMove(-1, -1); canMoveDownLeft = false; }

            if (Input.GetAxisRaw("MoveUp") == 0) { canMoveUp = true; }
            if (Input.GetAxisRaw("MoveDown") == 0) {canMoveDown = true; }
            if (Input.GetAxisRaw("MoveLeft") == 0) {canMoveLeft = true; }
            if (Input.GetAxisRaw("MoveRight") == 0) {canMoveRight = true; }
            if (Input.GetAxisRaw("MoveUpRight") == 0) { canMoveUpRight = true; }
            if (Input.GetAxisRaw("MoveDownRight") == 0) { canMoveDownRight = true; }
            if (Input.GetAxisRaw("MoveUpLeft") == 0) { canMoveUpLeft = true; }
            if (Input.GetAxisRaw("MoveDownLeft") == 0) { canMoveDownLeft = true; }
    }

	void PlayerMove(int x, int y){
		//generate new level
		if (posX + x < 0 || posY + y < 0 || posX + x >= mapCols || posY + y >= mapRows) {
			if (posY + y < 0) {
				levelY -= 1;
				Teleport (posX, mapRows - (posY) - 1);
			} else if (posX + x < 0) {
				levelX -= 1;
				Teleport (mapCols - (posX) - 1, posY);
			} else if (posY + y >= mapRows) {
				levelY += 1;
				Teleport (posX, 0);
			} else if (posX + x >= mapCols) {
				levelX += 1;
				Teleport (0, posY);
			}
			camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);			
			levelControllerScript.GetLGScript ().ChangeLevel (levelX, levelY);
			Debug.Log ("Moving to level " + levelX + ", "+ levelY);
		} else {
			Move (x, y);
		}
		if (cameraMode == "locked_camera") {
			camera.transform.position = new Vector3 (transform.position.x, transform.position.y, camera.transform.position.z);
		}
	}
}
