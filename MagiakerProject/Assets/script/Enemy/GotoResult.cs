using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoResult : MonoBehaviour {

	static public void InitGotoResult(){
		GameObject obj = new GameObject ("result");
		obj.AddComponent<GotoResult> ();
		Character.stop = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Destroy (MainSceneManager.hoziObject);
			SceneManager.LoadScene ("Result");
		}
	}
}
