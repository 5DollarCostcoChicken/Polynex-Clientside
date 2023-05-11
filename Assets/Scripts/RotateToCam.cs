using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{

    public GameObject Camera;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.transform);
    }
}
