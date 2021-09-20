using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    public float frequency, amplitude, offset;
    // public bool move, rotate, size;
    public bool x_on, y_on, z_on;
    


    // Start is called before the first frame update
    void Start()
    {
        frequency = 5f;
        amplitude = 20f;
        offset = Random.Range(0f, 100f);

        x_on = true;
        y_on = true;
        z_on = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x, y, z;

        if (x_on) {
            x = Mathf.Cos(( Time.time * frequency) + offset) * amplitude;
        }
        else {
            x = this.transform.localPosition[0];
        }

        if (y_on) {
            y = Mathf.Sin(( Time.time * frequency) + offset) * amplitude;
        }
        else {
            y = this.transform.localPosition.y;
        }

        if (z_on) {
            z = Mathf.Sin(( Time.time * frequency) + offset) * amplitude;
        }
        else {
            z = this.transform.localPosition.z;
        }

        this.transform.localPosition = new Vector3(x, y, z);
    }
}
