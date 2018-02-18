using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum element {
	normal,
	fire,
	ice,
	electric,
}

[System.Serializable]
public class ElementValue<T>
{
    public T fire, ice, electric;
    public T GetValue(element element)
    {
        switch (element)
        {
            case element.ice:
                return ice;
            case element.electric:
                return electric;
            case element.fire:
            default:
                return fire;
        }
    }
}

public class ConstData {
    public const int MAGICK_COUNT = 9;//登録可能な魔法の個数
	public const float WEAK_DAMAGE_BONUS = 1.5f;//弱点属性によるダメージ倍率
    public const float BEAM_DAMAGE_DELAY = 0.3f;//ビーム等の継続ダメージ待機時間
    public const float BOMB_END_DELAY = 1.0f;//ボムの持続時間

	static public Color GetElementColor(element ele){
		switch (ele) {
		case element.fire:
			return Color.red;
		case element.ice:
			return Color.blue;
		case element.electric:
			return Color.yellow;
		default:
			return Color.black;
		}
	}

	public const string MagickMakeScene = "MagickMakeScene";
    public const string TitleScene = "Start";
}
