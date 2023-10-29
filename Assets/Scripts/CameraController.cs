using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;

    public Vector3 offset = new Vector3(0, 20, 0);


    void Update()
    {
        transform.position = Player.transform.position + offset;
        
    }
}
