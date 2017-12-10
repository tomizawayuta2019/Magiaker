using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作成　佐藤 竜也
public class OnMouse : MonoBehaviour
{
    public GameObject mousePosObj;//mousePosObj
    Vector3 screenPoint;//ゲーム内のマウスのvector
    private Vector3 screenToWorldPointPosition; // スクリーン座標をワールド座標に変換した位置座標

    void Update()
    {
        if(!Character.stop)
        MouseLook();
    }

    void MouseLook()
    {
        ////マウス
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 a = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        mousePosObj.transform.position = Camera.main.ScreenToWorldPoint(a);
        //playerをマウスの位置に回す
        transform.LookAt(new Vector3(mousePosObj.transform.position.x, transform.position.y, mousePosObj.transform.position.z));
        //マウスここまで
    }
}