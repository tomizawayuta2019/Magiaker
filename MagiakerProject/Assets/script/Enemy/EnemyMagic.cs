using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagic : MonoBehaviour
{
	Rigidbody rb;
	[SerializeField]
	int LifeLimit = 5;
	int TimeSet = 0;
	private void Start()
	{
		LifeLimit *= 60;

		StartCoroutine(DestroyObj());
	}
	IEnumerator DestroyObj()
	{
		for (;;)
		{
			TimeSet++;
			if (LifeLimit <= TimeSet)
			{
				Destroy(gameObject);
			}
			yield return null;
		}

	}
	// Update is called once per frame
	private void OnEnable()
	{
		StartCoroutine(DestroyObj());
	}
}
