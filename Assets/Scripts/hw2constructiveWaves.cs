using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hw2constructiveWaves : MonoBehaviour
{
    // abstraction of sin waves
    public class Wave {
        private float amp, freq, offset;

        public Wave() 
        {
            amp = Random.Range(5f, 20f);
            freq = Random.Range(0f, 50f);
            offset = Random.Range(0f, 90f);
        }

        public float getSin(float theta) 
        {
            return Mathf.Sin( (theta / freq) + offset);
        }
    }

    // adjust waveform
    public float amplitude, frequency, offset;

    // amount of waveforms to add to each other
    public int iterations;
    // allow resizing seeds on the fly by looking for changes in iterations
    private int prevIterations;

    // multiplier of time delta for each frame
    public float speed;
    
    // when true, counter += speed * time.delta, else nothing
    public bool oscillate;

    // change material 4 all children
    public Material material;

    // keep track of time in scene
    private float counter;

    // an array of seeds with which to construct waveforms
    private Wave[] waves; 

    // Start is called before the first frame update
    void Start()
    {
        counter = 0f;
        amplitude = 1f;
        frequency = 1f;
        offset = 0f;
        speed = 1f;
        iterations = 2;
        prevIterations = iterations;
        oscillate = true;
        material = this.transform.GetChild(0).GetComponent<Renderer>().material;
        waves = new Wave[iterations];
        initializeWaves();
    }

    void initializeWaves() {
        for (int i = 0; i < iterations; i++) {
            waves[i] = new Wave();
        }
    }

    Vector3 calculatePosition(Vector3 position) {
        float y = calculateHeight(position).y;
        Vector3 newPos = new Vector3(position.x, y / 2, position.z);
        return newPos;
    }

    Vector3 calculateHeight(Vector3 position) {
        float y = 0;
        for (int i = 0; i < iterations; i++) {
            y += waves[i].getSin( ((counter + position.x)/frequency) + offset);
        }
        y = y * amplitude;
        return new Vector3(1, y, 1);
    }

    // so I don't have to select all the objects
    void updateChildren() {
        foreach(Transform child in this.transform)
        {
            child.transform.localScale = calculateHeight(child.transform.localPosition);
            child.transform.localPosition = calculatePosition(child.transform.localPosition);

            // https://answers.unity.com/questions/13356/how-can-i-assign-materials-using-c-code.html
            if (child.GetComponent<Renderer>().material != material)
            {
                child.GetComponent<Renderer>().material = material;
            }
        }
    }

    void iterationValidation() {
        if (iterations != prevIterations) {
            System.Array.Resize(ref waves, iterations);
            initializeWaves();
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
