using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BeamColider : MonoBehaviour {
    [SerializeField]
    private float targetDistance;

	// Use this for initialization
	void Start () {
        StartCoroutine(ChangeColliderSize(GetComponent<BoxCollider>(), targetDistance, 0.5f));
	}

    IEnumerator ChangeColliderSize(BoxCollider target,float targetDIstance,float time) {
        float distance = targetDIstance - target.size.z;
        float oldDistance = target.size.z;
        float t = 0;
        while (target.size.z != targetDistance) {
            t += Time.deltaTime;
            if (time >= t)
            {
                target.size = new Vector3(target.size.x, target.size.y, targetDistance);
            }
            else {
                target.size = new Vector3(target.size.x, target.size.y, oldDistance + (distance * (t / time)));
            }
            yield return null;
        }
    }
	
}
