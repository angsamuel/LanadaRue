using UnityEngine;
using System.Collections;

public class VagrantScript : HumanScript {

	// Use this for initialization
	void Start () {
		base.Start ();
		speed = 10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void AIMove(){
		while (ap >= 100) {
			Move (Random.Range(-1,2),Random.Range(-1,2));
			ap -= 100;
		}

	}
}
