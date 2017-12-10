using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 東谷 太喜
/// </summary>
public class FadeOutText : MonoBehaviour
{

    public GameObject damageUI;

    public Text fadeOutText;
    public float alpha;

    private float division = 1 / 255f;

    public enum DamageType
    {
        Player_Damage = 1,
        Player_HPHeal,
        Player_MPHeal,
        Enemy_Damage,
        Enemey_WeakDamage
    }
    public DamageType damageType;

    // Use this for initialization
    void Start()
    {
        fadeOutText = GetComponent<Text>();
        alpha = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(damageType);
        //UIのカラーの設定とフェード表示
            switch (damageType)
            {
                //Playerの被ダメージ時(Red)
                case DamageType.Player_Damage:
                    alpha -= 1.0f * Time.deltaTime;
                    fadeOutText.color = new Color(1, 0, 0, alpha);
                    break;

                //PlayerのHP回復時(Green)
                case DamageType.Player_HPHeal:
                    alpha -= 1.0f * Time.deltaTime;
                    fadeOutText.color = new Color(0, 128, 0, alpha);
                    break;

                //PlayerのMP回復時(Pink)
                case DamageType.Player_MPHeal:
                    alpha -= 1.0f * Time.deltaTime;
                    fadeOutText.color = new Color(255f, 132f * division, 214f * division ,alpha);
                    break;

                //Enemyの被ダメージ時(White)
                case DamageType.Enemy_Damage:
                    alpha -= 1.0f * Time.deltaTime;
                    fadeOutText.color = new Color(1, 1, 1, alpha);
                    break;

                //Enemeyの弱点での被ダメージ時(Yellow)
                case DamageType.Enemey_WeakDamage:
                    alpha -= 1.0f * Time.deltaTime;
                    fadeOutText.color = new Color(255, 255, 0, alpha);
                    break;
            }
    }
}
