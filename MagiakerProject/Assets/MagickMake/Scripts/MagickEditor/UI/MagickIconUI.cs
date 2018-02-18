using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MagickIconUI : ButtonList {
    public MagickIconManager iconManager;

    protected override void Start()
    {
        base.Start();
        List<List<string>> strings = new List<List<string>>();
        List<List<Sprite>> sprites = new List<List<Sprite>>();

        foreach (Sprite s in iconManager.Sprites) {
            sprites.Add(new List<Sprite>() { s });
        }

        MakeButtons(strings, sprites);

        foreach (var item in buttons.Select((v, i) => new { v, i })) {
            item.v.button.onClick.AddListener(() => OnClick(item.v.button.GetComponent<Image>().sprite));
            //アイコンの画像サイズに合わせる
            //item.v.rect.sizeDelta = item.v.images[0].sprite.rect.size;
        }
    }

    public void OnClick(Sprite sprite) {
        MagickMakeManager.Instance.magick.magickIcon = sprite;
        Destroy(gameObject);
    }

    public void CloseMenu() {
        Destroy(gameObject);
    }

}
