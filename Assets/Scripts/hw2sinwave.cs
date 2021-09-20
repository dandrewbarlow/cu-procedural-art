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

        foreach(Transform child in this.transform)
        {
            foreach (Transform grandchild in child.transform)
            {
                float y = Mathf.Sin(counter + grandchild.transform.position.x / 5) * amplitude;
                Vector3 newPos = new Vector3(grandchild.transform.position.x, y, grandchild.transform.position.z);
                grandchild.transform.position = newPos;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        updateChildren();
        counter += 0.01f;
    }
}
