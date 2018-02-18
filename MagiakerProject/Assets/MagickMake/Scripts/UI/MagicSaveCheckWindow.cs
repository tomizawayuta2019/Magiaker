using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSaveCheckWindow : SingletonMonoBehaviour<MagicSaveCheckWindow> {
    public enum CheckType {
        reset,
        load,
        close,
    }

    const string prefabName = "Prefabs/MagicSaveCheckWindow";
    public GameObject resetCheck, returnCheck,loadCheck;
    public static CheckType checkType;

    private void Start()
    {
        resetCheck.SetActive(checkType == CheckType.reset);
        returnCheck.SetActive(checkType == CheckType.close);
        loadCheck.SetActive(checkType == CheckType.load);
    }

    public void YesButton() {
        switch (checkType) {
            case CheckType.close:
                MagickMakeManager.Instance.CloseScene(true);
                break;
            case CheckType.load:
                MagickMakeManager.Instance.LoadMagick(true);
                break;
            case CheckType.reset:
                MagickMakeManager.Instance.ResetMagick(true);
                break;
        }
        Destroy(gameObject);
    }

    public void NoButton() {
        Destroy(gameObject);
    }

    public static void SaveCheck(CheckType checkType) {
        MagicSaveCheckWindow.checkType = checkType;
        Instantiate(Resources.Load<GameObject>(prefabName));
    }
}
