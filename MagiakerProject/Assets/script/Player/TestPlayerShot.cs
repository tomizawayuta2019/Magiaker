using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerShot : MonoBehaviour {
    public GameObject fireshot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      if (Input.GetMouseButtonDown(0))
      {
          //GameObject Bullets = null;
          //弾を生成する
          Instantiate(fireshot, transform.position, Quaternion.identity);

      }
    }
}
