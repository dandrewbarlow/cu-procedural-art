using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hw71 : MonoBehaviour
{
    public Camera camera;
    public Material material;
    public Color meshColor;
    public Color backgroundColor;

    private List<GameObject> objects;
    private float time;
    private float PI = Mathf.PI;

    void Start()
    {
        objects = new List<GameObject>();

        camera.backgroundColor = backgroundColor;

        for (int i = 0; i < 4; i++)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.parent = this.transform;

            g.transform.localPosition = new Vector3(
                Mathf.Cos(PI * ((float)i) / 2),
                Mathf.Sin(PI * ((float)i) / 2),
                0
            );

            g.transform.localScale = new Vector3(
                0.5f + Mathf.Cos(PI * i / 2),
                0.5f + Mathf.Sin(PI * i / 2),
                1
            );

            Renderer renderer = g.GetComponent<Renderer>();
            renderer.material = material;
            renderer.material.SetColor("_Color", meshColor);

            objects.Add(g);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        foreach (GameObject g in objects)
        {
            g.transform.position = new Vector3(
                g.transform.position.x + (Mathf.Cos(time) / 500),
                g.transform.position.y,
                0
            );
        }
    }
}
