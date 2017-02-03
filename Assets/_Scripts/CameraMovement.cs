using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	// VARIABLE FOR PAN SPEED
	public float panSpeed = 4.0f;

	// VARIABLE FOR ENABLE / DISABLE PAN TO CAMERA
	private bool isPanning = false;

	// VARIABLE FOR MOUSE ORIGIN
	private Vector3 mouseOrigin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// GETTING RIGHT MOUSE BUTTON
		if(Input.GetMouseButtonDown(1)){

			// GETTING MOUSE ORIGIN
			mouseOrigin = Input.mousePosition;
			isPanning = true;

		}

		// DISABLE MOVEMENT FOR PANING ON BUTTON RELEASE
		if(!Input.GetMouseButton(1)){
			isPanning = false;
		}

		// MOVE THE CAMERA ON IT'S XY PLANE
		if(isPanning){

			Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - mouseOrigin);

			Vector3 move = new Vector3 (pos.x * panSpeed, pos.y * panSpeed, 0);
			transform.Translate (move, Space.Self);

		}

	}
}
