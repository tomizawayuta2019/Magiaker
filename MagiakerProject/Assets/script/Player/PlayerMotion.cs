using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    //作成　佐藤竜也

    GameObject Player; //プレイヤー
    public Material[] Materials; //0:元のマテリアル　1~:変えるマテリアル

    [SerializeField]
    float interval = 0.0f;//点滅

    //[SerializeField]
    //private AudioSource Damage_SE; //ダメージ効果音

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    public IEnumerator Itenmetu()
    {
        for (int i = 0; i < 10; i++)
        {
            Player.GetComponent<Renderer>().material = Materials[1];
            yield return new WaitForSeconds(interval);
            Player.GetComponent<Renderer>().material = Materials[0];
            yield return new WaitForSeconds(interval);
        }
    }
    public void G_Damage()
    {
        //ダメージ処理
        StartCoroutine(Itenmetu());
        //Damage_SE.PlayOneShot(Damage_SE.clip);

    }
}
