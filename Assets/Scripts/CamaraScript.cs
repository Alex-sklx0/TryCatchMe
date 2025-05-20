using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform cristian;

    void Update()
    {
        if (cristian != null)
        {
            Vector3 posicion = transform.position;
            posicion.x = cristian.position.x;
            transform.position = posicion;
        }
    }
}
