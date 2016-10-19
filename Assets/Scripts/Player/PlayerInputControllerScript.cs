using UnityEngine;
using System.Collections;

public class PlayerInputControllerScript : MonoBehaviour {
	GameObject playerCharacterObject;
	string mode;
	// Use this for initialization
	void Start () {
		//link player character to input controller
		mode = "movement";
		playerCharacterObject = GameObject.Find ("PlayerCharacter");
	}
	
	
}
