using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	private string name;

	private int posX;
	private int posY;

	private int weight;
	private int size;
	private int rarity;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public string GetName(){
		return name;
	}
	public void SetName(string newName){
		name = newName;
	}
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }
    public int GetPosX()
    {
        return posX;
    }
    public int GetPosY()
    {
        return posY;
    }

}
