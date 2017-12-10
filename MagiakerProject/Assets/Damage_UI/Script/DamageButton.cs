using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour {

    public FadeOutText fade;

    public GameObject damageUI;

    public void Red()
    {
        damageUI = Resources.Load("Prefab/Damage UI") as GameObject;
        damageUI = Instantiate(damageUI);
        fade = damageUI.GetComponentInChildren<FadeOutText>();
        fade.damageType = FadeOutText.DamageType.Player_Damage;
        Destroy(damageUI, 2.0f);
    }

    public void Green()
    {
        damageUI = Resources.Load("Prefab/Damage UI") as GameObject;
        damageUI = Instantiate(damageUI);
        fade = damageUI.GetComponentInChildren<FadeOutText>();
        fade.damageType = FadeOutText.DamageType.Player_HPHeal;
        Destroy(damageUI, 2f);
    }

    public void Pink()
    {
        damageUI = Resources.Load("Prefab/Damage UI") as GameObject;
        damageUI = Instantiate(damageUI);
        fade = damageUI.GetComponentInChildren<FadeOutText>();
        fade.damageType = FadeOutText.DamageType.Player_MPHeal;
        Destroy(damageUI, 2f);
    }

    public void White()
    {
        damageUI = Resources.Load("Prefab/Damage UI") as GameObject;
        damageUI = Instantiate(damageUI);
        fade = damageUI.GetComponentInChildren<FadeOutText>();
        fade.damageType = FadeOutText.DamageType.Enemy_Damage;
        Destroy(damageUI, 2f);
    }

    public void Yellow()
    {
        damageUI = Resources.Load("Prefab/Damage UI") as GameObject;
        damageUI = Instantiate(damageUI);
        fade = damageUI.GetComponentInChildren<FadeOutText>();
        fade.damageType = FadeOutText.DamageType.Enemey_WeakDamage;
        Destroy(damageUI, 2f);
    }

    void Update()
    {

    }
}
