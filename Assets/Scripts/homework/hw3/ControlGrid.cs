using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art;

public class ControlGrid : ArtMakerTemplate
{
    public Transform parent;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        // MakeArt();
    }

    // Update is called once per frame
    public override void MakeArt()
    {
        parent.GetComponent<Renderer>().enabled = true;
        int m = 5;
        for (float i = 0; i < m; i++) 
        {
            for (float j = 0; j < m; j++) 
            {
                Transform cube = Instantiate(parent);
                // cube.GetComponent<Renderer>().material.color = color;
                cube.transform.position = new Vector3(
                    Random.Range(-i, i),
                    Random.Range(-i, i),
                    0
                );
                cube.transform.localScale = new Vector3(
                    Random.Range(-i, i),
                    Random.Range(-j, j),
                    Random.Range(-j, j)
                );
                AddToRoot(cube.transform);
            }
        }
        parent.GetComponent<Renderer>().enabled = false;
    }
}
