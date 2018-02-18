using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCenter : MonoBehaviour {

	public GameObject bullet;
	public GameObject target;
    public GameObject muzzle;
    public int bulletNumber;
	public float waitSec = 2.0f;
	public List<GameObject> bulletList;

    public int bulletSpeed;
    private int count;
	private GameObject bullets;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(MakeCoroutine());
        count = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(bulletList.Count == 0 && bulletNumber == count) 
		{
			Destroy(gameObject);
		}
	}

	IEnumerator MakeCoroutine() 
	{
		yield return new WaitForSeconds(waitSec);
		MakeBullet();
	}

	void MakeBullet()
	{
		bullets = Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
        bulletSpeed = bulletNumber * 11;
		bulletList.Add(bullets);
		bullets.transform.parent = target.transform;
        count++;
		bool bulletBool = true;
		if (bulletBool && count <= bulletNumber)
		{
		    StartCoroutine(MakeCoroutine());
            
			bulletBool = false;
		}
	}
}
