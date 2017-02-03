using UnityEngine;
using System.Collections;

public class DragObject : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offSet;
	private Collider col;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//================================================================================================
		void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
		offSet = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	void OnMouseDrag(){

		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offSet;
		curPosition.y = transform.position.y;
		transform.position = curPosition;
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	void OnMouseUp(){
		
	}
	//================================================================================================
}
