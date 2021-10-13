using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art;

public class hw4ArtMaker2 : ArtMakerTemplate
{
    public GameObject parent;

    public void lineRow(Vector3 pos) {

        bool vertical = Random.Range(0f, 1f) > 0.5f;

        List<GameObject> row = new List<GameObject>();

        float size = Random.Range(1f, 3f);

        // red || black
        bool col = Random.Range(0f,1f) > 0.5;
        
        for (float i = 0; i < Random.Range(10,20); i+= Random.Range(0.1f, 1f)) {
            GameObject g;

            if (vertical) 
            {
                g = Instantiate(parent);
                g.transform.localPosition = new Vector3(
                    pos.x + i,
                    pos.y,
                    pos.z
                );
                g.transform.localScale = new Vector3(
                    1f/Random.Range(5f, 10f),
                    size,
                    1
                );
            }
            else 
            {
                g = Instantiate(parent);
                g.transform.localPosition = new Vector3(
                    pos.x,
                    pos.y + i,
                    pos.z
                );
                g.transform.localScale = new Vector3(
                    size,
                    1f/Random.Range(5f, 10f),
                    1
                );                
            }

            // set color to black
            if (col)
            {
                g.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1);
            }

            row.Add(g);
        }

        foreach (GameObject g in row)
        {
            AddToRoot(g.transform);
        }
    }

    public override void MakeArt()
    {
        for (float x = -5; x < 5; x+= 0.1f)
        {
            for (float y = -5; y < 5; y+= 0.1f)
            {
                // probability of drawing a line
                float p = 0.0005f;
                if (Random.Range(0f, 1f) < p)
                {
                    lineRow(new Vector3(x, y, 0));
                }
            }
        }
    }
}
