using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一部アニメーションが回転を変更したまま元に戻らず、モデルのみが別方向を向いてしまうことがある。
/// 対策として、指定したアニメーションの終了を検知し、規定の回転角と位置に戻す処理を設定する。
/// </summary>
public class ResetAnimationRotation : MonoBehaviour {
    [SerializeField]
    private string animationName;//対象のアニメーション名
    [SerializeField]
    private int animationLayerNum;//対象のアニメーションの配置されたレイヤー
    private Animator animator;//対象のアニメーションを実行するAnimator
    private bool isTargetAnimationPlaying;//指定したアニメーションを実行中か否か

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator) {
            Debug.LogAssertion(name +"に付与されたResetAnimationRotationの対象が参照できませんでした。");
        }
    }

    // Update is called once per frame
    void Update () {
        if (animator.GetCurrentAnimatorStateInfo(animationLayerNum).IsName(animationName)) {
            //Debug.Log("対象のアニメーションを実行中です");
            isTargetAnimationPlaying = true;
        } else if(isTargetAnimationPlaying){
            //Debug.Log("対象のアニメーションが終了しました");
            transform.localEulerAngles = Vector3.zero;
            isTargetAnimationPlaying = false;
        }
	}
}
