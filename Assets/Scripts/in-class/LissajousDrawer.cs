using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LissajousDrawer : MonoBehaviour
{
    public float CounterX;
    public float CounterY;
    public float CounterZ;
    public float SpeedX;
    public float SpeedY;
    public float SpeedZ;

    public float Multiplier;

    // Start is called before the first frame update
    void Start()
    {
        print("Hello World!");

        // initial randomization of values
        SpeedX = Random.Range(0.01f, 1f);
        SpeedY = Random.Range(0.01f, 1f);
        SpeedZ = Random.Range(0.01f, 1f);
        Multiplier = Random.Range(0.01f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        CounterX += SpeedX;
        CounterY += SpeedY;
        CounterZ += SpeedZ;

        Transform t = this.GetComponent<Transform>();

        // basic lissajous
        // Vector3 v = new Vector3(Mathf.Sin(CounterX) * Multiplier, Mathf.Sin(CounterY) * Multiplier, 0);

        Vector3 v = new Vector3(Mathf.Cos(CounterX) * Multiplier, Mathf.Sin(CounterY) * Multiplier, Mathf.Cos(CounterZ));


        t.position = v;

        // ways to set position
        // v.Set(x, y, z);
        // t.Translate(0, posY, 0);
    }
    
}
