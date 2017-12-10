using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedMagickData {
    static public List<Magick> CreatedMagicks;//作成された魔法の情報を取得し、ここに格納しておく

    static public void Init() {
        CreatedMagicks = new List<Magick>();
    }
}
