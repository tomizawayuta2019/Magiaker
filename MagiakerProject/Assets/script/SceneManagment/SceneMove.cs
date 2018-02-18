using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Example/SceneManage")]
public class SceneMove : ScriptableObject {

    /// <summary>
    /// シーンの読み込み
    /// </summary>
    /// <param name="num"></param>
    public void LoadScene(int num) {
        SceneManager.LoadScene(num);
    }

    /// <summary>
    /// メインシーンへ進む際、難易度選択を行う
    /// </summary>
    /// <param name="num"></param>
    public void SelectMainSceneLevel(int num) {
        //SceneManager.LoadScene(MainSceneNum);
        MainSceneManager.StartMainScene((Level)System.Enum.ToObject(typeof(Level), num));
    }
}
