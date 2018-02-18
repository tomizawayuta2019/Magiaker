using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージのFloorに適用しているマテリアルのタイリングを動的に変更するスクリプト
/// </summary>
public class FloorTiling : MonoBehaviour {
    private const float tilingPercent = 1f;//Tilingする際の倍率
    private Vector3 scale;
    private new Renderer renderer;//元のrendererは非表示にしても問題無い？問題あったら名前を変えること。
    public Vector3 defaultScale = Vector3.one;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null) {
            Destroy(gameObject);
        }
        TilingUpdate();
    }

    private void Update()
    {
        TilingUpdate();
    }

    /// <summary>
    /// Materialのタイリングサイズを変更する *ゲーム中でないとMaterialのインスタンスが生成されていないため、mainTexureが存在しないことに注意
    /// </summary>
    internal void TilingUpdate() {
        if (transform.lossyScale == scale) return;

        scale = transform.lossyScale;
        renderer.material.mainTextureScale = new Vector2(scale.x / defaultScale.x, scale.z / defaultScale.z) / tilingPercent;
    }
}