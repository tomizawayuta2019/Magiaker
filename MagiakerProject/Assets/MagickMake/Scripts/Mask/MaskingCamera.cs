using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskingCamera : MonoBehaviour {
    public static Camera maskCamera;
    public static MaskingCamera instance;
    const int MASK_LAYER_NUMBER = 10;

    [SerializeField]
    private GameObject MaskObject;
    [SerializeField]
    private OnVisivle MaskSprite;

    private void Awake()
    {
        maskCamera = this.GetComponent<Camera>();
        instance = this;
    }

    public void Update()
    {
        //Debug.Log(isMaskingSpot);
    }

    public void Active(bool value) {
        //MaskObject.SetActive(value);
        MaskSprite.SetVivivlity(value);
    }

    float time;
    bool _value;
    public bool isMaskingSpot {
        get {
            if (time != Time.time) {
                time = Time.time;
                _value = IsMaskingSpot();
            }
            return _value;
        } }

    /// <summary>
    /// 現在のマウスの指し示す位置がマスク内か否か
    /// </summary>
    /// <returns></returns>
    private bool IsMaskingSpot() {
        RaycastHit hit;
        Ray ray = maskCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.gameObject.layer == MASK_LAYER_NUMBER)
            {
                return true;
            }
        }
        return false;
    }
}
