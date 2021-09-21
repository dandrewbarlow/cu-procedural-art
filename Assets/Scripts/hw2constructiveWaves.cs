using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hw2constructiveWaves : MonoBehaviour
{
    // abstraction of sin waves
    private class Wave {
        private float amp, freq, offset;

        public Wave() {
            amp = Random.Range(0f, 20f);
            freq = random(0f, 50f);
            offset = random(0f, 90f);
        }

        public int getSin(theta) {
            return Mathf.Sin( theta / frequency) + offset);
        }
    }

/*
    // adjust waveform
    public float amplitude, frequency, offset;

    // amount of waveforms to add to each other
    public int iterations;
    // allow resizing seeds on the fly by looking for changes in iterations
    private int prevIterations;
*/

    // multiplier of time delta for each frame
    public float speed;
    
    // when true, counter += speed * time.delta, else nothing
    public bool oscillate;

    // keep track of time in scene
    private float counter;

    // an array of seeds with which to construct waveforms
    private Wave[] waves;

    // Start is called before the first frame update
    void Start()
    {
        amplitude = 60f;
        frequency = 31.9f;
        counter = 0f;
        offset = 0f;
        speed = 1f;
        iterations = 2;
        prevIterations = iterations;
        oscillate = true;
        seeds = new Wave[iterations];
    }

    Vector3 calculatePosition(Vector3 position) {
        float y = calculateHeight(position).y;
        Vector3 newPos = new Vector3(position.x, y / 2, position.z);
        return newPos;
    }

    Vector3 calculateHeight(Vector3 position) {
        float y = 0;
        for (int i = 0; i < iterations; i++) {
            y += Mathf.Sin( (counter+ seeds[i] + position.x  / frequency) + offset);
        }
        y = (y / iterations) * amplitude;
        return new Vector3(1, y, 1);
    }

    // so I don't have to select all the objects
    void updateChildren() {
        foreach(Transform child in this.transform)
        {
            child.transform.localScale = calculateHeight(child.transform.position);
            child.transform.position = calculatePosition(child.transform.position);
        }
    }

/*
    void generateSeeds() {
        for (int i = 0; i < iterations; i++) {
            seeds[i] = Random.Range(0f, 1000f);
        }
    }
*/

    void iterationValidation() {
        if (iterations != prevIterations) {
            System.Array.Resize(ref seeds, iterations);
            generateSeeds();
            prevIterations = iterations;
        }

    }

    // Update is called once per frame
    void Update()
    {
        iterationValidation();
        updateChildren();
        if (oscillate) {
            counter += Time.deltaTime * speed;
        }
    }}
