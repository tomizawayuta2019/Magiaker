using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作成　佐藤 竜也
public class PlayerPositionSet : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag(Tags.Player);
        transform.position = Player.transform.position;
    }

    void Update()
    {
        transform.position = Player.transform.position;
    }
}
