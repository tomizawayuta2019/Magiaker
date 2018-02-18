using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//難易度
public enum Level {
    easy,
    normal,
    hard,
}

[System.Serializable]
public class LevelValue<T> {
    public T easy, normal, hard;
    public T GetValue() {
        switch (MainSceneManager.GetLevel()) {
            case Level.normal:
                return normal;
            case Level.hard:
                return hard;
            case Level.easy:
            default:
                return easy;
        }
    }
}
[System.Serializable]
public class LevelInt : LevelValue<int> { }
[System.Serializable]
public class LevelFloat : LevelValue<float> { }

public class MainSceneManager {
    public const int MainSceneNum = 2;

    private static Level _level;
    public static Level GetLevel() { return _level; }
    
    //難易度の管理
    static public void StartMainScene(Level value) {
        _level = value;
        PlayerController.InitState();
        SceneManager.LoadScene(MainSceneNum);
    }


    //シーン遷移の管理
	public static GameObject hoziObject;
    private static Scene mainScene;//移動元のシーン
    private static string sceneName;

	static public void OpenScene(string targetSceneName){
        sceneName = targetSceneName;
		if (hoziObject == null) {
			hoziObject = new GameObject ("GameRoot");
			//GameObject.DontDestroyOnLoad (hoziObject);
		}
		foreach (GameObject obj in GameObject.FindObjectsOfType (typeof(GameObject))) {
            if (obj.transform.parent == null && obj != hoziObject && !obj.GetComponent<BGMManager>() && !obj.GetComponent<UnityEngine.EventSystems.EventSystem>()) {
                obj.transform.SetParent(hoziObject.transform);
            }
		}
        foreach (EnemyController enemy in GameObject.FindObjectsOfType(typeof(EnemyController))) {
            enemy.InitFlag();
        }
		hoziObject.SetActive (false);
        
        mainScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneByName(sceneName);
        WaitLoadScene wait = new GameObject().AddComponent<WaitLoadScene>();
        wait.StartCoroutine(wait.WaitForSceneLoad(newScene));
    }

    /// <summary>
    /// 
    /// </summary>
    internal class WaitLoadScene : MonoBehaviour {
        /// <summary>
        /// シーンのロードを待機し、ロードが終了したらそのシーンをアクティブにする
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        internal IEnumerator WaitForSceneLoad(Scene scene)
        {
            while (!scene.isLoaded)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(scene);
            Destroy(gameObject);
        }
    }

	static public void CloseScene(){
		//foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject))) {
		//	if (obj != hoziObject && obj.transform.parent == null)
		//		GameObject.Destroy (obj);
		//}
        //Scene magickScene = SceneManager.GetActiveScene();
        //Debug.Log("現在アクティブなシーンは" + magickScene.name + "です");
        SceneManager.SetActiveScene(mainScene);
        SceneManager.UnloadSceneAsync(sceneName);
		hoziObject.SetActive (true);
        while (hoziObject.transform.childCount != 0) {
            hoziObject.transform.GetChild(0).parent = null;
        }
        GameObject.Destroy(hoziObject);
	}

	static public void EndScene () {
		//GameObject.Destroy (hoziObject);
	}
}