using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hwSixParticle : MonoBehaviour
{
    private GameObject particleObject;
    private Color particleColor;

    void Start() {}
    void Update(){}

    public void setObject(GameObject obj, Transform parent)
    {
        particleObject = obj;
        Instantiate(obj, parent);
    }

    public void createMaterial(Material source) 
    {
        Material newMaterial = new Material(source);
        Renderer renderer = particleObject.GetComponent<Renderer>();
        renderer.material = newMaterial;
    }

    public void setRotation(Vector3 angle)
    {
        // particleObject.local = angle;
    }

    public void setColor(Color c)
    {
        particleColor = c;
        particleObject.GetComponent<Renderer>().material.SetColor("_Color", c);
    }
    public void setPosition(Vector3 pos)
    {
        particleObject.transform.localPosition = pos;
    }

}
