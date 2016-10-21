using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	private int levelWidth;
	private int levelHeight;

	public Camera cam;
	public float originalCameraSpeedCutBy = 10f;
	public float cameraSpeedCutBy;

	public float ppuScale = .05f;
	public int ppu = 32;

	private float cameraCurrentZoom;
	private float cameraZoomMax;
	public float cameraZoomMin = 10;

	public float czm;
	public float mouseSensitivity =0f;
	private Vector3 lastPosition;

	void Start ()
	{
		//Orthographic size = ((Vert Resolution)/(PPUScale * PPU)) * 0.5
		//levelWidth = GameObject.Find("LevelController");
		//calculate maximum camera zoom
		cameraCurrentZoom = ((Screen.width)/(1 * 32))*0.5f;
		cameraZoomMax = cameraCurrentZoom;
		Camera.main.orthographicSize = cameraCurrentZoom;
		cameraSpeedCutBy = originalCameraSpeedCutBy * (cameraCurrentZoom / cameraZoomMax);
		czm = cameraCurrentZoom;
		Debug.Log ("width " + Screen.width);
		Debug.Log ("height " + Screen.height);
	}
	void Update () {
		czm = cameraCurrentZoom;
		if (transform.position.y < -49.5f + cam.orthographicSize) {
			transform.position = new Vector3 (transform.position.x, -49.5f + cam.orthographicSize, transform.position.z);;
		}
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
		cameraSpeedCutBy = originalCameraSpeedCutBy * (2f - ((cameraCurrentZoom) / cameraZoomMax));
		if (Input.GetMouseButtonDown(0))
		{
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Translate(-delta.x/cameraSpeedCutBy, -delta.y/cameraSpeedCutBy, 0);
			lastPosition = Input.mousePosition;
		}

	}
}

