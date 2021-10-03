using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mondrian : MonoBehaviour
{
    public Color[] colors = {
        new Color(0, 0, 0, 0), // black
        new Color(1f, 0, 0, 0), // red
        new Color(0, 0, 1f, 0), // blue
        new Color(1f, 1f, 0, 0), // yellow
        new Color(1f, 1f, 1f, 0) // white
    };

    public Transform parent;
    public Transform root;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++) {
            createBox(
                new Vector3(1.5f*i, 1.5f*i, 0),
                new Vector3(1.5f*i, 1.5f*i, 0.1f)
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createBox(Vector3 position, Vector3 size) {
        Transform clone = Instantiate(parent, root);

        clone.localPosition = position;
        clone.localScale = size;
        clone.GetComponent<Renderer>().material.color = colors[Random.Range(1, 5)];

        // 👨‍🌾 second generation clones, to create borders on boxes
        Transform[] inbred = new Transform[4];
        for (int i = 0; i < 4; i++ )
        {
            inbred[i] = Instantiate(parent, clone);
                        
            // black
            inbred[i].GetComponent<Renderer>().material.color = colors[0];

            // ok gotta create some borders w/ math

            if (i < 2) {
                inbred[i].localPosition = new Vector3(
                    clone.localPosition.x + (clone.localScale.x / 2) * Mathf.Pow(-1, i) ,
                    0,
                    0
                );
                inbred[i].localScale = new Vector3(
                    parent.localScale.y/10,
                    1,
                    0
                );
            }

            if (i >= 2) {
                inbred[i].localPosition = new Vector3(
                    0,
                    parent.localPosition.y * Mathf.Pow(-1, i) / i+2,
                    0
                );
                inbred[i].localScale = new Vector3(
                    1,
                    parent.localScale.x/10,
                    0
                );
            }
        }
    }
}