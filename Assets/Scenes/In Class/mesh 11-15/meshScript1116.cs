using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Grid;

public class meshScript1116 : MonoBehaviour
{
    void Start()
    {
        Mesh m = Grid.Generate(100, 100, MyFunc);
        GetComponent<MeshFilter>().mesh = m;
    }

    Vector3 MyFunc(float u, float v)
    {
        float x = Mathf.PerlinNoise(v, u);
        float y = Mathf.PerlinNoise(u, x);
        float z = Mathf.PerlinNoise(u, v);
        return new Vector3(x, v, z);
    }

    Vector3 Plane(float u, float v)
    {
        return new Vector3(u, v, 0);
    }
}
