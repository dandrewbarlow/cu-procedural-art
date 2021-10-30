using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Art;

public class hw5Lissajous : MonoBehaviour
{
    public bool rebuild;
    public bool randomize;
    public bool writeGCodeToConsole;

    public float randomRange = (.5f);

    private float counter;
    public float counterOffset = 0.01f;

    private Vector3[] points;
    private List<Vector3[]> lines;

    public int lineCount;
    public int amountOfPoints = 500;
    
    public Vector2 frequency;
    public Vector2 amplitude;
    public Vector2 offset;

    public Vector2 noiseFrequency;
    public Vector2 noiseOffset;
    public Vector2 noiseAmplitude;

    private Vector2[] parameters; 
    public LineRenderer lineRenderer;


    void Start()
    {
        frequency = randomParameters(10);
        amplitude = randomParameters(10);
        offset = randomParameters(10);
        
        noiseFrequency = randomParameters(10);
        noiseAmplitude = randomParameters(10);
        noiseOffset = randomParameters(10);

        lines = new List<Vector3[]>();

        DrawLine();
    }

    void Update()
    {

        if (randomize)
        {
            randomizeAllParameters();
            rebuild = true;
            randomize = false;
        }

        if (Input.GetKey("r")) {rebuild = true;}
        if (rebuild)
        {
            DrawLine();
            rebuild = false;
        }
        if (writeGCodeToConsole)
        {
            // 155, 130
            // 25, 230
            // 25, 210
            // WriteGCode();
            Vector3[] minMax = MinMaxPoint();

            WriteGCode.Write(
                points,
                new Vector3(-10, -10, 0),
                new Vector3(10, 10, 1),
                new Vector3(50, 50, 0),
                new Vector3(200, 200, 1)
            );
            writeGCodeToConsole = false;
        }

    }

    void DrawLine() 
    {
        counter = 0;
        points = new Vector3[amountOfPoints];

        // for (int j = 0; j < lineCount; j++)
        // {
            for (int i = 0; i < amountOfPoints; i++)
            {
                // Perlin Noise
                float noiseX = Mathf.PerlinNoise(noiseFrequency.x * counter + noiseOffset.x, 0) * noiseAmplitude.x;
                float noiseY = Mathf.PerlinNoise(noiseFrequency.y * counter + noiseOffset.y, 0) * noiseAmplitude.y;

                // Lissajous Equations
                float lisX = Mathf.Sin(frequency.x * counter + offset.x) * amplitude.x;
                float lisY = Mathf.Sin(frequency.y * counter + offset.y) * amplitude.y;
                
                Vector3 point = new Vector3(
                    noiseX + lisX,
                    noiseY + lisY,
                    0
                );

                points[i] = point;
                counter += counterOffset;
            }

            // points[points.Length - 2] = points[points.Length - 2] + new Vector3(0,0,1);
            // points[points.Length - 1] = new Vector3(0,0,1);

            // lines.Add(points);
        // }


        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    public void randomizeAllParameters() {
        frequency = randomParameters(randomRange);
        amplitude = randomParameters(randomRange);
        offset = randomParameters(randomRange);
        
        noiseFrequency = randomParameters(randomRange);
        noiseAmplitude = randomParameters(randomRange);
        noiseOffset = randomParameters(randomRange);
    }


    Vector3[] MinMaxPoint() {
        Vector3 max = points[0] + new Vector3();
        Vector3 min = points[0];

        foreach (Vector3 point in points)
        {
            if (point.x > max.x) {max.x = point.x;}
            if (point.y > max.x) {max.x = point.y;}
            if (point.x < min.x) {min.x = point.x;}
            if (point.y < min.x) {min.x = point.y;}
        }

        Vector3[] minMax = {min, max};

        return minMax;
    }

     float MaxValue (float[] floatArray)
     {
        float max = floatArray[0];
        for (int i = 1; i < floatArray.Length; i++) {
            if (floatArray[i] > max) {
                max = floatArray[i];
            }
        }
        return max;
    }

    public void SetRandomize(bool b) {randomize = b;}

// /*
    public Vector2 randomParameters(float range)
    {
        return new Vector2(
            Random.Range(0f, range),
            Random.Range(0f, range)
        );
    }
// */

}
