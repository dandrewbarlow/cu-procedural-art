using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineParent : MonoBehaviour
{
    public Transform parent;
    public Transform root;
    public bool rebuild;
    public int lines = 1;
    public List<Transform> children;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < lines; i++)
        {
            children.Add( Instantiate(parent, root) );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rebuild)
        {
            foreach(Transform t in root)
            {
                // t.gameObject.SetRandomize(true);
            }
        }
    }
}
