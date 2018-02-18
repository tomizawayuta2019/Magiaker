using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(G_Toge))]
public class G_Move : Gimick
{
    //作成　佐藤竜也
    [SerializeField]
    float WaitTime = 0.0f; // 待機時間
    [SerializeField]
    Vector3 EndPos; //目標地点

    private void Start()
    {
        Move_go();
        //StartCoroutine(MoveGimmick());
    }


    void Move_go()
    {
        //目的地へ
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", transform.position.x + EndPos.x,
            "z", transform.position.z + EndPos.z,
            "time", WaitTime,
            "oncomplete", "Come_go",　//動き終わったらCome_goへ
            "oncompletetarget", gameObject
            ));

    }
    void Come_go()
    {
        //スタート位置へ
        iTween.MoveTo(gameObject, iTween.Hash(
       "x", transform.position.x - EndPos.x,
       "z", transform.position.z - EndPos.z,
       "time", WaitTime,
       "oncomplete", "Move_go", //動き終わったらMove_goへ
       "oncompletetarget", gameObject
       ));
    }


    ////動くギミックの動作
    //IEnumerator MoveGimmick()
    //{
    //    for (;;)
    //    {
    //        yield return StartCoroutine(Goto_position());
    //    }
    //}
    //IEnumerator Goto_position()
    //{
    //    //最初の距離
    //    Vector3 StartPos = transform.position;
    //    //終わりの距離
    //    Vector3 pos = transform.position + EndPos;
    //    float diff = 0.0f;
    //    for (;;)
    //    {
    //        diff += Time.deltaTime;
    //        float rate = diff / MoveSpeed;
    //        transform.position = Vector3.Lerp(StartPos, pos, rate);
    //        yield return null;
    //    }
    //}
}