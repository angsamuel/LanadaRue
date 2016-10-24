using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	protected bool passable;
	protected Sprite mySprite;

    protected bool occupied = false;
    protected GameObject occupant;
	protected void Start(){
		mySprite = GetComponent<Sprite> ();
		passable = true;
	}
	public bool IsPassable(){
		return passable;
	}
	public void SetPassable(bool newPassable){
		passable = newPassable;
	}
    public GameObject GetOccupant()
    {
        return occupant;
    }
    public void SetOccupant(GameObject newOccupant)
    {
        occupant = newOccupant;
        occupied = true;
    }
    public void RemoveOccupant()
    {
        occupant = null;
        occupied = false;
    }

}
