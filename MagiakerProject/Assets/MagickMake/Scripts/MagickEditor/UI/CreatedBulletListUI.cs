using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatedBulletListUI : ButtonList
{
    private int bulletsCount;
    private Magick selectMagick;

    [SerializeField]
    private Text text;//ビルド後のデータで、このTextを消去するとスクロールバー内が表示されない場合あり。

    protected override void Update()
    {
        base.Update();
        if (selectMagick != MagickMakeManager.Instance.magick)
            SelectMagicDate(MagickMakeManager.Instance.magick);

        //弾道が追加・削除されていたら更新する
        if (IsButtonUpdate())
        {
            AddButton();
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i < selectMagick.Bullets.Count)
                {
                    if (buttons[i].texts[0].text != selectMagick.Bullets[i].bullet.name)
                    {
                        buttons[i].texts[0].text = selectMagick.Bullets[i].bullet.GetBulletName();
                    }
                }
                else
                {
                    //ボタンの数が多ければ、必要無い分は削除する
                    MagiakerButton button = buttons[i--];
                    buttons.Remove(button);
                    Destroy(button.gameObject);
                }
            }
            bulletsCount = selectMagick.Bullets.Count;
        }
        else if (selectMagick == null)
        {
            MagiakerButton button;
            //魔法が存在しないなら全てのボタンを消去する
            while (buttons.Count > 0)
            {
                button = buttons[0];
                buttons.Remove(button);
                Destroy(button.gameObject);
            }
        }

        text.text = "aaa";//buttons[0].transform.localPosition.ToString();
    }

    private bool IsButtonUpdate()
    {
        return (selectMagick != null && bulletsCount != selectMagick.Bullets.Count);
    }

    private void AddButton()
    {
        //ボタンが足りなければ追加する
        while (buttons.Count < selectMagick.Bullets.Count)
        {
            int num = buttons.Count;
            MagiakerButton button = AddButton(new List<string>(), new List<Sprite>());
            button.button.onClick.AddListener(() => OnClick(num));
        }
    }

    private void DeleteBUtton()
    {

    }

    public void SelectMagicDate(Magick magick)
    {
        selectMagick = magick;
        bulletsCount = -100;
    }

    private void OnClick(int num)
    {
        MagickMakeManager.Instance.BulletSelect(selectMagick.Bullets[num].bullet);
    }
}
