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

	//direction
	public enum Direction {NORTH, SOUTH, EAST, WEST};

	//point
		public class PointI{
			public int X;
			public int Y;
		}

	//tiles
	GameObject corridorTile; 

	// Use this for initialization
	void Start () {
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		ymax = levelControllerScript.GetMapCols ();
		xmax = levelControllerScript.GetMapRows ();
		_levelGrid = levelControllerScript.GetLevelGrid ();

		corridorTile = Resources.Load ("Prefabs/Environment/Tile") as GameObject;

	}

	// Update is called once per frame
	void Update () {
	
	}

	public int Corridors
	{
		get;
		private set;
	}

		public static bool IsWall(int x, int y, int xlen, int ylen, int xt, int yt, Direction d)
		{
			Func<int, int, int> a = GetFeatureLowerBound;

			Func<int, int, int> b = IsFeatureWallBound;
			switch (d)
			{
			case Direction.NORTH:
				return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y - ylen + 1;
			case Direction.EAST:
				return xt == x || xt == x + xlen - 1 || yt == a(y, ylen) || yt == b(y, ylen);
			case Direction.SOUTH:
				return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y + ylen - 1;
			case Direction.WEST:
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

	public static IEnumerable<PointI> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d)
	{
		// north and south share the same x strategy
		// east and west share the same y strategy
		Func<int, int, int> a = GetFeatureLowerBound;
		Func<int, int, int> b = GetFeatureUpperBound;

		switch (d)
		{
		case Direction.NORTH:
			for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt > y - ylen; yt--) yield return new PointI { X = xt, Y = yt };
			break;
		case Direction.EAST:
			for (var xt = x; xt < x + xlen; xt++) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new PointI { X = xt, Y = yt };
			break;
		case Direction.SOUTH:
			for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt < y + ylen; yt++) yield return new PointI { X = xt, Y = yt };
			break;
		case Direction.WEST:
			for (var xt = x; xt > x - xlen; xt--) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new PointI { X = xt, Y = yt };
			break;
		default:
			yield break;
		}
	}

	public GameObject GetCellType(int x, int y)
	{
		try
		{
			return this._levelGrid[x , this._xsize * y];
		}
		catch (IndexOutOfRangeException)
		{
			//new { x, y }.Dump("exceptional");
			throw;
		}
	}
	public int GetRand(int min, int max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public bool MakeCorridor(int x, int y, int length, Direction direction)
	{
		// define the dimensions of the corridor (er.. only the width and height..)
		int len = this.GetRand(2, length);
		GameObject floor = corridorTile;

		int xtemp;
		int ytemp = 0;

		switch (direction)
		{
		case Direction.NORTH:
			// north
			// check if there's enough space for the corridor
			// start with checking it's not out of the boundaries
			if (x < 0 || x > this._xsize) return false;
			xtemp = x;

			// same thing here, to make sure it's not out of the boundaries
			for (ytemp = y; ytemp > (y - len); ytemp--)
			{
				if (ytemp < 0 || ytemp > this._ysize) return false; // oh boho, it was!
				if (GetCellType(xtemp, ytemp) != null) return false;
			}

			// if we're still here, let's start building
			Corridors++;
			for (ytemp = y; ytemp > (y - len); ytemp--)
			{
				this.SetCell(xtemp, ytemp, floor);
			}

			break;

		case Direction.EAST:
			// east
			if (y < 0 || y > this._ysize) return false;
			ytemp = y;

			for (xtemp = x; xtemp < (x + len); xtemp++)
			{
				if (xtemp < 0 || xtemp > this._xsize) return false;
				if (GetCellType(xtemp, ytemp) != null) return false;
			}

			Corridors++;
			for (xtemp = x; xtemp < (x + len); xtemp++)
			{
				this.SetCell(xtemp, ytemp, floor);
			}

			break;

		case Direction.SOUTH:
			// south
			if (x < 0 || x > this._xsize) return false;
			xtemp = x;

			for (ytemp = y; ytemp < (y + len); ytemp++)
			{
				if (ytemp < 0 || ytemp > this._ysize) return false;
				if (GetCellType(xtemp, ytemp) != null) return false;
			}

			Corridors++;
			for (ytemp = y; ytemp < (y + len); ytemp++)
			{
				this.SetCell(xtemp, ytemp, floor);
			}

			break;
		case Direction.WEST:
			// west
			if (ytemp < 0 || ytemp > this._ysize) return false;
			ytemp = y;

			for (xtemp = x; xtemp > (x - len); xtemp--)
			{
				if (xtemp < 0 || xtemp > this._xsize) return false;
				if (GetCellType(xtemp, ytemp) != null) return false;
			}

			Corridors++;
			for (xtemp = x; xtemp > (x - len); xtemp--)
			{
				this.SetCell(xtemp, ytemp, floor);
			}

			break;
		}
		// woot, we're still here! let's tell the other guys we're done!!
		return true;
	}
		
	public void SetCell(int x, int y, GameObject newTile){
		levelControllerScript.ReplaceTile (x, y, newTile);
	}


}
