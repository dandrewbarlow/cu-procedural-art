using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hwSixManager : MonoBehaviour
{
    public Texture2D image;
    public int particleCount = 100;
    public float randomRange = 10;
    public GameObject particleObject;
    public bool rebuild;
    public Material material;
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
        if (rebuild)
        {
            rebuildParticles();
            rebuild = false;
        }
    }

    void rebuildParticles()
    {
        particles.Clear();
        foreach(Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        createParticles();
    }

    hwSixParticle createParticle()
    {
        hwSixParticle p = new hwSixParticle();
        p.setObject(particleObject, this.transform);
        p.createMaterial(material);
        p.setColor(
            Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)
        );
        p.setPosition(
            new Vector3(
                Random.Range(-randomRange/2, randomRange/2),
                Random.Range(-randomRange/2, randomRange/2),
                Random.Range(-randomRange/2, randomRange/2)
            )
        );
        
        return p;
    }

    void createParticles()
    {
        if (particles == null) {return;}
        for (int i = 0; i < particleCount; i++)
        {
            particles.Add( createParticle() );
        }
    }

    void loopThroughImage()
    {
        for (int x = 0; x < image.width; x++)
        {
            for (int y =0; y < image.height; y++)
            {
                Debug.Log(image.GetPixel(x, y));
            }
        }
    }
}