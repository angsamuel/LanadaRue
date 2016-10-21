using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	protected bool passable;
	protected Sprite mySprite;
	void Start(){
		mySprite = GetComponent<Sprite> ();
	}
	protected bool IsPassable(){
		return passable;
	}
	protected void SetPassable(bool newPassable){
		passable = newPassable;
	}
}
