using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : SingletonMonoBehaviour<Menu> {
    private static float? defaultTimeScale;//時間を止める前のタイムスケール
    const float menuTImeScale = 0;//メニュー画面を開いている際のタイムスケール
    const string Prefab = "Prefabs/MenuCanvas";//メニュー画面のプレファブ
    public AudioClip OpenSE;

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// メニュー画面を生成した際、メニュー画面を再度アクティブにした際に呼び出される
    /// </summary>
    private void OnEnable()
    {
        OpenMenu();
    }

    /// <summary>
    /// タイムスケールの初期化
    /// </summary>
    private static void InitTImeScale() {
        if (!defaultTimeScale.HasValue) {
            defaultTimeScale = Time.timeScale;
        }
    }

    /// <summary>
    /// メニュー画面の生成と時間の停止
    /// </summary>
    public static void OpenMenu() {
        InitTImeScale();
        Time.timeScale = 0;
        //Character.stop = true;
        if (Instance == null) {
            Instantiate( Resources.Load<GameObject>(Prefab));
        }
    }

    /// <summary>
    /// メニュー画面の削除と時間停止の解除
    /// </summary>
    public static void CloseMenu() {
        Time.timeScale = defaultTimeScale.Value;
        //Character.stop = false;
        Destroy(Instance.gameObject);
    }

    //以下はボタンからのアクセス用

    /// <summary>
    /// ゲーム再開
    /// </summary>
    public void Close() {
        CloseMenu();
    }

    /// <summary>
    /// 魔法作成
    /// </summary>
    public void GotoMagicMakeScene() {
        CloseMenu();
        MainSceneManager.OpenScene(ConstData.MagickMakeScene);
    }

    /// <summary>
    /// 最初から
    /// </summary>
    public void RestartMainScene() {
        CloseMenu();
        PlayerController.InitState();
        MainSceneManager.StartMainScene(MainSceneManager.GetLevel());
    }

    /// <summary>
    /// タイトルへ戻る
    /// </summary>
    public void ReturnToTitle() {
        CloseMenu();
        SceneManager.LoadScene(ConstData.TitleScene);
    }
}
