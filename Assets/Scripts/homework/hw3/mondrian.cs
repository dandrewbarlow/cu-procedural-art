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

    public bool randomize;

    public Transform parent;
    public Transform root;

    // private List<Box> boxes;

    // for rect packing, keep an array to track which areas are filled
    private Box[,] grid;
    // allow grid size to be changed from unity
        // NOTE: grid is assumed to be unit squares for simplicity, this size refers to the amount of squares in the grid, which itself is square
    public int gridSize = 10;

    // modularize box logic
    public class Box 
    {
        private Transform colorClone;
        private Transform parent, root;
        private Color[] colors;
        public Color color;

        // box constructor
        public Box(Vector3 position, Vector3 size, Transform p, Transform r, Color[] c) 
        {
            // super ugly, but I want these params viewable in unity, so they gotta be max scope
            parent = p;
            root = r;
            colors = new Color[c.Length];
            for (int i = 0; i < c.Length; i++) {colors[i] = c[i];}

            colorClone = Instantiate(parent, root);

            colorClone.localPosition = position;
            colorClone.localScale = size;
            color = setRandomColor();

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

        public Color setRandomColor() 
        {
            Color c = colors[Random.Range(1, 5)];
            colorClone.GetComponent<Renderer>().material.color = c;
            return c;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new Box[gridSize, gridSize];

        // initialize grid
        // for (int x = 0; x < gridSize; x++) 
        // {
        //     for (int y = 0; y < gridSize; y++) 
        //     {
        //         grid[x, y] = n;
        //     }
        // }

        // boxes = new List<Box>();

        createBoxes();
        /*
        for (int i = 0; i < 5; i++) 
        {
            boxes.Add(new Box(
                new Vector3(1.5f*i, 1.5f*i, 0),
                new Vector3(1.5f*i, 1.5f*i, 0.1f),
                parent, root, colors
            ));
        }
        */

        // banish the unit cube to the shadow realm
            // shadow realm? I'm sending you to tampa, florida 🐊
            // https://vm.tiktok.com/ZMRwJtfVh/
        parent.GetComponent<Renderer>().enabled = false;
    }

    /*
    ok this is where the artistry comes in.

    I want this to look like a Mondrian. I'm going to create a box, with semi-randomized proportions. 
    They need to be ints, but I want boxes that have 2:1, 3:1, or even some occasional large 5:1 types
    I also don't want them to be uniformly random. Having more on the somewhat small side is closer to the actual style

    After I create them, I want to increase their proportions until they hit either an edge or another box
    In order to not have them all be tiny, I need to have them placed randomly
    But in order to not have a really ineffecient algorithm, I also want to loop through the grid to find empty spots

    I'm pretty sure I've not found the best way to do this, but I'm gonna try anyway
    */

    void createBoxes() {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x,y] = new Box(
                    new Vector3(x, y, 0),
                    new Vector3(1, 1, 0.1f),
                    parent, root, colors
                );
            }
        }
    }

/*
    void createBox(int x, int y) {

        // if this works, I may come back later to try and find a gaussian alternative
        int largeAspect = Random.Range(1, 5);

        bool vertical = Random.value > 0.5f;

        // we're gonna have to use decimals 😞
        float xf = x;
        float yf = y;

        // center = average between position & postion + larger aspect ratio/2
        if (vertical) {
            yf = (2f*yf + largeAspect) / 4f;
        }
        else {
            xf = (2f*xf + largeAspect) / 4f;
        }

        // bounds check

        bool maxSize = false;

        Vector2 position = new Vector2(xf, yf);
        Vector2 size;

        // so verbose
        if (vertical) 
        {
            size = new Vector2(1, largeAspect);
        }
        else
        {
            size = new Vector2(largeAspect, 1);
        }

        // check if it can fit
        if (boundsCheck(position, size) == false) {
            return;
        }

        int scalar = 1;
        while (!maxSize) 
        {
            if (boundsCheck(position, (scalar+1) * size)) {
                scalar++;
            }
            else
            {
                maxSize = true;
            }
        }

        size = size * scalar;

        // its been a long day, and this file is past 300 lines. let me have this sloppy triple nested command
        boxes.Add(new Box(
            new Vector3(position.x, position.y, 0f),
            new Vector3(size.x, size.y, 0.1f),
            parent, root, colors
        ));

        // gotta keep the funckin grid updated
        for (int i = 0; i < size.x; i++)
        {
            // also realizing this woulda been the way to do the bounds checking 
            for (int j = 0; j < size.y; j++) 
            {
                float offset = 0f;

                if (position.x % 1 > 0) 
                {
                    offset = 0.5f;
                }

                grid[saneFloatCast(position.x+i+offset) ,saneFloatCast(position.y+j+offset)] = true;

            }
        }

    }

    // helper function for creating boxes; returns true if box can fit, false if it can't
    bool boundsCheck(Vector2 position, Vector2 size)
    {
        // for those pesky boxes w/ a center in between 2 grid points
        if (position.x % 1 > 0 )
        {
            for (int i = 0; i < size.x; i++)
            {
                // brains a little fried rn with all these boundary conditions tbh. I think this is right
                int posIndex = saneFloatCast(position.x + 0.5f + i);
                int negIndex = saneFloatCast(position.x - 0.5f - i);

                if (grid[posIndex, saneFloatCast(position.y)] || grid[negIndex, saneFloatCast(position.y)]) {return false;}
            }
        }
        if (position.y % 1 > 0)
        {
            for (int i = 0; i < size.y; i++)
            {
                // brains a little fried rn with all these boundary conditions tbh. I think this is right
                int posIndex = (int)Mathf.Round(position.y + 0.5f + i);
                int negIndex = (int)Mathf.Round(position.y - 0.5f - i);

                if (grid[(int)position.x, posIndex] || grid[(int)position.x, negIndex]) {return false;}
            }
        }

        // for those blessed normal boxes
        for (int i = 0; i < size.x; i++) 
        {
            int posIndex = saneFloatCast(position.x + i);
            int negIndex = saneFloatCast(position.x - i);

            // this is getting out of hand
            if (grid[saneFloatCast(position.x) + posIndex, saneFloatCast(position.y)] || grid[saneFloatCast(position.x) - posIndex, saneFloatCast(position.y)])
            {
                return false;
            }
        }

        for (int i = 0; i < size.y; i++) 
        {
            int posIndex = saneFloatCast(position.y + i);
            int negIndex = saneFloatCast(position.y - i);

            // this is getting out of hand
            if (grid[saneFloatCast(position.x), saneFloatCast(position.y) + posIndex] || grid[saneFloatCast(position.x), saneFloatCast(position.y) - posIndex])
            {
                return false;
            }
        }

        return true;
    }

    // bc you never know
    int saneFloatCast(float f) {
        return (int) Mathf.Round(f);
    }

*/
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
            foreach (Box b in grid)
            {
                b.setRandomColor();
            }
        }
    
}