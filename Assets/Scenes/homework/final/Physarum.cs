/*
 * Andrew Barlow
 * Procedural Art Final: Physarum Simulation
 * 🍄 fuck yeah, fungi
 *
 * Resources:
 * 
 * Sage Jenson 
 * https://sagejenson.com/physarum
 * 
 * Jeff Jones- Characteristics of Pattern Formation and Evolution in Approximations of Physarum Transport Networks
 * http://eprints.uwe.ac.uk/15260/1/artl.2010.16.2.pdf
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ListExtensions;

/*
 * https://payload.cargocollective.com/1/18/598881/13800048/diagram_670.jpg
 * Agent Algorithm:
 *  1. Sense
 *  2. Rotate
 *  3. Move
 *  4. Deposit
 *  5. Diffuse
 *  6. Decay
 */

public class Physarum : MonoBehaviour
{

    // AGENT CLASS //////////////////////////////////////////////////

    private class Agent
    {
        public float angle;
        public Vector2 position;

        public Agent(float ang, Vector2 pos)
        {
            angle = ang;
            position = pos;
        }
    }

    // PUBLIC //////////////////////////////////////////////////

    // Parameters (for more info, see Jones, 134)

    // Misc Settings
    public int resolution = 200;

    // particle object settings
    public Mesh mesh;
    public Material material;

    // Agent Variables
    public int agentAmount = 100;

    [Range(0f,90f)]
    public int agentRotationAngle = 45;

    [Range(1, 500)]
    public int spawnRange = 50;

    public int stepSize = 1;

    // Sensor Variables
    public float sensorAngle = 45;

    // public int sensorWidth = 1;

    public int sensorOffset = 9;


    [Range(0f,1f)]
    public float probabilityOfRandomChange;

    [Range(0f,1f)]
    public float sensitivityThreshold;

    [Range(0f,5f)]
    public float depositPerStep;

    [Range(2,6)]
    public int diffusionKernelSize = 3;

    [Range(0f, 1f)]
    public float decayFactor;

    // Display
    private Texture2D trailTexture;
    // private Sprite trailSprite;
    // private SpriteRenderer spriteRenderer;

    // PRIVATE //////////////////////////////////////////////////

    // Data Structures
    private Agent[,] agentMap;
    private List<Agent> agentList;
    private List<GameObject> agentObjects;
    private float[,] trailMap;


    // METHODS //////////////////////////////////////////////////

