using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作成者　富澤勇太
/// </summary>
[System.Serializable]
public class Magick {
    [System.Serializable]
    public class MagickBullet {
        public MagickBullet(Bullet bul,Bullet prefab) {
            bullet = bul;
            bulletPrefab = prefab;
            position = bul.transform.localPosition;
            rotation = bul.transform.localEulerAngles;
        }
        public Bullet bullet;
        [SerializeField]
        private Bullet bulletPrefab;
        [SerializeField]
		private AbnState state;
        public Vector3 position;
        public Vector3 rotation;
        //属性
        
        public int GetMP { get{ return bulletPrefab.useMP; } }
		public int GetDamage { get { return bulletPrefab.damage; } }

        /// <summary>
        /// 弾道の実行
        /// </summary>
        /// <param name="parent">親オブジェクト</param>
        /// <param name="isTestScene">シーンがテストシーンか否か</param>
        /// <returns></returns>
        public GameObject Enter(GameObject parent,bool isTestScene = false) {
            Instantiate(parent);
            bullet.Enter(isTestScene);
            return bullet.gameObject;
        }

        public void TestEnter(GameObject parent) {
			bullet.SetPrefab(bulletPrefab.gameObject);
			bullet.parent = parent.GetComponent<Character> ();
            bullet.Enter(true);
        }

        public void Save() {
			position = bullet.transform.localPosition;
            rotation = bullet.transform.localEulerAngles;
			state = bullet.GetAbnState ();
        }

		public MagickBullet GetClone(){
			return (MagickBullet)MemberwiseClone ();
		}

        public void Delete() {
            if (bullet) {
                GameObject.Destroy(bullet.gameObject);
                bullet = null;
            }
        }

        public void Instantiate(GameObject parent) {
            //弾道オブジェクトを消去し、新しく生成する
            //Delete();
            Bullet bulObj = Object.Instantiate(bulletPrefab.gameObject).GetComponent<Bullet>();//
            //bulObj.transform.SetParent(parent.transform);
			bulObj.parent = parent.GetComponent<Character>();
			bulObj.transform.SetParent (parent.transform);
			bulObj.transform.localPosition = position;
            if (bulObj.transform.localPosition.y < 0.5f) {
                Vector3 pos = bulObj.transform.localPosition;
                pos.y += 0.5f;
                bulObj.transform.localPosition = pos;
            }
            bulObj.transform.localEulerAngles = rotation;
            bullet = bulObj;
            bullet.SetPrefab(bulletPrefab.gameObject);
            bulObj.ChangeElement (state);
        }
    }
		
    [SerializeField]
    public List<MagickBullet> Bullets = new List<MagickBullet>();
    public string magickName;
    public Sprite magickIcon;

    public int GetMP {
		get {
			int mp = 0;
			foreach (MagickBullet mBul in Bullets)
				mp += mBul.GetMP;
			return mp;
		}
	}

	public int GetDamage {
		get {
			int damage = 0;
			foreach (MagickBullet mb in Bullets)
				damage += mb.GetDamage;
			return damage;
		}
	}

    /// <summary>
    /// 魔法の実行
    /// </summary>
    public void Enter(GameObject parent) {
        foreach (MagickBullet mBul in Bullets) {
            mBul.Enter(parent);
        }
    }

    public void TestEnter(GameObject parent) {
        foreach (MagickBullet mBul in Bullets) {
            mBul.TestEnter(parent);
        }
    }

    /// <summary>
    /// 弾道プレファブの追加
    /// </summary>
    /// <param name="bul">生成された弾道</param>
    public void AddBullet(Bullet bul,Bullet prefab) {
        Bullets.Add(new MagickBullet(bul,prefab));
    }

    /// <summary>
    /// 弾道プレファブの削除
    /// </summary>
    /// <param name="bul">削除する弾道</param>
    public void RemoveBullet(Bullet bul) {
        MagickBullet mBul = null;
        foreach (MagickBullet magickBullet in Bullets)
            if (magickBullet.bullet == bul) mBul = magickBullet;

        if (mBul != null)
            Bullets.Remove(mBul);
    }

    /// <summary>
    /// データの保存
    /// </summary>
    public void Save() {
        foreach (MagickBullet mb in Bullets) {
            mb.Save();
        }
    }

    /// <summary>
    /// コピーを生成して返す処理
    /// </summary>
    /// <returns></returns>
	public Magick GetClone(){
        Magick m = new Magick
        {
            magickIcon = magickIcon,
            magickName = magickName
        };

        foreach (MagickBullet mb in Bullets) {
			m.Bullets.Add (mb.GetClone ());
		}
		return m;
	}

    /// <summary>
    /// この魔法で参照している各種オブジェクトを削除する　データ読み込み時の、前に編集していたデータ破棄用
    /// </summary>
    public void Delete() {
        foreach (MagickBullet mb in Bullets) {
            mb.Delete();
        }
    }

    public void Instantiate(GameObject parent) {
        foreach (MagickBullet mb in Bullets) {
            mb.Instantiate(parent);
        }
    }

    public int Debug_GetBulletsCount() {
        return Bullets.Count;
    }
}
