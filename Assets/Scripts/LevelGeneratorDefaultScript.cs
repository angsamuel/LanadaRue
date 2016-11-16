using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class LevelGeneratorDefaultScript : MonoBehaviour {
	public GameObject unused;
	public GameObject floor;
	public GameObject wall;
	public GameObject door;
	public GameObject corridor;

    public Sprite verWall;
    public Sprite horzWall;
    public Sprite topRightCorner;
    public Sprite topLeftCorner;
    public Sprite botRightCorner;
    public Sprite botLeftCorner;

	public int _roomChance = 30;
	public int _maxFeatures = 1000;

    public int myX;
    public int myY;

    public int _width, _height;
	public List<Tile> _tiles;
	public List<Rect> _rooms;
	public List<Rect> _exits;

	LevelControllerScript levelControllerScript;

	public class TilesForJson{
		public List<Tile> tilesForJson;
	}

	int randomInt(int exclusiveMax){
		return (int)Random.Range (0, exclusiveMax-1); 
	}
	int randomInt(int min, int max){ //inclusive min/max
		return (int)Random.Range(min, max);
	}
	bool randomBool(double probability = 0.5){
		if (Random.value >= 0.5) {
			return true;
		}
		return false;
	}

	public class Rect{
		public int x, y;
		public int width, height;
	};

	public enum Tile{
		Unused,
		Floor,
		Wall,
		Door,
		Corridor
	};

	public enum Direction{
		North,
		South,
		East,
		West,
		DirectionCount
	};

	public void Dungeon(int width, int height){
		_width = width;
		_height = height;
		_tiles = new List<Tile>();
		for(int i = 0; i<width*height; ++i){
			_tiles.Add(Tile.Unused);
		}
		//Debug.Log ("initial _tiles.Count " + _tiles.Count);
		_rooms = new List<Rect> ();
		_exits = new List<Rect> ();
	}

	void generate(int maxFeatures){

		//place the first room in the center
		if (!makeRoom (_width / 2, _height / 2, randomDirection ())) {
			Debug.Log ("Unable to place first room.");
			return;
		}

		for (int i = 1; i < maxFeatures; ++i) {
			if (!createFeature ()) {
				Debug.Log ("Unable to place more features (placed " + i + ")");
				break;
			}
		}

		if(!placeObject(Tile.Door)){
			Debug.Log("Unable to place up stairs.");
			return;
		}

		if(!placeObject(Tile.Door)){
			Debug.Log("unable to place down stairs.");
		}

		for (int i = 0; i < _tiles.Count; ++i) {
			if (_tiles [i] == Tile.Unused) {
				//maybe something idk
			} else if (_tiles [i] == Tile.Floor) {
				//no clue
			}
		}

	}

	private Direction randomDirection(){
		int r = (int)Random.Range(1, 4);
		if(r == 1){return Direction.North;}
		if(r == 2){return Direction.South;}
		if(r == 3) {return Direction.East;}
		if(r == 4) {return Direction.West;}
		else{return Direction.North;}
	}

	void print(){
		for (int y = 0; y < _height; ++y) {
			for (int x = 0; x < _width; ++x) {
				//replace the tile with getTile(x, y)
				switch (getTile (x, y)) {
				case Tile.Corridor:
					levelControllerScript.ReplaceTile (x, y, corridor);
					break;
				case Tile.Floor:
					levelControllerScript.ReplaceTile (x, y, floor);
					break;
				case Tile.Door:
					levelControllerScript.ReplaceTile (x, y, door);
					break;
				case Tile.Wall:
					levelControllerScript.ReplaceTile (x, y, wall);
					break;
				case Tile.Unused:
					levelControllerScript.ReplaceTile (x, y, unused);
					break;
				default:
					levelControllerScript.ReplaceTile (x, y, unused);
					break;
				}
			}
		}
	}

	public Tile getTile(int x, int y){
		if (x < 0 || y < 0 || x >= _width || y >= _height) {
			return Tile.Unused;
		}
		//Debug.Log (x + ", " + y + " " + (x + y * _width));
		//Debug.Log (_tiles.Count);
		return _tiles [x + y * _width];
	}
		

	public void setTile(int x, int y, Tile tile){
		_tiles [x + y * _width] = tile;
	}

	//create features
	bool createFeature(){
		for (int i = 0; i < 1000; ++i) {
			if (_exits.Count < 1) {
				break;
			}
			int r = randomInt (_exits.Count);
			int x = randomInt (_exits [r].x, _exits [r].x + _exits [r].width - 1);
			int y = randomInt (_exits [r].y, _exits [r].y + _exits [r].height - 1);

			for (int j = 0; j < (int)Direction.DirectionCount; ++j) {
				if (createFeature (x, y, (Direction)(j))) {
					_exits.RemoveAt (r);
					return true;
				}
			}
		}
		return false;
	}

	bool createFeature(int x, int y, Direction dir){
		int roomChance = _roomChance; //corridorChance = 100 - roomChance
		int dx = 0;
		int dy = 0;

		if (dir == Direction.North) {
			dy = 1;
		} else if (dir == Direction.South) {
			dy = -1;
		} else if (dir == Direction.West) {
			dx = 1;
		} else if (dir == Direction.East) {
			dx = -1;
		}
		if (getTile (x + dx, y + dy) != Tile.Floor && getTile (x + dx, y + dy) != Tile.Corridor) {
			return false;
		}
		if (randomInt (100) < roomChance) {
			if (makeRoom (x, y, dir)) {
				setTile(x, y, Tile.Door);
				return true;
			}
		}else{
			if(makeCorridor(x, y, dir)){
				if(getTile(x + dx, y + dy) == Tile.Floor){
					setTile(x, y, Tile.Door);
				}else{
					setTile(x, y, Tile.Corridor);
				}
				return true;
			}

		}
		return false;
	}

	bool makeRoom(int x, int y, Direction dir, bool firstRoom = false){
		const int minRoomSize = 3;
		const int maxRoomSize = 6;

		Rect room = new Rect{};
		room.width = randomInt (minRoomSize, maxRoomSize);
		room.height = randomInt (minRoomSize, maxRoomSize);

		if (dir == Direction.North) {
			room.x = x - room.width / 2;
			room.y = y - room.height;
		} else if (dir == Direction.South) {
			room.x = x - room.width / 2;
			room.y = y + 1;
		} else if (dir == Direction.West) {
			room.x = x - room.width;
			room.y = y - room.height / 2;
		} else if (dir == Direction.East) {
			room.x = x + 1;
			room.y = y - room.height / 2;
		}

		if(placeRect(ref room, Tile.Floor, true)){
			_rooms.Add(room);
			if(dir != Direction.South || firstRoom){
				Rect r = new Rect {};
				r.x = room.x; r.y = room.y - 1; r.width = room.width; r.height = 1;
				_exits.Add(r);
			}
			if (dir != Direction.North || firstRoom) {
				Rect r = new Rect{ };
				r.x = room.x; r.y = room.y + room.height; r.width = room.width; r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.East || firstRoom) {
				Rect r = new Rect { };
				r.x = room.x - 1; r.y = room.y; r.width = 1; r.height = room.height;
				_exits.Add (r);
			}
			if (dir != Direction.West || firstRoom) {
				Rect r = new Rect { };
				r.x = room.x + room.width; r.y = room.y; r.width = 1; r.height = room.height;
				_exits.Add (r);
			}

			return true;
		}
		return false; 
	}

	//make corridor
	bool makeCorridor(int x, int y, Direction dir){
		int minCorridorLength = 3;
		int maxCorridorLength = 6;

		Rect corridor = new Rect{};
		corridor.x = x;
		corridor.y = y;

		if (randomBool ()) { //horizontal corridor
			corridor.width = randomInt (minCorridorLength, maxCorridorLength);
			corridor.height = 1;

			if (dir == Direction.North) {
				corridor.y = y - 1;
				if (randomBool ()) {//west
					corridor.x = x - corridor.width + 1;
				}
			} else if (dir == Direction.South) {
				corridor.y = y + 1;
				if (randomBool ()) {// west
					corridor.x = x - corridor.width + 1;
				}
			} else if (dir == Direction.West) {
				corridor.x = x - corridor.width;
			} else if (dir == Direction.East) {
				corridor.x = x + 1;
			}

		} else {//vertical corridor
			corridor.width = 1;
			corridor.height = randomInt (minCorridorLength, maxCorridorLength);

			if (dir == Direction.North) {
				corridor.y = y - corridor.height;
			} else if (dir == Direction.South) {
				corridor.y = y + 1;
			} else if (dir == Direction.West) {
				corridor.x = x - 1;
				if (randomBool ()) {//north
					corridor.y = y - corridor.height + 1;
				}
			} else if (dir == Direction.East) {
				corridor.x = x + 1;
				if (randomBool ()) { //south
					corridor.y = y - corridor.height + 1;
				}
			}

		}
		if (placeRect (ref corridor, Tile.Corridor, false)) {
			if (dir != Direction.South && corridor.width != 1) {
				Rect r = new Rect { };
				r.x = corridor.x;
				r.y = corridor.y - 1;
				r.width = corridor.width;
				r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.North && corridor.width != 1) {
				Rect r = new Rect{ };
				r.x = corridor.x;
				r.y = corridor.y + corridor.height;
				r.width = corridor.width;
				r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.East && corridor.height != 1) {
				Rect r = new Rect { };
				r.x = corridor.x - 1;
				r.y = corridor.y;
				r.width = 1;
				r.height = corridor.height;
				_exits.Add (r);
			}
			if (dir != Direction.West || corridor.height != 1) {
				Rect r = new Rect { };
				r.x = corridor.x + corridor.width;
				r.y = corridor.y;
				r.width = 1;
				r.height = corridor.height;
				_exits.Add (r);
			}
			return true;
		}
		return false;
	}

	//add bool to this to remove walls from corridors
	bool placeRect(ref Rect rect, Tile tile, bool needsWall){
		if (rect.x < 1 || rect.y < 1 || rect.x + rect.width > _width - 1 || rect.y + rect.height > _height - 1) {
			return false;
		}
		for (int y = rect.y; y < rect.y + rect.height; ++y) {
			for (int x = rect.x; x < rect.x + rect.width; ++x) {
				if (getTile (x, y) != Tile.Unused) {
					return false; //this area is already used
				}
			}
		}
		for (int y = rect.y - 1; y < rect.y + rect.height + 1; ++y) {
			for (int x = rect.x - 1; x < rect.x + rect.width + 1; ++x) {
				if (x == rect.x - 1 || y == rect.y - 1 || x == rect.x + rect.width || y == rect.y + rect.height) {
					if(needsWall){ setTile(x, y, Tile.Wall);}
				} else {
					setTile (x, y, tile);
				}
			}
		}
		return true;
	}

	bool placeObject(Tile tile){
		if (_rooms.Count < 1) {
			return false;
		}

		int r = randomInt (_rooms.Count);
		int x = randomInt (_rooms [r].x + 1, _rooms [r].x + _rooms [r].width - 2);
		int y = randomInt (_rooms [r].y + 1, _rooms [r].y + _rooms [r].height - 2);

		if (getTile (x, y) == Tile.Floor) {
			setTile (x, y, tile);

			_rooms.RemoveAt (r);
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		
		//load prefabs
		corridor = Resources.Load ("Prefabs/Environment/Corridor") as GameObject;
		floor = Resources.Load("Prefabs/Environment/Floor") as GameObject;
		wall = Resources.Load("Prefabs/Environment/Wall") as GameObject;
		door = Resources.Load("Prefabs/Environment/Door") as GameObject;
		unused = Resources.Load("Prefabs/Environment/Unused") as GameObject;

		//find levelController
		levelControllerScript = GameObject.Find ("LevelController").GetComponent<LevelControllerScript> ();

		myX = levelControllerScript.GetMapCols ();
		myY = levelControllerScript.GetMapRows ();
	
		ChangeLevel ();
	}

	public void ChangeLevel(){
		if (!System.IO.File.Exists (Application.dataPath + "/tiles_0_0.json")) { //generate level if it does not exist
			Dungeon (myX, myY);
			generate (_maxFeatures);
			OutlineMap ();
			print ();

			//save file as json
			TilesForJson tfj = new TilesForJson ();
			tfj.tilesForJson = _tiles;
			string levelToJson = JsonUtility.ToJson(tfj);
			System.IO.File.WriteAllText (Application.dataPath + "/tiles_0_0.json",levelToJson);
		} else { //load level otherwise
			string fileString = System.IO.File.ReadAllText(Application.dataPath + "/tiles_0_0.json");
			Dungeon (myX, myY);
			TilesForJson loadedTiles = new TilesForJson ();
			loadedTiles.tilesForJson = new List<Tile> ();
			loadedTiles = JsonUtility.FromJson<TilesForJson>(fileString);
			Debug.Log (loadedTiles.tilesForJson.Count);
			_tiles = loadedTiles.tilesForJson;
			print ();
		}
	}


	void OutlineMap(){
		for (int y = 0; y < _height; ++y) {
			for (int x = 0; x < _width; ++x) {
				if (y == 0 || y==_height-1) {
					if (getTile (x, y) == Tile.Unused) {
						setTile (x,y,Tile.Corridor);
					}
				}
				if (x == 0 || x==_width-1) {
					if (getTile (x, y) == Tile.Unused) {
						setTile (x,y,Tile.Corridor);
					}
				}
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
	}
}
