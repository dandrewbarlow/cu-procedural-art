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
    public void setScale(Vector3 scale)
    {
        particleObject.transform.localScale = scale;
    }

    public void setRotation(float angle)
    {
        particleObject.transform.localEulerAngles = new Vector3(0, 0, angle);
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
