using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hw2sinwave : MonoBehaviour
{
    public float amplitude;
    
    private float counter;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        amplitude = 20f;
        counter = 0f;
        pos = this.transform.position;
    }

    void updateChildren() {

    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(counter + pos.x / 5) * amplitude;
        
        Vector3 newPos = new Vector3(pos.x, y, pos.z);
        this.transform.position = newPos;

        counter += 0.01f;
    }
}
