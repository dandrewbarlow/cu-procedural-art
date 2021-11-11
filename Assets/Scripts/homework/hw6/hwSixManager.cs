using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hwSixManager : MonoBehaviour
{
    public Camera camera;
    public Texture2D image;
    public int pixelStep = 1;
    public float perlinStep = 1;

    public GameObject particleObject;
    public Material material;

    public bool rebuild;

    private float randomPerlinOffset = 0f;
    private List<hwSixParticle> particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = new List<hwSixParticle>();

        createParticles();
        loopThroughImage();
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (hwSixParticle particle in particles)
        // {
        //     Debug.Log("test");
        //     particle.Update();
        // }

        if (rebuild)
        {
            rebuildParticles();
            rebuild = false;
        }
    }

    void adjustCamera() 
    {
        camera.transform.position = new Vector3(
            image.width / 2,
            image.height / 2,
            -10
        );

        camera.orthographicSize = Mathf.Max(image.width, image.height) / 2;
    }

    void rebuildParticles()
    {
        randomPerlinOffset = Random.Range(0f, 1000f);
        particles.Clear();
        foreach(Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        createParticles();
    }

    hwSixParticle createParticle(Color color, Vector3 position, Vector3 scale, float rotation)
    {
        // hwSixParticle p = gameObject.AddComponent(typeof(hwSixParticle)) as hwSixParticle;
        hwSixParticle p = new hwSixParticle();
        p.setObject(particleObject, this.transform);
        p.createMaterial(material);
        p.setColor(color);
        p.setPosition(position);
        p.setScale(scale);
        p.setRotation(rotation);
        
        return p;
    }

    void createParticles()
    {
        if (particles == null) {return;}

        loopThroughImage();
        adjustCamera();
    }

    void loopThroughImage()
    {
        for (int x = 0; x < image.width; x+=pixelStep)
        {
            for (int y =0; y < image.height; y+=pixelStep)
            {
                Color c = image.GetPixel(x, y);
                Vector3 pos = new Vector3(
                    x,y,
                    0
                );
                float noiseVal = Mathf.PerlinNoise(
                    x * perlinStep + randomPerlinOffset,
                    y * perlinStep + randomPerlinOffset
                );

                float noiseScale = (noiseVal + 1) * pixelStep;

                float rotation = 0;//noiseVal * 90;

                Vector3 scale = new Vector3(pixelStep,pixelStep,1);//noiseScale, noiseScale, 1);

                createParticle(c, pos, scale, rotation);
            }
        }
    }
}