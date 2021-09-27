using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeFace_andrew : MonoBehaviour
{
    public Transform mouth;
    public Transform eye;
    public Transform eyebrow;
    public Transform nose;

    // store all parts in array
    private Transform[] bodyParts = new Transform[4];

    public bool randomize;

    public float minWidth = 0.2f;
    public float maxWidth = 0.3f;

    public float minHeight = 0.2f;
    public float maxHeight = 0.3f;

    [Tooltip("X:min, Y:max")]
    public Vector2 rotation;

    // Start is called before the first frame update
    void Start()
    {
        // randomize = false;
        bodyParts = new Transform[4] {mouth, eye, eyebrow, nose};
    }

    void Randomize(Transform t) 
    {

        // only randomize elements w/ no children, else recursively randomize all children
        if (t.childCount > 0) 
        {
            foreach (Transform child in t)
            {
                // 👻 recursion
                Randomize(child);
            }
        }
        else 
        {
            MeshRenderer meshRenderer = t.GetComponent<MeshRenderer>();

            // mouth rotation
            t.localEulerAngles = MakeRandomVector(
                new Vector3(rotation.x, 0, 0),
                new Vector3(rotation.y, 0, 0)
            );

            // mouth scale
            t.localScale = MakeRandomVector(
                new Vector3(minWidth, minHeight, .2f),
                new Vector3(maxWidth, maxHeight, .2f)
            );
        }
    }

    Vector3 MakeRandomVector(Vector3 min, Vector3 max) 
    {
        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            randomize = true;
        }

        if (randomize)
        {
            foreach (Transform t in bodyParts)
            {
                Randomize(t);
            }

            randomize = false;
        }
    }
}
