using UnityEngine;
using System.Collections;

public class LivingThingScript : MonoBehaviour {
	private string name;
	private string gender;
    int posX;
    int posY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected void Move(int x, int y)
    {
        posX += x;
        posY += y;
        transform.Translate(new Vector3(x, y, -1));

    }

	//GET FUNCTIONS
	public string GetName(){return name;}
	public string GetGender(){return gender;}
}
