using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成　針ヶ谷天紀
public class PlayerSensor : MonoBehaviour
{
    float ColliderSize;
    bool PL_Search = false;
    void SetPL_Search(bool set) { PL_Search = set; }
    public bool GetPL_Search() { return PL_Search; }
    RaycastHit hit;
    private void Start()
    {
        ColliderSize = GetComponent<SphereCollider>().radius;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.Player)
        {
            transform.LookAt(other.transform.position);
            if (Physics.Raycast(transform.position, transform.forward, out hit, ColliderSize * 2))
            {
                if (hit.collider.gameObject.tag == Tags.Player)
                {
                    SetPL_Search(true);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.Player)
        {
            SetPL_Search(false);
        }
    }
}
