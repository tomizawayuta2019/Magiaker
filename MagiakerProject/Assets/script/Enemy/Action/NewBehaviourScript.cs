using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    GameObject ParentObj;
    private void Start()
    {
        ParentObj = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ParentObj)
        {
            NavMeshAgent objAgent = other.gameObject.GetComponent<NavMeshAgent>();
            if (objAgent)
            {
                objAgent.Stop();
                Vector3 velocity = Vector3.zero;
                if (Mathf.Abs(objAgent.velocity.x) >= 1)
                { velocity.x = objAgent.velocity.x / 2; }
                if (Mathf.Abs(objAgent.velocity.y) >= 1)
                { velocity.y = objAgent.velocity.y / 2; }
                if (Mathf.Abs(objAgent.velocity.z) >= 1)
                { velocity.z = objAgent.velocity.z / 2; }
                objAgent.velocity = velocity;
                Debug.Log(objAgent.velocity);
            }
        }
    }

}
