using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// creating a sin wave with a bunch of spheres
public class hw2sinwave : MonoBehaviour
{
    public float amplitude, frequency, offset;
    
    public bool oscillate;

    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        amplitude = 60f;
        frequency = 31.9f;
        counter = 0f;
        offset = 0f;
        oscillate = true;
    }

    Vector3 calculatePosition(Vector3 position) {
        float y = calculateHeight(position).y;
        Vector3 newPos = new Vector3(position.x, y / 2, position.z);
        return newPos;
    }

    Vector3 calculateHeight(Vector3 position) {
        float y = Mathf.Sin((counter + position.x / frequency) + offset) * amplitude;
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

    // Update is called once per frame
    void Update()
    {
        updateChildren();
        if (oscillate) {
            counter += Time.deltaTime;
        }
    }
}
