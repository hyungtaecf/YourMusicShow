using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class topCube : MonoBehaviour
{
    private Vector3 originPos;
    private Vector3 currentPos;

    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        currentPos = transform.position;
        currentPos.x = originPos.x;
        currentPos.z = originPos.z;
        transform.position = currentPos;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
