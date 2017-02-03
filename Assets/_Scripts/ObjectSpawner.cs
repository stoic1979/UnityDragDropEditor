using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//========================================================================================================
	public void SpawnObject(){
		Debug.Log ("This is test method");
		GameObject.Find ("GameObject_TaskManager").GetComponent<TaskManager> ().AddObject (gameObject.name);
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
}
