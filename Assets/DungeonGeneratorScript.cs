using UnityEngine;
using System.Collections;
using System.Linq;
//using System.Diagnostics;
using System;
using System.Collections.Generic;


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
    private GameObject[,] _levelGrid;

	//keep an eye out here
	//readonly IRandomize _rnd;
	//readonly OverflowAction<string> _logger;

	//adjusting tiles
	float oddColAdjustment = 0;
	float oddRowAdjustment = 0;

	//direction
	public enum Direction {NORTH, SOUTH, EAST, WEST};

	//point
		public class PointI{
			public int X;
			public int Y;
            public Direction direction;
            public GameObject pointTile;
        }


	//tiles
	GameObject corridorTile;
    GameObject floorTile;
    GameObject wallTile;
    GameObject doorTile;
    GameObject unusedTile;
    GameObject woodWallTile;
    // Use this for initialization
    void Start () {
		if (xmax % 2 == 0) {oddColAdjustment = -.5f;}
		if (ymax % 2 == 0) {oddRowAdjustment = -.5f;}

		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();
		ymax = levelControllerScript.GetMapCols ();
		xmax = levelControllerScript.GetMapRows ();
		_levelGrid = levelControllerScript.GetLevelGrid ();

		corridorTile = Resources.Load ("Prefabs/Environment/Corridor") as GameObject;
        floorTile = Resources.Load("Prefabs/Environment/Floor") as GameObject;
        wallTile = Resources.Load("Prefabs/Environment/Wall") as GameObject;
        doorTile = Resources.Load("Prefabs/Environment/Door") as GameObject;
        unusedTile = Resources.Load("Prefabs/Environment/Unused") as GameObject;
        woodWallTile = Resources.Load("Prefabs/Environment/WoodWall") as GameObject;
        //GameObject sut = Instantiate(unusedTile, new Vector3(0, 0, -1), Quaternion.identity) as GameObject;
        //levelControllerScript.ReplaceTile(0, 0, unusedTile);

        CreateDungeon(xmax, ymax, 100);
        //Initialize();
		levelControllerScript.SetLevelGrid(_levelGrid);
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

	public List<PointI> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d)
	{
		// north and south share the same x strategy
		// east and west share the same y strategy
		Func<int, int, int> a = GetFeatureLowerBound;
		Func<int, int, int> b = GetFeatureUpperBound;
        List<PointI> output = new List<PointI>();
        
        switch (d)
        {
            case Direction.NORTH:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt > y - ylen; yt--) output.Add(new PointI { X = xt, Y = yt });
                // Debug.Log("we north");
                break;
            case Direction.EAST:
                for (var xt = x; xt < x + xlen; xt++) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) output.Add(new PointI { X = xt, Y = yt });
                // Debug.Log("we east");
                break;
            case Direction.SOUTH:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt < y + ylen; yt++) output.Add(new PointI { X = xt, Y = yt });
                //  Debug.Log("we south");
                break;
            case Direction.WEST:
                for (var xt = x; xt > x - xlen; xt--) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) output.Add(new PointI { X = xt, Y = yt });
                // Debug.Log("we west");
                break;
            default:
                break;
        }
        
        return output;
	}

	public GameObject GetCellType(int x, int y)
	{
		try
		{
			Debug.Log(x + ", " + y);
			return this._levelGrid[x , y];
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
				if (GetCellType(xtemp, ytemp).GetType() != unusedTile.GetType()) return false;
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
				if (GetCellType(xtemp, ytemp).GetType() != unusedTile.GetType()) return false;
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
				if (GetCellType(xtemp, ytemp).GetType() != unusedTile.GetType()) return false;
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
				if (GetCellType(xtemp, ytemp).GetType() != unusedTile.GetType()) return false;
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

    public List<PointI> GetSurroundingPoints(PointI v)
    {
        List<string> mylist = new List<string> { "element1", "element2", "element3" };
        PointI n = new PointI { X = v.X, Y = v.Y + 1, direction = Direction.NORTH };
        PointI e = new PointI { X = v.X - 1, Y = v.Y, direction = Direction.EAST };
        PointI s = new PointI { X = v.X, Y = v.Y - 1, direction = Direction.SOUTH };
        PointI w = new PointI { X = v.X + 1, Y = v.Y, direction = Direction.WEST };

        List<PointI> points = new List<PointI> { n, e, s, w };
        List<PointI> output = new List<PointI>();
        foreach(PointI p in points)
        {
            if(InBounds(p))
            {
                output.Add(p);
            }
        }
        return output;

    }
    /*
    public IEnumerable<Tuple<PointI, Direction, Tile>> GetSurroundings(PointI v)
    {
        return
            this.GetSurroundingPoints(v)
                .Select(r => Tuple.Create(r.Item1, r.Item2, this.GetCellType(r.Item1.X, r.Item1.Y)));
    }*/
    public List<PointI> GetSurroundings(PointI v){
        List<PointI> firstList = GetSurroundingPoints(v);
        List<PointI> output = new List<PointI>();
        foreach (PointI p in firstList)
        {
			GameObject ti = GetCellType(p.X, p.Y);
            p.pointTile = ti;
            output.Add(p);
        }
        
        return output;   
    }


    public bool InBounds(int x, int y)
    {
        return x > 0 && x < this.xmax && y > 0 && y < this.ymax;
    }

    public bool InBounds(PointI v)
    {
        return this.InBounds(v.X, v.Y);
    }

    public bool MakeRoom(int x, int y, int xlength, int ylength, Direction direction)
    {
        // define the dimensions of the room, it should be at least 4x4 tiles (2x2 for walking on, the rest is walls)
        int xlen = this.GetRand(4, xlength);
        int ylen = this.GetRand(4, ylength);

        // the tile type it's going to be filled with
        GameObject Floor = floorTile;

        GameObject Wall = wallTile;
        // choose the way it's pointing at
        //splunk1
        List<PointI> points = GetRoomPoints(x, y, xlen, ylen, direction);
        Debug.Log("points size: " + points.Count);
        // Check if there's enough space left for it
        if (
            points.Any(
                s =>
				s.Y < 0 || s.Y > this._ysize || s.X < 0 || s.X > this._xsize || this.GetCellType(s.X, s.Y).GetType() != unusedTile.GetType())) return false;
       
        //logger went here

        foreach (var p in points)
        {
            //Debug.Log("making dat room");
            this.SetCell(p.X, p.Y, IsWall(x, y, xlen, ylen, p.X, p.Y, direction) ? Wall : Floor);
        }

        // yay, all done
        return true;
    }

    public GameObject[,] GetLevelGrid()
    {
        return this._levelGrid;
    }

    public void ShowDungeon()
    {
        for (int y = 0; y < this._ysize; y++)
        {
            for (int x = 0; x < this._xsize; x++)
            {
                // Console.Write(GetCellTile(x, y));
                levelControllerScript.ReplaceTile(x, y, GetCellType(x,y));
            }
            //bandaid
            //if (this._xsize <= xmax) { levelControllerScript.ReplaceTile(x, y, wallTile); }
        }
    }
    public Direction RandomDirection()
    {
        int dir = this.GetRand(0, 4);
        switch (dir)
        {
            case 0:
                return Direction.NORTH;
            case 1:
                return Direction.EAST;
            case 2:
                return Direction.SOUTH;
            case 3:
                return Direction.WEST;
            default:
                throw new InvalidOperationException();
        }
    }

    public bool CreateDungeon(int inx, int iny, int inobj)
    {
        this._objects = inobj < 1 ? 10 : inobj;

        // adjust the size of the map, if it's smaller or bigger than the limits
        if (inx < 3) this._xsize = 3;
        else if (inx > xmax) this._xsize = xmax;
        else this._xsize = inx;

        if (iny < 3) this._ysize = 3;
        else if (iny > ymax) this._ysize = ymax;
        else this._ysize = iny;

        //Console.WriteLine(MsgXSize + this._xsize);
        //Console.WriteLine(MsgYSize + this._ysize);
        //Console.WriteLine(MsgMaxObjects + this._objects);

        // redefine the map var, so it's adjusted to our new map size
        //this._dungeonMap = new Tile[this._xsize * this._ysize];
        this._levelGrid = new GameObject[levelControllerScript.GetMapCols(), levelControllerScript.GetMapRows()];

        // start with making the "standard stuff" on the map
        this.Initialize();

        /*******************************************************************************
        And now the code of the random-map-generation-algorithm begins!
        *******************************************************************************/

        // start with making a room in the middle, which we can start building upon
        this.MakeRoom(this._xsize / 2, this._ysize / 2, 8, 6, RandomDirection()); // getrand saken f????r att slumpa fram riktning p?? rummet

        // keep count of the number of "objects" we've made
        int currentFeatures = 1; // +1 for the first room we just made

        // then we sart the main loop
        for (int countingTries = 0; countingTries < 1000; countingTries++)
        {
            //Debug.Log("try counted"); --we made it here

            // check if we've reached our quota
            if (currentFeatures == this._objects)
            {
				Debug.Log ("enough features, breaking out");
                break;
            }

            // start with a random wall
            int newx = 0;
            int xmod = 0;
            int newy = 0;
            int ymod = 0;
            Direction? validTile = null;

            // 1000 chances to find a suitable object (room or corridor)..
            for (int testing = 0; testing < 1000; testing++)
            {
                //Debug.Log("testing counted"); -made it here
                newx = this.GetRand(1, this._xsize - 1);
                newy = this.GetRand(1, this._ysize - 1);

				//Debug.Log ("testing tile at" + newx + " " + newy);
				/*
				if (GetCellType (newx, newy).GetType() == unusedTile.GetType()) {
				//	Debug.Log ("found unused");
				}else if(GetCellType (newx, newy).GetType() == floorTile.GetType() || GetCellType (newx, newy).GetType() == corridorTile.GetType()){
					Debug.Log ("found floor");
				}else {
					Debug.Log ("wall");
				}*/
				//GetCellType (newx, newy).GetType ();


				if (GetCellType(newx, newy).GetType() == wallTile.GetType() || GetCellType(newx, newy).GetType() == corridorTile.GetType())
                {
					//HERE WE ARE

                    //Debug.Log("wall or corr"); //!!!
                    var surroundings = this.GetSurroundings(new PointI() { X = newx, Y = newy });

                    // check if we can reach the place
                    //var canReach = surroundings.FirstOrDefault(s => s.Item3 == Tile.Corridor || s.Item3 == Tile.DirtFloor);

                    PointI canReach = new PointI { };
                    foreach(PointI p in surroundings)
                    {
						if(p.pointTile.GetType() == corridorTile.GetType() || p.pointTile.GetType() == floorTile.GetType())
                        {
                            canReach = p;
                            break;
                        }
                    }
                    if (canReach == null)
                    {
						//Debug.Log ("found no canReach");
                        continue;
                    }
                    validTile = canReach.direction;

                        switch (validTile)
                    {
                        case Direction.NORTH:
							Debug.Log ("n");
                            xmod = 0;
                            ymod = -1;
                            break;
                        case Direction.EAST:
							Debug.Log ("e");
                            xmod = 1;
                            ymod = 0;
                            break;
                        case Direction.SOUTH:
							Debug.Log ("s");
                            xmod = 0;
                            ymod = 1;
                            break;
						case Direction.WEST:
							Debug.Log ("w");
                            xmod = -1;
                            ymod = 0;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

					if (validTile.HasValue) {
						Debug.Log ("WORKS HERE-----------");
					}

                    // check that we haven't got another door nearby, so we won't get alot of openings besides
                    // each other

					if (GetCellType (newx, newy + 1).GetType () == doorTile.GetType ()) { // north
						Debug.Log ("Found a north door");
						//validTile = null;
					} else if (GetCellType (newx - 1, newy).GetType () == doorTile.GetType ()) { // east
						//validTile = null;
						Debug.Log ("Found an east door");
					} else if (GetCellType (newx, newy - 1).GetType () == doorTile.GetType ()) { // south
						Debug.Log ("Found a south door");
						//validTile = null;
					} else if (GetCellType (newx + 1, newy).GetType () == doorTile.GetType ()) { // west
						//validTile = null;
						Debug.Log ("Found a west door");
					}

                    // if we can, jump out of the loop and continue with the rest
					if (validTile.HasValue) {
						Debug.Log ("WORKS HERE TOO-----------");
						break;
					}
                }
            }

            if (validTile.HasValue)
            {
                Debug.Log("has value");
                // choose what to build now at our newly found place, and at what direction
                int feature = this.GetRand(0, 100);
                if (feature <= ChanceRoom)
                { // a new room
                    Debug.Log("new room!");
                    if (this.MakeRoom(newx + xmod, newy + ymod, 8, 6, validTile.Value))
                    {
                        currentFeatures++; // add to our quota

                        // then we mark the wall opening with a door
                        this.SetCell(newx, newy, doorTile);

                        // clean up infront of the door so we can reach it
                        this.SetCell(newx + xmod, newy + ymod, floorTile);
                    }
                }
                else if (feature >= ChanceRoom)
                { // new corridor
                    if (this.MakeCorridor(newx + xmod, newy + ymod, 6, validTile.Value))
                    {
                        // same thing here, add to the quota and a door
                        currentFeatures++;

                        this.SetCell(newx, newy, doorTile);
                    }
                }
            }
        }

        /*******************************************************************************
        All done with the building, let's finish this one off
        *******************************************************************************/
        //AddSprinkles();

        // all done with the map generation, tell the user about it and finish
        // Console.WriteLine(MsgNumObjects + currentFeatures);

        return true;
    }

    //WORKING
    void Initialize()
    {
        UnityEngine.Debug.Log("initialization called");
        for (int y = 0; y < this._ysize; y++)
        {
            for (int x = 0; x < this._xsize; x++)
            {
                // ie, making the borders of unwalkable walls
                if (y == 0 || y == this._ysize - 1 || x == 0 || x == this._xsize - 1)
                {
                    //Debug.Log("calling SetCell()");
                    this.SetCell(x, y, woodWallTile);
                }
                else
                {                        // and fill the rest with dirt
                    //Debug.Log("calling SetCell()");
                    this.SetCell(x, y, unusedTile);
                }
            }
        }
    }

    public void SetCell(int x, int y, GameObject newTile){
        //Debug.Log("Replacing tile...");
		ReplaceTile (x, y, newTile);
	}

	public void ReplaceTile(int x, int y, GameObject newTile){
		Debug.Log (x + ", " + y);
		if (_levelGrid [x, y] != null) {
			Destroy (_levelGrid [x, y]);
		}
		_levelGrid [x, y] = Instantiate (newTile, CoordToVector3(x,y, 0), Quaternion.identity) as GameObject;
		levelControllerScript.SetLevelGrid (_levelGrid);
	}

	public Vector3 CoordToVector3(int x ,int y, int z){
		return new Vector3 (x - ymax / 2 - oddColAdjustment, y - xmax / 2 - oddRowAdjustment, z);
	}
}