    void Start(){

        // INITIALIZATION //////////////////////////////

        // create data structures
        agentMap = new Agent[resolution,resolution];
        agentList = new List<Agent>();
        // agentObjects = new List<GameObject>();
        trailMap = new float[resolution, resolution];
        trailTexture = new Texture2D(resolution, resolution);

        // create agents
        for(int i = 0; i < agentAmount; i++)
        {

            int min = (resolution / 2) - spawnRange;
            int max = (resolution / 2) + spawnRange;
            int x = Random.Range(min, max);
            int y = Random.Range(min, max);

            // find valid random placement
            while(agentMap[x,y] != null)
            {
                x = Random.Range(0, resolution);
                y = Random.Range(0, resolution);
            }

            // create a new agent
            Agent agent = new Agent(
                Random.Range(0f, 360f),             // random heading
                new Vector2(x, y)                  // map pos
            );

            // Add it to data structs
            agentMap[x,y] = agent;
            agentList.Add(agent);
        }

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                trailMap[x,y] = 0.0f;
            }
        }
    }

    void Update(){
        MotorStage();
        SensoryStage();
        // RenderAgents();
        RenderTrail();
        Decay();
    }

    // (Jones, 133)
    private void MotorStage()
    {

        agentList.Shuffle();

        // transition
        List<Agent> newAgentList = new List<Agent>();

        foreach(Agent agent in agentList)
        {
            // try to move agent 
                // if failed, the heading is rotated so newAgent always replaces agent
            Agent newAgent = attemptMove(agent);

            newAgentList.Add(newAgent);

            // apply agent move to agentMap
            if (newAgent.position != agent.position)
            {
                agentMap[(int)agent.position.x, (int)agent.position.y] = null;

                int x = (int)Mathf.Floor(newAgent.position.x);
                int y = (int)Mathf.Floor(newAgent.position.y);
                agentMap[x, y] = newAgent;

                Deposit(x, y);
            }
        }

        // shallow list copy
        // https://stackoverflow.com/questions/10627141/how-can-i-replace-the-contents-of-one-listint-with-those-from-another
        agentList.Clear(); //= new List<Agent>(newAgentList);
        agentList.AddRange(newAgentList);
    }

    private Agent attemptMove(Agent agent)
    {
        // calculate next step in current direction
        Vector2 newPos = agent.position + new Vector2(
            stepSize * Mathf.Cos(agent.angle),
            stepSize * Mathf.Sin(agent.angle)
        );

        // integer indices for agentMap
        int x = (int)Mathf.Floor(newPos.x);
        int y = (int)Mathf.Floor(newPos.y);

        // bounds check
        if (!BoundsCheck(x) || !BoundsCheck(y))
        {
            agent.angle += 180;
            return agent;
        }

        // if new position is not occupied
        if ( agentMap[x, y] == null)
        {
            return new Agent(agent.angle, newPos);
        }
        else
        {
            if (Random.Range(0f, 1f) < probabilityOfRandomChange)
            {
                agent.angle += Mathf.Pow(-1, Random.Range(0, 1)) * agentRotationAngle;
            }
            return agent;
        }
    }

    private void Deposit(int x, int y)
    {
        trailMap[x,y] += depositPerStep;

        // diffuse Chemoattractant in an area (s=diffusionKernelSize) of a given position
        // uses mean filter https://homepages.inf.ed.ac.uk/rbf/HIPR2/mean.htm

        int n = 0;
        float sum = 0;

        for (int i = x - (diffusionKernelSize / 2); i < x + (diffusionKernelSize / 2); i++)
        {
            for (int j = y - (diffusionKernelSize / 2); j < y + (diffusionKernelSize / 2); j++)
            {
                if (BoundsCheck(i) && BoundsCheck(j))
                {
                    sum += trailMap[i,j];
                    n++;
                }
            }
        }

        float average = sum / (float)n;
        for (int i = x - (int)(diffusionKernelSize / 2); i < x + (int)(diffusionKernelSize / 2); i++)
        {
            for (int j = y - (int)(diffusionKernelSize / 2); j < y + (int)(diffusionKernelSize / 2); j++)
            {
                if (BoundsCheck(i) && BoundsCheck(j))
                {
                    trailMap[i,j] = average;
                }
            }
        }
    }
    
    // returns true if within bounds, false if not
    private bool BoundsCheck(int i)
    {
        return (i >= 0 && i < resolution);
    }

    private void SensoryStage()
    {

        foreach (Agent agent in agentList)
        {
            // sample chemoattractants near agent
            float[] samples = SampleTrailMap(agent);

            // Boilerplate-y, but makes code marginally more readable
            float FL = samples[0];
            float F = samples[1];
            float FR = samples[2];


            if (F > FL && F > FR)
            {
                // stay in same direction
                return;
            }
            else if (F < FL && F < FR)
            {
                // rotate randomly left or right by RA
                agent.angle += Mathf.Pow(-1, Random.Range(0, 1)) * agentRotationAngle;
            }
            else if (FL < FR)
            {
                // rotate right by RA
                agent.angle += agentRotationAngle;
            }
            else if (FR < FL)
            {
                // rotate left by RA
                agent.angle -= agentRotationAngle;
            }
            else
            {
                // continue in same direction
                return;
            }
        }
    }

    // whew. this is the individual sampling logic
    private float[] SampleTrailMap(Agent agent)
    {

        float FL, F, FR;
        FL = F = FR = 0;

        // Left Sensor
        int FLx = (int) (sensorOffset * Mathf.Cos(agent.angle + sensorAngle) + agent.position.x);
        int FLy = (int) (sensorOffset * Mathf.Sin(agent.angle + sensorAngle) + agent.position.x);

        if (BoundsCheck(FLx) && BoundsCheck(FLy))
        {
            FL = trailMap[FLx, FLy];
        }

        // Middle Sensor
        int Fx = (int) (sensorOffset * Mathf.Cos(agent.angle) + agent.position.x);
        int Fy = (int) (sensorOffset * Mathf.Sin(agent.angle) + agent.position.x);

        if (BoundsCheck(Fx) && BoundsCheck(Fy))
        {
            F = trailMap[Fx, Fy];
        }
        
        // Right Sensor
        int FRx = (int) (sensorOffset * Mathf.Cos(agent.angle - sensorAngle) + agent.position.x);
        int FRy = (int) (sensorOffset * Mathf.Sin(agent.angle - sensorAngle) + agent.position.x);

        if (BoundsCheck(FRx) && BoundsCheck(FRy))
        {
            FR = trailMap[FRx, FRy];
        }

        return new float[] {FL, F, FR};
    }

    private void Decay()
    {
        for(int i = 0; i < resolution; i++)
        {
            for(int j = 0; j < resolution; j++)
            {
                trailMap[i,j] *= decayFactor;
            }
        }
    }

    private void RenderTrail()
    {
        GameObject background = this.gameObject.transform.Find("Background").gameObject;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                Color c = new Color(trailMap[x,y], trailMap[x,y], trailMap[x,y], 1);
                trailTexture.SetPixel(x, y, c);
            }
        }

        trailTexture.Apply();

        background.GetComponent<Renderer>().material.SetTexture("_MainTex", trailTexture);
    }


    // ? Used to test object behavior by instantiating objects; no longer
    // ? necessary, but leaving it in case I wanna do something interesting

    // private void DestroyAgents()
    // {
    //     if (agentObjects.Count > 0)
    //     {
    //         foreach (GameObject obj in agentObjects)
    //         {
    //             Destroy(obj);
    //         }

    //         agentObjects.Clear();
    //     }
    // }

    // private void RenderAgents()
    // {
    //     DestroyAgents();
    //     foreach (Agent agent in agentList)
    //     {
    //         RenderAgent(agent);
    //     }
    // }

    // // create a gameobject for the agent
    // private void RenderAgent(Agent agent)
    // {
    //     GameObject a = new GameObject(
    //         "Agent " + agent.position.x * resolution + agent.position.y, 
    //         typeof(MeshFilter), 
    //         typeof(MeshRenderer)
    //     );
    //     a.GetComponent<MeshFilter>().mesh = mesh;
    //     a.GetComponent<MeshRenderer>().material = new Material(material);
    //     a.AddComponent<TrailRenderer>();
    //     a.transform.position = new Vector3(agent.position.x, agent.position.y, 0);
    //     a.transform.parent = this.transform;

    //     agentObjects.Add(a);
    // }
}