using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Toge : Gimick
{
    //作成　佐藤竜也
    [SerializeField]
    float InvincibleTime = 2.0f;
    private float inv_time;

    [SerializeField]
    float Damage = 10.0f;
    
    //このスクリプトがついてるものをギミックと定義
    aligment aligment = aligment.gimmick;

    GameObject Player;

    AudioSource audioSource;
    public AudioClip GimickDamage;

    PlayerMotion PM;




    private void Start()
    {
        //初期ダメージ
        inv_time = InvincibleTime;
        Player = GameObject.FindWithTag("Player");
        PM = Player.GetComponent<PlayerMotion>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    //トゲでダメージする処理
    void OnTriggerStay(Collider other)
    {
        //無敵処理
        if (inv_time <= InvincibleTime)
        {
            if (other.gameObject.tag == Tags.Player)
            {
                inv_time += Time.deltaTime;
            }
        }
        //ダメージ処理
        else
        {
            if (other.gameObject.tag == Tags.Player)
            {
                //Debug.Log(Damage);
                audioSource.PlayOneShot(GimickDamage);
                inv_time = 0.0f;
                G_TogeDamage(other);
                //PM.G_Damage();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //抜けた後すぐダメージ
        if(other.gameObject.tag == Tags.Player)
        {
            inv_time = InvincibleTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        G_TogeDamage(collision);
    }

    void G_TogeDamage(Collision other) {
        Character target = other.gameObject.GetComponent<Character>();
        if (target)
        {
            if (!target.isAligment(aligment))
            {
                Vector3 hitpos = transform.position;
                foreach (ContactPoint pos in other.contacts)
                {
                    hitpos = pos.point;
                }
                target.TakeAttack(Damage, hitpos);
            }
        }
    }

    void G_TogeDamage(Collider other)
    {
        
        Character target = other.GetComponent<Character>();
        if (target)
        {
            if (!target.isAligment(aligment))
            {
                target.TakeAttack(Damage, other.ClosestPointOnBounds(transform.position));
            }
        }
    }
}