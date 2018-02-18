using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoResult : MonoBehaviour {
    public static bool isClear;

	static public void InitGotoResult(){
		GameObject obj = new GameObject ("result");
		obj.AddComponent<GotoResult> ();
		Character.stop = true;
        isClear = true;
        Instantiate(Resources.Load("Prefabs/GameClearCanvas"));
	}

    static public void GameOver() {
        GameObject obj = new GameObject("result");
        obj.AddComponent<GotoResult>();
        Character.stop = true;
        isClear = false;
        Instantiate(Resources.Load("Prefabs/GameOverCanvas"));
    }

    const float waitTime = 0.5f;//UIが表示されてから入力が可能になるまでの時間
    private float time;
	
	// Update is called once per frame
	void Update () {
        if ((time += Time.deltaTime) >= waitTime) {
            if (Input.GetMouseButtonDown(0))
            {
                //Destroy (MainSceneManager.hoziObject);
                PlayerController.InitState();
                SceneManager.LoadScene("Result");
            }
        }
	}
}
