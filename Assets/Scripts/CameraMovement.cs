using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float d;
    public Vector3 delta;

    // Start is called before the first frame update
    void Start()
    {
        d = 3.14159f / 12f;
        delta = new Vector3(0, d, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localEulerAngles += delta;
    }
}

