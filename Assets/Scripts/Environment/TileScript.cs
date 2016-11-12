using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	protected bool passable;
	protected Sprite mySprite;

	public float minRandScale =  0.0f;
	public float maxRandScale = 0.0f;

	public string id;

    protected bool occupied = false;
    protected GameObject occupant;

	protected void Start(){
		mySprite = GetComponent<Sprite> ();
		passable = true;

		//for aesthetic
		float ran = Random.Range (minRandScale, maxRandScale);
		transform.localScale += new Vector3(ran, ran, 0);
	}
	public bool IsPassable(){
		if (occupied) {
			//more complicated than this
			return false;
		}
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
