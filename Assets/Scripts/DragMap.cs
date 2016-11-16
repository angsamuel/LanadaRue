using UnityEngine;
using System.Collections;

public class DragMap : MonoBehaviour {
	private float dist;
	private Vector3 v3OrgMouse;

	public float mapWidth = 20.0f;
	public float zoomFactor = 0.4f;
	public float minZoom = 0.4f;

	private float maxZoom = 25.0f;
	private float minX, maxX;

	void Start () {
		dist = transform.position.y;  // Distance camera is above map
		CalcMinMax();
		maxZoom = Camera.main.orthographicSize * Screen.height / Screen.width * 2.0f;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			v3OrgMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			v3OrgMouse = Camera.main.ScreenToWorldPoint (v3OrgMouse);
			v3OrgMouse.y = transform.position.y;
		} 
		else if (Input.GetMouseButton (0)) {
			var v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
			v3Pos = Camera.main.ScreenToWorldPoint (v3Pos);
			v3Pos.y = transform.position.y;
			transform.position -= (v3Pos - v3OrgMouse);
		}

		float sw = Input.GetAxis ("Mouse ScrollWheel");
		if (Mathf.Abs (sw) > 0.01f) {
			Camera.main.orthographicSize += sw * zoomFactor;
			Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, minZoom, maxZoom);
			CalcMinMax();
		}
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, minX, maxX);
		transform.position = pos;
	}

	void CalcMinMax() {
		float height = Camera.main.orthographicSize * 2.0f;
		float width  = height * Screen.width / Screen.height;
		maxX = (mapWidth - width) / 2.0f;
		minX = -maxX;
	}
}