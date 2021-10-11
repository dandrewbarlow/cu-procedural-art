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

    // for dev experience more than anything
    public enum BorderPosition
    {
        Top,
        Right,
        Bottom,
        Left
    }

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
        private List<Transform> borders;
        private Transform parent, root;
        private Color[] colors;
        public Vector3 position, size;
        public Color color;

        // box constructor
        public Box(Vector3 pos, Vector3 s, Transform p, Transform r, Color[] c) 
        {
            // super ugly, but I want these params viewable in unity, so they gotta be max scope
            parent = p;
            root = r;
            colors = new Color[c.Length];
            borders = new List<Transform>();

            position = pos;
            size = s;
            for (int i = 0; i < c.Length; i++) {colors[i] = c[i];}

            colorClone = Instantiate(parent, root);

            colorClone.localPosition = position;
            colorClone.localScale = size;
            color = setRandomColor();
        } // END: Box()

        public void createBorder(BorderPosition bp) {
            Transform border = Instantiate(parent, colorClone);
            switch (bp)
            {
                case BorderPosition.Top:
                    border.localPosition = new Vector3(0, 0.5f, -1);
                    border.localScale = new Vector3(1, 0.1f, 0);
                    break;
                case BorderPosition.Bottom:
                    border.localPosition = new Vector3(0, -0.5f, -1);
                    border.localScale = new Vector3(1, 0.1f, 0);
                    break;
                case BorderPosition.Left:
                    border.localPosition = new Vector3(-0.5f, 0, -1);
                    border.localScale = new Vector3(0.1f, 1, 0);
                    break;
                case BorderPosition.Right:
                    border.localPosition = new Vector3(0.5f, 0, -1);
                    border.localScale = new Vector3(0.1f, 1, 0);
                    break;
                default:
                    Debug.Log("Invalid Border Position");
                    break;
            }
            borders.Add(border);
        }

        public Color setRandomColor() 
        {
            Color c = colors[Random.Range(1, 5)];
            colorClone.GetComponent<Renderer>().material.color = c;
            return c;
        }

        public void setColor(Color c) {
            colorClone.GetComponent<Renderer>().material.color = c;
        }

        public Color getColor() {
            return color;
        }

        public void destroyBorders() {
            foreach (Transform border in borders)
            {
                Destroy(border.gameObject);
            }
            borders.Clear();
            

            // Destroy(colorClone.gameObject);
            return;
            // try
            // {
            //     foreach (Transform border in borders)
            //     {
            //         Destroy(border.gameObject);
            //     }
                
            // }
            // catch (System.Exception)
            // {
            //     return;
            // }
        }

        // public void destroyBorder(BorderPosition bp){
        //     Debug.Log(borders);
        //     Destroy(
        //         borders[(int)bp].gameObject
        //     );
        // }
    } // END Class Box


    // Start is called before the first frame update
    void Start()
    {
        parent.GetComponent<Renderer>().enabled = true;

        grid = new Box[gridSize, gridSize];
        createBoxes();

        // banish the unit cube to the shadow realm
            // shadow realm? I'm sending you to tampa, florida 🐊
            // https://vm.tiktok.com/ZMRwJtfVh/
        parent.GetComponent<Renderer>().enabled = false;
    }

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

        createBorders();
    }

    void createBorders() {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // probs can loopify this, but had logic trouble

                // bounds checking & color checking
                if (x-1 >= 0 && grid[x,y].color != grid[x-1, y].color)
                {
                    grid[x,y].createBorder(BorderPosition.Left);
                }
                if (y-1 >= 0 && grid[x,y].color != grid[x, y-1].color)
                {
                    grid[x,y].createBorder(BorderPosition.Bottom);
                }

                // bounds checking
                if (x+1 <= Mathf.Sqrt(gridSize) && grid[x,y].color != grid[x+1, y].color)
                {
                    grid[x,y].createBorder(BorderPosition.Right);
                }
                if (y+1 <= Mathf.Sqrt(gridSize) && grid[x,y].color != grid[x, y+1].color)
                {
                    grid[x,y].createBorder(BorderPosition.Top);
                }

            }
        }
    }

    // Box boxCombine(Box b1, Box b2) {
    //     Vector3 position = (b1.position + b2.position) / 2f;
    //     Vector3 size = (b1.position - b2.position) * 2f;
    //     size.z += 0.2f;

    //     Box newB = new Box(position, size, parent, root, colors);

    //     // newB.setColor(b1.getColor());

    //     return newB;
    // }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("r")) {randomize = true;}

        if (randomize)
        {
            // foreach (Box box in grid)
            // {
                // box.setRandomColor();
                // box.destroyBorders();
            // }
            // createBorders();

            createBoxes();
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