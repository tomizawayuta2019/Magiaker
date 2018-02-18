using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBullet : MonoBehaviour {

	public GameObject target;
	//public float angle = 30.0f;
	public float speed;
    public float? elapsedTIme;

	private RotateCenter rotateCenter;
	//private Vector3 vec3;

	// Use this for initialization
	void Start () 
	{
        if (target) {
            SetTarget(target);
        }
	}

    /// <summary>
    /// 追従対象の設定
    /// </summary>
    public void SetTarget(GameObject value) {
        target = value;
        defPos = target.transform.position;
    }

    public void SetTime(float? value) {
        elapsedTIme = value;
    }

    private Vector3 defPos;

	// Update is called once per frame
	void Update () 
	{
        if (!target) return;

        //対象が動いた分だけ自分も動く
        Vector3 distance = target.transform.position - defPos;
        defPos = target.transform.position;
        transform.position = transform.position + distance;

        //対象との距離を保ちつつ周囲を回転する
		transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);

        if (elapsedTIme.HasValue) {
            elapsedTIme = elapsedTIme - Time.deltaTime;
            if (elapsedTIme <= 0) {
                Destroy(gameObject);
            }
        }
	}
}
