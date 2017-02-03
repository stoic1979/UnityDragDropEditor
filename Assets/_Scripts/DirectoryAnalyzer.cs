using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class DirectoryAnalyzer : MonoBehaviour {

	public GameObject buttonPrefab;
	private GameObject container;

	// Use this for initialization
	void Start () {

		container = GameObject.Find ("ContainerPanel");
		DirectoryInfo dir = new DirectoryInfo ("Assets/Resources/");
		FileInfo[] info = dir.GetFiles ("*.prefab");
		foreach (FileInfo f in info) {
			GameObject obj = (GameObject) Instantiate (buttonPrefab, Vector3.zero, Quaternion.identity);
			obj.name = Path.GetFileNameWithoutExtension (f.Name);
			obj.GetComponentInChildren<Text> ().text = Path.GetFileNameWithoutExtension (f.Name);
			obj.transform.parent = container.transform; 
			obj.transform.localScale = Vector3.one;
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
