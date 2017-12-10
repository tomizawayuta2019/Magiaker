using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager {
    public const string PrefabPath = "Prefab/Damage UI";
    private static GameObject prefab;
	//ダメージUIの表示場所　対象の距離に＋する
	static public Vector3 GetDamageUIDeltaPos{ get{ return new Vector3 (0, 1, 0); } }

    static public GameObject Instantiate(float value,FadeOutText.DamageType type) {
        if (prefab == null)
            prefab = Resources.Load(PrefabPath) as GameObject;
        GameObject UIObj = Object.Instantiate(prefab);
        FadeOutText fade = UIObj.GetComponentInChildren<FadeOutText>();
        fade.damageType = type;
        fade.fadeOutText.text = value.ToString();
        Object.Destroy(UIObj, 2.0f);
        return UIObj;
    }
}
