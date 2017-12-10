using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum element {
	normal,
	fire,
	ice,
	electric,
}

public class ConstData {
    public const int MAGICK_COUNT = 9;//登録可能な魔法の個数
	public const float WEAK_DAMAGE_BONUS = 1.5f;//弱点属性によるダメージ倍率
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
}
