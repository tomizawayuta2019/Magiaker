using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/SEManager")]
public class SEManager : ScriptableObject {

    public AudioSource ButtonPush,//ボタンを押した際の音
                       Heal,//回復音
                     Damage;//ダメージ音

    [System.Serializable]
    public class ELementAudio : ElementValue<AudioSource>{}//seriarize用のClass宣言

    public ELementAudio elementShot,//属性ごとの通常魔法
                        elementBomb,//属性ごとの爆発音
                        elementBeam;//属性ごとのビーム

    public AudioSource mizugorowaShot;


    private class CalledSE {
        const float interval = 0.1f;//短い期間に連続で同じ音が鳴る場合、音の数を減らす 
        private float time;//この音が鳴った時間
        public AudioClip audio;

        public CalledSE(AudioClip audio) {
            this.audio = audio;
            time = Time.time;
        }

        public bool IsInterval() {
            return Time.time - time > interval;
        }
    }

    private static List<CalledSE> seList = new List<CalledSE>();

    /// <summary>
    /// 引数のSEを鳴らしても良いか確認する
    /// </summary>
    /// <param name="audio"></param>
    /// <returns></returns>
    public static bool IsCanPlayAudio(AudioClip audio) {
        if (!audio) return false;

        //Debug.Log("isplay");

        //foreach (var se in seList.Select((v, i) => new { v, i }))
        //{
        //    Debug.Log(se.v.audio.name);
        //    if (se.v.audio == audio) {
        //        if (!se.v.IsInterval())
        //        {
        //            Debug.Log("interval");
        //            return false;
        //        }
        //        else {
        //            seList.Remove(se.v);
        //            seList.Add(new CalledSE(audio));
        //            Debug.Log("interval");
        //            return true;
        //        }
        //    }
        //}

        //seList.Add(new CalledSE(audio));

        return true;
    }

    /// <summary>
    /// 指定されたSEを鳴らす
    /// </summary>
    /// <param name="value">鳴らすSE</param>
    public static void SetSE(AudioClip value,GameObject parent = null,bool loop = false) {
        if (!IsCanPlayAudio(value)) return;

        AudioSource audio = new GameObject(value.name).AddComponent<AudioSource>();
        audio.clip = value;
        audio.playOnAwake = true;
        audio.loop = loop;
        audio.Play();
        audio.gameObject.AddComponent<DestroySE>();

        if (parent) {
            audio.transform.SetParent(parent.transform);
        }
    }

    /// <summary>
    /// 指定されたSEを鳴らす
    /// </summary>
    /// <param name="value">鳴らすSE</param>
    public static void SetSE(AudioSource value, GameObject parent = null, bool loop = false)
    {
        if (!IsCanPlayAudio(value.clip)) return;

        AudioSource audio = Instantiate(value);
        audio.gameObject.AddComponent<DestroySE>();
        audio.Play();

        if (parent) {
            audio.transform.SetParent(parent.transform);
            audio.transform.localPosition = Vector3.zero;
        }
    }
}
