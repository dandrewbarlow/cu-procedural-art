using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art
{
    public class hw4ArtMaker : ArtMakerTemplate
    {
        public GameObject parent;
        // private List<Transform> objects = new List<Transform>();

        public Vector2 hue = new Vector2(0f, 1f);
        public Vector2 saturation = new Vector2(0f, 1f);
        public Vector2 value = new Vector2(0f, 1f);

        private Transform spawnSphere()
        {
            GameObject g = Instantiate(parent);
            g.GetComponent<Renderer>().material.color = Random.ColorHSV(
                hue.x, hue.y, 
                saturation.x, saturation.y,
                value.x, value.y
            );

            AddToRoot(g.transform);
            return g.transform;
        }

        // /*
        public void LateUpdate() {
            if (Input.GetKey(KeyCode.R))
                rebuild = true;
            if (rebuild)
            {
                // objects.Clear();
                Rebuild();
                rebuild = false;
            }

            // if (objects.Count > 0)
            // {
                foreach (Transform sphere in root.transform)
                {
                    moveSphere(sphere);
                }
            // }
        }
        private void moveSphere(Transform sphere)
        {
            // just for ease of typing
            Vector3 spherePos = sphere.localPosition;
            sphere.localPosition = new Vector3(
                spherePos.x + Random.Range(-1f,1f),
                spherePos.y + Random.Range(-1f,1f),
                spherePos.z + Random.Range(-1f,1f));
        }
        // */
        public override void MakeArt()
        {
            // object
            for(float i = -5; i < 5; i+=.5f)
            {
                for(float j = -5; j < 5; j+=.5f)
                {
                    for(float k = -5; k < 5; k+=.5f)
                    {
                        Transform sphere = spawnSphere();
                        sphere.localScale = new Vector3(.5f, .5f, .5f);
                        sphere.localPosition = new Vector3(
                            i + Random.Range(-1f,1f),
                            j + Random.Range(-1f,1f),
                            k + Random.Range(-1f,1f)
                        );
                        sphere.eulerAngles = new Vector3(
                            Random.Range(-90f,90f),
                            Random.Range(-90f,90f),
                            Random.Range(-90f,90f)
                        );
                        // objects.Add(sphere);
                    }
                }
            }
        }
    }
}

