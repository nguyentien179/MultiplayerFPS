using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectToCam : MonoBehaviour
{

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
