using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArea : Gimick {
    [SerializeField]
    float waittime; //フェードアウト中の待ち時間
    [SerializeField]
    Fade fade;
    [SerializeField]
    Vector3 PlayerVec;//Playerの位置に+したい値
    [SerializeField]
    float gototime;//終了位置まで向かうまでの時間


    //playerがボスシーン移動位置に入ったら
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.Player) {
            EnemyController.stop = true;
            StartCoroutine(MovePlayer(other.gameObject));
        }
    }

    //playerが移動してフェードアウト
    IEnumerator MovePlayer(GameObject player)
    {
        yield return StartCoroutine(Goto_position(player));
        fade.FadeOut();
        yield return new WaitForSeconds(waittime);
        SceneManager.LoadScene("Boss");
		MainSceneManager.EndScene ();
    }

    //playerの移動スクリプト
    IEnumerator Goto_position(GameObject player)
    {
        //最初の距離
        Vector3 StartPos = player.transform.position;
        //終わりの距離
        Vector3 pos = player.transform.position + PlayerVec;
        float diff = 0.0f;
        for (;;)
        {
            diff += Time.deltaTime;
            float rate = diff / gototime;
            player.transform.position = Vector3.Lerp(StartPos, pos, rate);
            yield return null;
            //プレイヤーが終わりの位置にたどり着いたら終了
            if(player.transform.position == pos)
            {
                break;
            }
        }
    }



    //IEnumerator bossPlayer(GameObject player)
    //{
    //    Debug.Log(player.transform.position);
    //    //目的地へ
    //    iTween.MoveTo(player, iTween.Hash(
    //        "x", transform.position.x + PlayerVec.x,
    //        "z", transform.position.z + PlayerVec.z,
    //        "time", 5,
    //        "oncomplete", "fade.FadeOut", 
    //        "oncompletetarget", gameObject
    //            ));
    //    yield return new WaitForSeconds(waittime);
    //    Debug.Log(player.transform.position);
    //    yield return null;
    //    SceneManager.LoadScene("Boss");
    //}
}
