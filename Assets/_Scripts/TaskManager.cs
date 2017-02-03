using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour {

	private string path;
	private List<GameObject> allGameObjects = new List<GameObject> ();

	//================================================================================================
	// EDITOR UI VARIABLES
	public GameObject loadingPopup;
	public GameObject buttonPanel;
	public GameObject noDataPopup;
	public Text popupMessage;
	public Text prefabName;
	public GameObject addObjectButton;
	public GameObject objectList;
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


	//================================================================================================
	// VARIABLES TO KEEP TRACK AND MAINTAINANCE OF CAMERA FIELD OF VIEW
	private float sensitivity = 10.0f;
	private float minFov = 20.0f;
	private float maxFov = 80.0f;
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


	// Use this for initialization
	void Start () {

		loadingPopup.SetActive (false);
		buttonPanel.SetActive (true);
		noDataPopup.SetActive (false);
		addObjectButton.SetActive (true);
		objectList.SetActive (false);
	}

	//================================================================================================
	// METHOD TO GET ALL GAMEOBJECTS AVAILABLE IN SCENE
	public void saveAllData(){

		GameObject[] cubeObjects      = new GameObject[GameObject.FindGameObjectsWithTag ("cube").Length];
		GameObject[] capsuleObjects   = new GameObject[GameObject.FindGameObjectsWithTag ("capsule").Length];
		GameObject[] cylinderObjects  = new GameObject[GameObject.FindGameObjectsWithTag ("cylinder").Length];
		GameObject[] sphereObjects    = new GameObject[GameObject.FindGameObjectsWithTag ("sphere").Length];

		cubeObjects      = GameObject.FindGameObjectsWithTag ("cube");
		capsuleObjects   = GameObject.FindGameObjectsWithTag ("capsule");
		cylinderObjects  = GameObject.FindGameObjectsWithTag ("cylinder");
		sphereObjects    = GameObject.FindGameObjectsWithTag ("sphere");


		AppendDataToList(cubeObjects,allGameObjects);
		AppendDataToList(capsuleObjects,allGameObjects);
		AppendDataToList(cylinderObjects,allGameObjects);
		AppendDataToList(sphereObjects,allGameObjects);

		Save ();
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO APPEND THE DATA TO THE REQUIRED LIST
	public void AppendDataToList(GameObject[] array, List<GameObject> list){
		
		for (int i = 0; i < array.Length; i++) {
			// ADDING DATA FROM SCENE TO THE LIST OF GAMEOBJECTS TO SAVE INTO XML FILE.
			list.Add (array [i]);
		}
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// SAVE THE DATA TO THE XML FILE
	public void Save(){
		path = getPath ();
		XmlDocument xmlDoc = new XmlDocument ();

		XmlElement elmRoot = xmlDoc.CreateElement ("Data");
		xmlDoc.AppendChild (elmRoot);

		for (int i = 0; i < allGameObjects.Count; i++) {
			
			// CHECK FOR THE TYPE OF THE OBJECT BEFOR SAVE INTO XML FILE

			XmlElement Current_Object;

			if (allGameObjects [i].tag == "cube") {
				// CREATING AN XML ELEMENT WITH CUBE
				Current_Object = xmlDoc.CreateElement ("cube");	
			} else if (allGameObjects [i].tag == "capsule") {
				//CREATING AN XML ELEMENT WITH CUBE
				Current_Object = xmlDoc.CreateElement ("capsule");
			} else if (allGameObjects [i].tag == "cylinder") {
				//CREATING AN XML ELEMENT WITH CUBE
				Current_Object = xmlDoc.CreateElement ("cylinder");
			} else if (allGameObjects [i].tag == "sphere") {
				//CREATING AN XML ELEMENT WITH CUBE
				Current_Object = xmlDoc.CreateElement ("sphere");
			} else {
				// CREATE AN XML ELEMENT WITH EMPTY
				Current_Object = xmlDoc.CreateElement("empty");
			}


			//CREATING AN XML ELEMENT FOR SAVING OBJECT NAME.
			XmlElement Current_Obj_Name = xmlDoc.CreateElement("name");
			Current_Obj_Name.InnerText = allGameObjects [i].name;

			// CREATING AN XML ELEMENT FOR SAVING OBJECT POSITION.
			XmlElement Current_Obj_Position = xmlDoc.CreateElement("position");

			// CREATING AN XML ELEMENT FOR SAVING POSITION'S X-AXIS VALUE.
			XmlElement Current_Position_x = xmlDoc.CreateElement("x");
			Current_Position_x.InnerText = allGameObjects [i].transform.position.x + "";

			// CREATING AN XML ELEMENT FOR SAVING POSITION'S Y-AXIS VALUE.
			XmlElement Current_Position_y = xmlDoc.CreateElement("y");
			Current_Position_y.InnerText = allGameObjects [i].transform.position.y + "";

			//CREATING AN XML ELEMENT FOR SAVING POSITION'S Z-AXIS VALUE. 
			XmlElement Current_Position_z = xmlDoc.CreateElement("z");
			Current_Position_z.InnerText = allGameObjects [i].transform.position.z + "";

			Current_Obj_Position.AppendChild (Current_Position_x);
			Current_Obj_Position.AppendChild (Current_Position_y);
			Current_Obj_Position.AppendChild (Current_Position_z);

			// CREATING AN XML ELEMENT FOR SAVING OBJECT SCALE.
			XmlElement Current_Obj_Scale = xmlDoc.CreateElement("scale");

			//CREATING AN XML ELEMENT FOR SAVING SCALE'S X-AXIS VALUE.
			XmlElement Current_Scale_x = xmlDoc.CreateElement("x");
			Current_Scale_x.InnerText = allGameObjects [i].transform.localScale.x + "";

			//CREATING AN XML ELEMENT FOR SAVING SCALE'S Y-AXIS VALUE.
			XmlElement Current_Scale_y = xmlDoc.CreateElement("y");
			Current_Scale_y.InnerText = allGameObjects [i].transform.localScale.y + "";

			//CREATING AN XML ELEMENT FOR SAVING SCALE'S Z-AXIS VALUE.
			XmlElement Current_Scale_z = xmlDoc.CreateElement("z");
			Current_Scale_z.InnerText = allGameObjects [i].transform.localScale.z + "";

			Current_Obj_Scale.AppendChild (Current_Scale_x);
			Current_Obj_Scale.AppendChild (Current_Scale_y);
			Current_Obj_Scale.AppendChild (Current_Scale_z);

			// CREATING AN XML ELEMENT FOR SAVING TAG OF THE OBJECT
			XmlElement Current_Tag = xmlDoc.CreateElement("tag");
			Current_Tag.InnerText = allGameObjects [i].tag;

			Current_Object.AppendChild (Current_Obj_Name);
			Current_Object.AppendChild (Current_Obj_Position);
			Current_Object.AppendChild (Current_Obj_Scale);
			Current_Object.AppendChild (Current_Tag);

			elmRoot.AppendChild (Current_Object);
		}

		StreamWriter outStream = System.IO.File.CreateText (path);
		xmlDoc.Save (outStream);
		outStream.Close ();

		popupMessage.text = "Data Saved Successfully !!!";
		StartCoroutine (DisplayMessage ());
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
		
	// Update is called once per frame
	void Update () {

		// ZOOMING CAMERA WITH MOUSE WHEEL SCROLLING
		HandleCameraWithMouseWheel();
	}

	//================================================================================================
	// METHOD TO HANDLE THE CAMERA
	// FIELD OF VIEW WITH MOUSE WHEEL SCROLLING
	private void HandleCameraWithMouseWheel(){
		float fov = Camera.main.fieldOfView;
		fov -= Input.GetAxis ("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp (fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO HANDLE THE CAMERA
	// FIELD OF VIEW WITH BUTTON CLICK
	public void HandleCameraWithButtonClick(bool zoomIn){
		
		float fov = Camera.main.fieldOfView;

		if (zoomIn) {

			fov -= 5;
			if (fov < minFov) {
				fov = minFov;
			}
		} else {

			fov += 5;
			if (fov > maxFov) {
				fov = maxFov;
			}
		}

		Camera.main.fieldOfView = fov;
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// COROUTINE TO DISPLAY MESSAGE POPUP
	private IEnumerator DisplayMessage(){
		noDataPopup.SetActive (true);
		buttonPanel.SetActive (false);
		addObjectButton.SetActive (false);
		yield return new WaitForSeconds (2.0f);
		noDataPopup.SetActive (false);
		buttonPanel.SetActive (true);
		addObjectButton.SetActive (true);
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO SHOW ADD OBJECT POPUP
	public void ShowAddObjectPopup(){
		
		addObjectButton.SetActive (false);
		buttonPanel.SetActive (false);
		objectList.SetActive (true);
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO HIDE ADD OBJECT POPUP
	public void HideAddObjectPopup(){

		addObjectButton.SetActive (true);
		buttonPanel.SetActive (true);
		objectList.SetActive (false);
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO ADD OBJECT
	public void AddObject(string val){

		Instantiate (Resources.Load (val, typeof(GameObject)) as GameObject, Vector3.zero, Quaternion.identity);

		popupMessage.text = "Object Added";
		objectList.SetActive (false);
		StartCoroutine (DisplayMessage ());
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// CHECK CONDITIONS TO LOAD DATA
	public void LoadData(){

		if (!File.Exists (getPath())) {
			popupMessage.text = "No Saved File Found !!!";
			StartCoroutine (DisplayMessage());
			return;
		}

		GameObject[] cubeObjects      = new GameObject[GameObject.FindGameObjectsWithTag ("cube").Length];
		GameObject[] capsuleObjects   = new GameObject[GameObject.FindGameObjectsWithTag ("capsule").Length];
		GameObject[] cylinderObjects  = new GameObject[GameObject.FindGameObjectsWithTag ("cylinder").Length];
		GameObject[] sphereObjects    = new GameObject[GameObject.FindGameObjectsWithTag ("sphere").Length];

		cubeObjects      = GameObject.FindGameObjectsWithTag ("cube");
		capsuleObjects   = GameObject.FindGameObjectsWithTag ("capsule");
		cylinderObjects  = GameObject.FindGameObjectsWithTag ("cylinder");
		sphereObjects    = GameObject.FindGameObjectsWithTag ("sphere");


		AppendDataToList(cubeObjects,allGameObjects);
		AppendDataToList(capsuleObjects,allGameObjects);
		AppendDataToList(cylinderObjects,allGameObjects);
		AppendDataToList(sphereObjects,allGameObjects);

		if (allGameObjects.Count == 0) {
			LoadInitialData ();
		} else {
			loadingPopup.SetActive (true);
			buttonPanel.SetActive (false);
		}

	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// HIDING LOADING POPUP
	public void HidePopUp(){
		loadingPopup.SetActive (false);
		buttonPanel.SetActive (true);
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// GO FOR LOADING INITIAL DATA
	public void LoadPreviouslySavedData(){

		for (int i = allGameObjects.Count - 1; i >= 0; i--) {
			Destroy (allGameObjects [i].gameObject);
			allGameObjects.RemoveAt (i);
		}

		loadingPopup.SetActive (false);
		buttonPanel.SetActive (true);

		LoadInitialData ();
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// LOAD THE INITIAL DATA FROM THE XML FILE
	public void LoadInitialData(){
		

		path = getPath ();

		XmlReader reader = XmlReader.Create (path);
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.Load (reader);
		XmlNodeList Data = xmlDoc.GetElementsByTagName ("Data");

		for (int i = 0; i < Data.Count; i++) {

			// GETTING DATA NODE.
			XmlNode DataChilds = Data.Item(i);

			// CRAETING A LIST AND STORING ALL GAMEOBJECTS STORED INSIDE DATA.
			XmlNodeList allGameObjects = DataChilds.ChildNodes;

			for (int j = 0; j < allGameObjects.Count; j++) {
				XmlNode game_Object = allGameObjects.Item (j);

				//LOAD PROPERTIES OF ALL THE CUBES AND GENERATE WITH THOSE PROPERTIES.
				XmlNodeList gameObjectProperties = game_Object.ChildNodes;

				// GETTING NAME VALUE OF OBJECT.
				XmlNode gameObjectName = gameObjectProperties.Item (0);

				//GETTING POSITION VALUES OF OBJECT.
				XmlNodeList gameObjectPositionValues = gameObjectProperties.Item (1).ChildNodes;
				//X-AXIS VALUE.
				XmlNode gameObjectPositionX = gameObjectPositionValues.Item(0);
				// Y-AXIS VALUE.
				XmlNode gameObjectPositionY = gameObjectPositionValues.Item(1);
				// Z-AXIS VALUE.
				XmlNode gameObjectPositionZ = gameObjectPositionValues.Item(2);

				//SAVE THE POSITION VALUE INTO VECTOR3 VARIABLE.
				Vector3 pos = new Vector3();
				pos.x = float.Parse(gameObjectPositionX.InnerText);
				pos.y = float.Parse(gameObjectPositionY.InnerText);
				pos.z = float.Parse(gameObjectPositionZ.InnerText);

				// GETTING SCALE VALUES OF OBJECT.
				XmlNodeList gameObjectScaleValues = gameObjectProperties.Item(2).ChildNodes;
				// X- AXIS VALUES.
				XmlNode gameObjectScaleX = gameObjectScaleValues.Item(0);
				//Y-AXIS VALUES.
				XmlNode gameObjectScaleY = gameObjectScaleValues.Item(1);
				// Z-AXIS VALUES.
				XmlNode gameObjectScaleZ = gameObjectScaleValues.Item(2);

				// SAVE THE SCALE VALUES INTO VECTOR3 VARIABLE.
				Vector3 scale = new Vector3();
				scale.x = float.Parse(gameObjectScaleX.InnerText);
				scale.y = float.Parse(gameObjectScaleY.InnerText);
				scale.z = float.Parse(gameObjectScaleZ.InnerText);

				// GETTING TAG VALUE OF OBJECT TO GET DECISION TO INSTANTIATE PREFAB

				XmlNode gameObjectTag = gameObjectProperties.Item (3);
				GameObject requiredObject;
				// CHECK VALUE OF GAME OBJECT TAG
				if (gameObjectTag.InnerText.ToString() == "cube") {
					
					requiredObject = (GameObject) Instantiate (Resources.Load ("Cube", typeof(GameObject)) as GameObject, pos, Quaternion.identity);

				} else if (gameObjectTag.InnerText.ToString() == "cylinder") {
					
					requiredObject = (GameObject) Instantiate (Resources.Load ("Cylinder", typeof(GameObject)) as GameObject, pos, Quaternion.identity);

				} else if (gameObjectTag.InnerText.ToString() == "capsule") {
					
					requiredObject = (GameObject) Instantiate (Resources.Load ("Capsule", typeof(GameObject)) as GameObject, pos, Quaternion.identity);

				} else {
					
					requiredObject = (GameObject) Instantiate (Resources.Load ("Sphere", typeof(GameObject)) as GameObject, pos, Quaternion.identity);

				}

				requiredObject.name = gameObjectName.InnerText;
				requiredObject.transform.localScale = scale;

			}
		}

		popupMessage.text = "Data Loaded Successfully !!!";
		StartCoroutine (DisplayMessage ());
	}
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

	//================================================================================================
	// METHOD TO RETRIVE THE RELATIVE PATH AS DEVICE PLATFORM
	private string getPath(){
		#if UNITY_EDITOR
		return Application.dataPath + "/InterfaceList.xml";
		#elif UNITY_ANDROID
		return Application.persistentDataPath + "/InterfaceList.xml";
		#elif UNITY_IPHONE
		return Application.persistentDataPath + "/InterfaceList.xml";
		#else
		return Application.dataPath + "/XmlDocs/InterfaceList.xml";
		#endif
		}
	
	//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
}