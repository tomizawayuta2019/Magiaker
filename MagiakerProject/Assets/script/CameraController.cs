using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    float TurnSpeed = 200f;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    float StartCameraPosX = 0;
    [SerializeField]
    float StartCameraPosY = 10;
    [SerializeField]
    float StartCameraPosZ = -2;
    float mouseInputX;
    Vector3 offset;
    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(
            Player.transform.position.x + StartCameraPosX,
            Player.transform.position.y + StartCameraPosY,
            Player.transform.position.z + StartCameraPosZ);
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mouseInputX = Input.GetAxisRaw("Mouse X");
            transform.RotateAround(Player.transform.position, Vector3.up, mouseInputX * Time.deltaTime * TurnSpeed);
        }
    }
}
