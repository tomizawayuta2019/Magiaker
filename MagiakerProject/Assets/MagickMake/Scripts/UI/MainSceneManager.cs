using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager {
	public static GameObject hoziObject;

	static public void OpenScene(string sceneName){
		if (hoziObject == null) {
			hoziObject = new GameObject ("GameRoot");
			GameObject.DontDestroyOnLoad (hoziObject);
		}

		foreach (GameObject obj in GameObject.FindObjectsOfType (typeof(GameObject))) {
			if (obj.transform.parent == null && obj != hoziObject)
				obj.transform.SetParent (hoziObject.transform);
		}
		hoziObject.SetActive (false);
		SceneManager.LoadScene (sceneName);
	}

	static public void CloseScene(){
		foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject))) {
			if (obj != hoziObject && obj.transform.parent == null)
				GameObject.Destroy (obj);
		}
		hoziObject.SetActive (true);
	}

	static public void EndScene () {
		GameObject.Destroy (hoziObject);
	}
}