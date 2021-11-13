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
        private Vector2 bounds;

        public Agent(float ang, Vector2 pos, Vector2 boundaries)
        {
            angle = ang;
            position = pos;
            bounds = boundaries;
        }

        public void rotate(float theta)
        {
            angle += theta;
        }

        public void move(Vector2 delta)
        {
            position += delta;
            boundsCheck();
        }

        public void boundsCheck()
        {
            if (position.x < 0)
            {
                position.x = bounds.x - position.x;
            }
            else if (position.x > bounds.x)
            {
                position.x = position.x - bounds.x;
            }

            if (position.y < 0)
            {
                position.y = bounds.y - position.y;
            }
            else if (position.y > bounds.y)
            {
                position.y = position.y - bounds.y;
            }
        }
    }

    // PUBLIC //////////////////////////////////////////////////

    // Parameters (for more info, see Jones, 134)

    // particle object settings
    public Mesh mesh;
    public Material material;

    // Agent Variables
    public int agentAmount = 100;
    [Range(0f,90f)]
    public int agentRotationAngle = 45;
    public int stepSize = 1;

    // Sensor Variables
    public float sensorAngle = 45;
    public int sensorWidth = 1;
    public int sensorOffset = 9;

    // Misc Settings
    public int resolution = 200;

    [Range(0f,1f)]
    public float probabilityOfRandomChange;

    [Range(0f,1f)]
    public float sensitivityThreshold;

    [Range(0f,1f)]
    public float depositPerStep;

    [Range(0f,5f)]
    public float diffusionKernelSize;

    [Range(0f, 1f)]
    public float decayFactor;


    // PRIVATE //////////////////////////////////////////////////

    // Data Structures
    private Agent[,] agentMap;
    private List<Agent> agentList;
    private List<GameObject> agentObjects;
    private float[,] trailMap;

    // Utility
    private static System.Random rng = new System.Random();  

    // METHODS //////////////////////////////////////////////////

    void Start(){

        // INITIALIZATION //////////////////////////////

        agentMap = new Agent[resolution,resolution];
        agentList = new List<Agent>();
        agentObjects = new List<GameObject>();

        int spawnedAgents = 0;

        for(int i = 0; i < agentAmount; i++)
        {

            int x = Random.Range(0, resolution);
            int y = Random.Range(0, resolution);

            // find valid random placement
            while(agentMap[x,y] != null)
            {
                x = Random.Range(0, resolution);
                y = Random.Range(0, resolution);
            }

            // create a new agent
            Agent agent = new Agent(
                Random.Range(0f, 360f),             // random heading
                new Vector2(x, y),                  // map pos
                new Vector2(resolution, resolution) // boundaries
            );

            // Add it to data structs
            agentMap[x,y] = agent;
            agentList.Add(agent);
        }
             
        trailMap = new float[resolution, resolution];
    }

    void Update(){
        MotorStage();
        RenderAgents();
    }

    // (Jones, 133)
    private void MotorStage()
    {
        /* PSEUDOCODE
        foreach(agent in agents)
        {
            attempt to move forward in current direction

            if (moved forward successfully)
            {
                depositTrailInNewLocation()
            }
            else
            {
                chooseRandomNewOrientation()
            }
        }
        */

        // TODO: random selection process to avoid sequential bias
        // TODO: shuffle agentList
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
            return new Agent(agent.angle, newPos, new Vector2(resolution,resolution));
        }
        else
        {
            agent.angle += Mathf.Pow(-1, Random.Range(0, 1)) * agentRotationAngle;
            return agent;
        }
    }

    // TODO: DEPOSIT CHEMOATTRACTANT
    private void Deposit(int x, int y)
    {
        // diffuse Chemoattractant in an area (s=diffusionKernelSize) of a given position

        for (int i = x - (int)(diffusionKernelSize / 2); i < x + (int)(diffusionKernelSize / 2); i++)
        {
            for (int j = y - (int)(diffusionKernelSize / 2); j < y + (int)(diffusionKernelSize / 2); j++)
            {
                if (BoundsCheck(i) && BoundsCheck(j))
                {
                    trailMap[i,j] += depositPerStep;
                }
            }
        }

    }
    
    // returns true if within bounds, false if not
    private bool BoundsCheck(int i)
    {
        return (i >= 0 || i < resolution);
    }

    private void SensoryStage()
    {
        // TODO: Implement
        /* PSEUDOCODE

        foreach(agent in agents)
        {
            FL = leftSensor
            F = middleSensor
            FR = rightSensor

            if (F > FL) && (F > FR)
            {
                stay in same direction
            }
            else if (F < FL) && (F < FR)
            {
                rotate randomly left or right by RA
            }
            else if (FL < FR)
            {
                rotate right by RA
            }
            else if (FR < FL)
            {
                rotate left by RA
            }
            else
            {
                continue in same direction
            }
        }
        */
    }

    private void DestroyAgents()
    {
        if (agentObjects.Count > 0)
        {
            foreach (GameObject obj in agentObjects)
            {
                Destroy(obj);
            }

            agentObjects.Clear();
        }
    }

    private void RenderAgents()
    {
        DestroyAgents();
        foreach (Agent agent in agentList)
        {
            RenderAgent(agent);
        }
    }

    // create a gameobject for the agent
    private void RenderAgent(Agent agent)
    {
        GameObject a = new GameObject(
            "Agent " + agent.position.x * resolution + agent.position.y, 
            typeof(MeshFilter), 
            typeof(MeshRenderer)
        );
        a.GetComponent<MeshFilter>().mesh = mesh;
        a.GetComponent<MeshRenderer>().material = new Material(material);
        a.transform.position = new Vector3(agent.position.x, agent.position.y, 0);
        a.transform.parent = this.transform;

        agentObjects.Add(a);
    }
}