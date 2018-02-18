using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystemManager : MonoBehaviour {
    public static MagicSystemManager instance {
        get {
            return _instance; }
        set {
            if (_instance != null)
            {
                Destroy(value.gameObject);
            }
            else
            {
                _instance = value;
                DontDestroyOnLoad(value.gameObject);
            }
        }
    }
    private static MagicSystemManager _instance;

    public AbnStateManager _abnstateManager;//状態異常や属性の管理用
    public CreatedMagickData createdMagickData;//現在保存している魔法作成履歴
    public DefaultMagickData defaultMagickData;//初期保持魔法
    public SEManager SEManager;

    private void Awake()
    {
        instance = this;
        createdMagickData.Init();
    }
}
