/*
 * CameraScript manages user input for the camera, 
 * and adjusts orthographic size and other variables
 * depending on resolution
 * */
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    //level controller and script
	GameObject levelController;
	LevelControllerScript levelControllerScript;

    //number of rows and columns in the level grid
	private int mapRows;
	private int mapCols;

    //camera dimensions
	private float camHeight;
	private float camWidth;
	private float maxCamHeight;
	private float maxCamWidth;
    //the actual camera
	public Camera cam;

    //cuts camera semsitivity, should be adjustable in the future
	public float originalCameraSpeedCutBy = 10f;
	public float cameraSpeedCutBy;

    //pixels per unit and scale, needed for camera 
	public float ppuScale = 1f;
	public int ppu = 32;

    //camera zoom boundaries
	private float cameraCurrentZoom;
	private float cameraZoomMax;
	public float cameraZoomMin = 10;
    //used to view currentCameraZoom without changing it
    public float czm;

    //last position of cursor, used for panning the map
	private Vector3 lastPosition;


	void Start ()
	{
        //connect with level controller and script
		levelController = GameObject.Find ("LevelController");
		levelControllerScript = levelController.GetComponent<LevelControllerScript> ();

        //find the rows and columns in of the level grid
		mapRows = levelControllerScript.GetMapRows();
		mapCols = levelControllerScript.GetMapCols();

		//calculate maximum camera zoom //Orthographic size = ((Vert Resolution)/(PPUScale * PPU)) * 0.5
		cameraCurrentZoom = ((Screen.height)/(ppuScale * 32))*0.5000f;
		cameraZoomMax = cameraCurrentZoom;
		Camera.main.orthographicSize = cameraCurrentZoom;

        //make the camera less sensitive as we zoom in
		cameraSpeedCutBy = originalCameraSpeedCutBy * (cameraCurrentZoom / cameraZoomMax);

        //czm is for viewing only
		czm = cameraCurrentZoom;

        //get the dimensions of the camera (screen resolution), save values in max variables
		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;
		maxCamWidth = camWidth;
		maxCamHeight = camHeight;
	}
    
    void LateUpdate()
    {
        //make sure camera height and with are always accurate
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        czm = cameraCurrentZoom;

        //clamps camera position so we don't go off the board 
        Vector3 v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, -(mapCols / 2) + camWidth / 2, (mapCols / 2) - camWidth / 2);
        v3.y = Mathf.Clamp(v3.y, -(mapRows / 2) + camHeight / 2, (mapRows / 2) - camHeight / 2);
        transform.position = v3;

        //camera zooming
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (cameraCurrentZoom < cameraZoomMax)
            {
                cameraCurrentZoom += 1;
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize + 1);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (cameraCurrentZoom > cameraZoomMin)
            {
                cameraCurrentZoom -= 1;
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize - 1);
            }
        }
        //ensure camera sensitivity adjusts for zoom level
        cameraSpeedCutBy = originalCameraSpeedCutBy * (2f - ((cameraCurrentZoom) / cameraZoomMax));
    }

    void Update()
    {
        //Pan Camera Input scanning
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x / cameraSpeedCutBy, -delta.y / cameraSpeedCutBy, 0);
            lastPosition = Input.mousePosition;
        }
    }
}