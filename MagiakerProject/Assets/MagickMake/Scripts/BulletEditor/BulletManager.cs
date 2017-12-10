using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作成者　富澤勇太
/// </summary>
[CreateAssetMenu(menuName = "Example/Create BulletManager Instance")]
public class BulletManager : ScriptableObject {
	//弾道データの管理
    [SerializeField]
    public List<Bullet> Bullets;

	/// <summary>
	/// 空のデータが入っていたら削除する
	/// </summary>
    public void Init() {
        List<Bullet> newBull = new List<Bullet>();
        foreach (Bullet bul in Bullets)
            if (bul) newBull.Add(bul);
        Bullets = newBull;
    }

    /// <summary>
    /// Bulletプレファブの読み込み
    /// </summary>
    /// <param name="name">読み込むプレファブの名前</param>
    /// <returns>ロードしたプレファブのGameObject　存在しなかったらnull</returns>
    public GameObject Load(string name) {
        foreach (Bullet bul in Bullets) {
            if (bul.name == name) return bul.gameObject;
        }
        return null;
    }

    /// <summary>
    /// Bulletプレファブの保存
    /// </summary>
    /// <param name="bul">保存するBullet</param>
    public void Save(Bullet bul) {
		#if UNITY_EDITOR
        int num = isBulletExist(bul.name);
        if (num < 0)
            Bullets
				.Add(
					UnityEditor.
					PrefabUtility
					.CreatePrefab("Assets/MagickMake/Resources/Prefabs/" + 
						bul.name + ".prefab", 
						bul.gameObject)
					.GetComponent<Bullet>());
        else
            Bullets[num] = UnityEditor.PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/" + bul.name + ".prefab", bul.gameObject).GetComponent<Bullet>();

        Debug.Log("Save");
		#endif
    }

    /// <summary>
    /// 同じ名前のBulletプレファブが既に存在するか確認
    /// </summary>
    /// <param name="name">確認する名前</param>
    /// <returns>存在するなら、そのBulletの番号　存在しないなら、常に-1</returns>
    public int isBulletExist(string name) {
        for(int i = 0;i < Bullets.Count;i++)
            if (Bullets[i].name == name) return i;
            
        return -1;
    }
}
