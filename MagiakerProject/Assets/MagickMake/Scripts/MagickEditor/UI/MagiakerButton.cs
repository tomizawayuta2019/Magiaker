using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MagiakerButton : MonoBehaviour {

    public Button button;
    [HideInInspector]
    public RectTransform rect;
    [SerializeField]
    public List<Image> images;
    [SerializeField]
    public List<Text> texts;
    [SerializeField]
    private AudioClip PushSE;

    protected virtual void Start() {
        if (button == null) {
            button = GetComponent<Button>();
        }
        if (PushSE)
        {
            button.onClick.AddListener(() => SEManager.SetSE(PushSE));
        }
        else {
            button.onClick.AddListener(() => SEManager.SetSE(MagicSystemManager.instance.SEManager.ButtonPush));
        }
        
    }

    /// <summary>
    /// ボタンに表示する文字・画像の初期化
    /// </summary>
    /// <param name="strings">文字列リスト</param>
    /// <param name="sprites">画像リスト</param>
    public MagiakerButton Init(List<string> strings,List<Sprite> sprites) {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();

        if (strings != null)
            for (int i = 0; i < texts.Count && i < strings.Count; i++) {
                if (texts[i]) texts[i].text = strings[i];
            }

        if (sprites != null)
            for (int i = 0; i < images.Count && i < sprites.Count; i++) {
                if (images[i]) images[i].sprite = sprites[i];
            }

        return this;
    }
}
