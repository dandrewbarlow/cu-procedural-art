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

    [Range(0f,5f)]
    public float diffusionKernelSize;

    [Range(0f, 1f)]
    public float decayFactor;


    // PRIVATE //////////////////////////////////////////////////

    // Data Structures
    private Agent[,] agentMap;
    private List<Agent> agentList;
    private float[,] trailMap;

    // Utility
    private static System.Random rng = new System.Random();  

    // METHODS //////////////////////////////////////////////////

    void Start(){

        // INITIALIZATION //////////////////////////////

        agentMap = new Agent[resolution,resolution];
        agentList = new List<Agent>();

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

    void Update(){}

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
        // loop through agentMap 
            // (agentMap.Length == resoution, but this is more descriptive)
        for (int i=0; i < agentMap.Length; i++)
        {
            for (int j=0; j < agentMap.Length; j++)
            {
                // check if an agent exists in current pos
                if (agentMap[i,j] != null)
                {
                    Agent agent = agentMap[i,j];

                    // try to move agent
                    Agent newAgent = attemptMove(agent);

                    // apply agent move if it happened
                    if (newAgent.position != agent.position)
                    {
                        agentMap[i,j] = null;

                        // over complicated, but just putting agent in new spot in the array
                        agentMap[(int)Mathf.Floor(newAgent.position.x), (int)Mathf.Floor(newAgent.position.y)] = newAgent;

                        // TODO: DEPOSIT CHEMOATTRACTANT

                    }
                    // if it didn't move, randomly change orientation
                    else
                    {
                        
                    }
                }
            }
        }

    }

    private Agent attemptMove(Agent agent)
    {
        // calculate next step in current direction
        Vector2 newPos = agent.position + new Vector2(
            stepSize * Mathf.Cos(agent.angle),
            stepSize * Mathf.Sin(agent.angle)
        );

        // if new position is already occupied
        if ( agentMap[(int)Mathf.Floor(newPos.x), (int)Mathf.Floor(newPos.y)] == null)
        {
            return new Agent(agent.angle, newPos, new Vector2(resolution,resolution));
        }
        else
        {
            return agent;
        }
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

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}