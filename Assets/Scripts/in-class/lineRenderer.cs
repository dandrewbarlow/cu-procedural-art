using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRenderer : MonoBehaviour
{
    public float counter, counterOffset;
    public int amountOfPoints;
    public Vector3 frequency, amplitude, offset;

    public Vector3[] points;

    public bool rebuild;
    public bool writeGCode = false;

    // Start is called before the first frame update
    void Start()
    {
        // points = new Vector3(0f,0f,0f)[amountOfPoints];
    }

    // Update is called once per frame
    void Update()
    {
        if (rebuild)
        {
            DrawLine();
        }
    }

    void DrawLine()
    {
        counter = 0;
        points = new Vector3[amountOfPoints];

        for (int i = 0; i < amountOfPoints; i++) {
            counter += counterOffset;
            float x = Mathf.Sin(frequency.x * counter + offset.x) * amplitude.x;
            float y = Mathf.Sin(frequency.y * counter + offset.y) * amplitude.y;
            Vector3 point = new Vector3(x, y, 0);
            points[i] = point;
        }

        // lineRenderer.positionCount = points.Length;
        // lineRenderer.SetPositions(points);
    }

    void WriteGCode() {
        string header = "";
        string positions = "";
        for (int i = 0; i < amountOfPoints; i++) {
            positions += 
            "G0 X" + 
            (((points[i].x + 1) * .5f) * 220)
            + 
            " Y" + 
            (((points[i].y + 1) * .5f) * 220)
            + "\n";
        }

        header += positions;
        print(header);
    }
}
