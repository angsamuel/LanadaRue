using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	GameObject levelController;
	LevelControllerScript levelControllerScript;

	private int mapRows;
	private int mapCols;

	private float camHeight;
	private float camWidth;
	private float maxCamHeight;
	private float maxCamWidth;

	public Camera cam;
	public float originalCameraSpeedCutBy = 10f;
	public float cameraSpeedCutBy;

	public float ppuScale = .05f;
	public int ppu = 32;

	public float cameraCurrentZoom;
	private float cameraZoomMax;
	public float cameraZoomMin = 10;

	public float czm;
	public float mouseSensitivity = 0f;
	private Vector3 lastPosition;


	void Start ()
	{
		levelController = GameObject.Find ("LevelController");
		levelControllerScript = levelController.GetComponent<LevelControllerScript> ();

		mapRows = levelControllerScript.GetMapRows();
		mapCols = levelControllerScript.GetMapCols();

		//calculate maximum camera zoom //Orthographic size = ((Vert Resolution)/(PPUScale * PPU)) * 0.5
		cameraCurrentZoom = ((Screen.height)/(1 * 32))*0.5000f;
		cameraZoomMax = cameraCurrentZoom;
		Camera.main.orthographicSize = cameraCurrentZoom;
		cameraSpeedCutBy = originalCameraSpeedCutBy * (cameraCurrentZoom / cameraZoomMax);
		czm = cameraCurrentZoom;
		Debug.Log ("width " + Screen.width);
		Debug.Log ("height " + Screen.height);

		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;
		maxCamWidth = camWidth;
		maxCamHeight = camHeight;
	}

    void LateUpdate()
    {
        Vector3 v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, -(mapCols / 2) + camWidth / 2, (mapCols / 2) - camWidth / 2);
        v3.y = Mathf.Clamp(v3.y, -(mapRows / 2) + camHeight / 2, (mapRows / 2) - camHeight / 2);
        transform.position = v3;
    }


    void Update () {
		czm = cameraCurrentZoom;

		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;
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
		//Pan Movement
		//camera boundaries
		//transform.position = new Vector3(Mathf.Clamp(Time.time, 1.0F, 3.0F), 0, 0);
		//Mathf.Clamp (transform.position.y,-(mapRows/2) + camHeight/2,(mapRows/2) - camHeight/2);

		cameraSpeedCutBy = originalCameraSpeedCutBy * (2f - ((cameraCurrentZoom) / cameraZoomMax));
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