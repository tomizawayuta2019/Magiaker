using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Create AbnStatemanager")]
public class AbnStateManager : ScriptableObject {
	//状態異常・属性の管理
	[SerializeField]
	public List<AbnState> fire,ice,electric;
	public GameObject fireObj, iceObj, electricObj;
	public GameObject[] AbnStateSymbol;

	/// <summary>
	/// 指定した条件の属性を取得する
	/// </summary>
	/// <returns>The element.</returns>
	/// <param name="ele">指定する属性</param>
	/// <param name="num">リスト内の格納番号</param>
	public AbnState GetElement(element ele,int num = 0){
		return GetStateList (ele) [num];
	}

	/// <summary>
	/// 指定した条件の属性を取得する
	/// </summary>
	/// <returns>The element.</returns>
	/// <param name="ele">指定する属性</param>
	/// <param name="state">同じパラメータの属性</param>
	public AbnState GetElement(element ele,AbnState state){
		return GetStateList (ele) [GetAbnStateNum (state)];
	}

	/// <summary>
	/// 属性の格納位置を調べる
	/// </summary>
	/// <returns>The abn state number.</returns>
	/// <param name="state">State.</param>
	private int GetAbnStateNum(AbnState state){
		foreach (var item in (GetStateList(state.type).Select((v,i) => new {v, i}))) {
			if (item.v == state)
				return item.i;
		}
		return 0;
	}

	/// <summary>
	/// 指定された属性のListを取得する
	/// </summary>
	/// <returns>The state list.</returns>
	/// <param name="type">Type.</param>
	private List<AbnState> GetStateList(element type){
		switch (type) {
		case element.fire:
			return fire;
		case element.ice:
			return ice;
		case element.electric:
			return electric;
		default:
			return fire;
		}
	}

	/// <summary>
	/// 弾丸オブジェクトのプレファブを取得する
	/// </summary>
	/// <returns>The element prefab.</returns>
	/// <param name="state">State.</param>
	public GameObject GetElementPrefab(element ele){
		GameObject obj;
		switch (ele) {
		case element.fire:
			obj = fireObj;
			break;
		case element.ice:
			obj = iceObj;
			break;
		case element.electric:
			obj = electricObj;
			break;
		default:
			obj = fireObj;
			break;
		}
		return obj;
	}
}
