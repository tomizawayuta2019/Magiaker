using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : Gimick {
    [SerializeField]
    float fadetime = 3.0f;

    void Start()
    {
        //フェードイン処理
        EnemyController.stop = true;
        FadeIn();
    }

    public void FadeOut()
    {
        // SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0f,
            "to", 1f,
            "time", fadetime,
            "onupdate", "SetValue"));
    }

    public void FadeIn()
    {
        // SetValue()を毎フレーム呼び出して、１秒間に１から０までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1f,
            "to", 0f,
            "time", fadetime,
            "onupdate", "SetValue",
            "oncomplete", "Stopfalse"));
    }
    void Stopfalse()
    {
		Destroy (MainSceneManager.hoziObject);
		//フェードINした後、プレイヤーと敵が動く
        EnemyController.stop = false;
		Debug.Log ("GameStart");
    }
    void SetValue(float alpha)
    {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }
}