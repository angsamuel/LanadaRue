namespace DungeonGenerator.Java
{
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class DungeonGeneratorScript : MonoBehaviour {
	LevelControllerScript levelControllerScript;
	//max size of map
	private int ymax; //clumns 
	private int xmax; //columns

	//actual size of map
	int _xsize;
	int _ysize;

	//number of objects to generate on the map
	int _objects;
	public const int ChanceRoom = 75;

	//our map
	private GameObject [,] _levelGrid;
	//keep an eye out here
	//readonly IRandomize _rnd;
	//readonly OverflowAction<string> _logger;

	// Use this for initialization
	void Start () {
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		ymax = levelControllerScript.GetMapCols ();
		xmax = levelControllerScript.GetMapRows ();
		_levelGrid = levelControllerScript.GetLevelGrid ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int Corridors
	{
		get;
		private set;
	}

		public static bool IsWall(int x, int y, int xlen, int ylen, int xt, int yt, String direction)
		{
			Func<int, int, int> a = GetFeatureLowerBound;

			Func<int, int, int> b = IsFeatureWallBound;
			switch (direction)
			{
			case "North":
				return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y - ylen + 1;
			case "East":
				return xt == x || xt == x + xlen - 1 || yt == a(y, ylen) || yt == b(y, ylen);
			case "South":
				return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y + ylen - 1;
			case "West":
				return xt == x || xt == x - xlen + 1 || yt == a(y, ylen) || yt == b(y, ylen);
			}

			throw new InvalidOperationException();
		}

		public static int GetFeatureLowerBound(int c, int len)
		{return c - len / 2;}
		public static int IsFeatureWallBound(int c, int len)
		{return c + (len - 1) / 2;}
		public static int GetFeatureUpperBound(int c, int len)
		{return c + (len + 1) / 2;}


}
}