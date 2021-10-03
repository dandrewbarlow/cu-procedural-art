using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mondrian : MonoBehaviour
{

    public bool randomize;
    public Transform parent;

    private Box[] boxes;

    // modularize box logic
    public class Box 
    {

        public Color[] colors = {
            new Color(0, 0, 0, 0), // black
            new Color(1f, 0, 0, 0), // red
            new Color(0, 0, 1f, 0), // blue
            new Color(1f, 1f, 0, 0), // yellow
            new Color(1f, 1f, 1f, 0) // white
        };

        public Transform root;
        private Transform colorClone;

        // box constructor
        public Box(Vector3 position, Vector3 size, Transform parent) 
        {
            colorClone = Instantiate(parent, root);

            colorClone.localPosition = position;
            colorClone.localScale = size;
            setRandomColor();

            // 👨‍🌾 second generation clones, to create borders on boxes
            Transform[] inbred = new Transform[4];

            for (int i = 0; i < 4; i++ )
            {
                inbred[i] = Instantiate(parent, colorClone);
                            
                // black borders
                inbred[i].GetComponent<Renderer>().material.color = colors[0];

                // ok gotta create some borders w/ math. Probably a better way to do this, but just using index & 
                    // properties of raising (-1)^index to offset in positive & negative x & y directions
                // borders on x axis
                if (i < 2) {
                    inbred[i].localPosition = new Vector3(
                        0.45f * Mathf.Pow(-1, i) ,
                        0,
                        // we want the borders in front of the boxes
                        -1
                    );
                    inbred[i].localScale = new Vector3(
                        0.1f,
                        1,
                        0
                    );
                }

                // borders on y axis
                if (i >= 2) {
                    inbred[i].localPosition = new Vector3(
                        0,
                        0.45f* Mathf.Pow(-1, i),
                        // we want the borders in front of the boxes
                        -1
                    );
                    inbred[i].localScale = new Vector3(
                        1,
                        0.1f,
                        0
                    );
                }
            }

        } // END: Box()

        public void setRandomColor() 
        {
            Color c = colors[Random.Range(1, 5)];
            colorClone.GetComponent<Renderer>().material.color = c;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // amount of boxes
        int n = 5;
        boxes = new Box[n];

        for (int i = 0; i < n; i++) 
        {
            boxes[i] = new Box(
                new Vector3(1.5f*i, 1.5f*i, 0),
                new Vector3(1.5f*i, 1.5f*i, 0.1f),
                parent
            );
        }

        // banish the unit cube to the shadow realm
            // shadow realm? I'm sending you to tampa, florida 🐊
            // https://vm.tiktok.com/ZMRwJtfVh/
        parent.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("p")) {randomize = true;}

        if (randomize)
        {
            randomizeAllColors();
            randomize = false;
        }
    }

   
    void randomizeAllColors() 
        {
            foreach (Box b in boxes)
            {
                b.setRandomColor();
            }
        }
    
}