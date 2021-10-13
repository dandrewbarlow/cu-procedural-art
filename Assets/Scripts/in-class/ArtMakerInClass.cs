using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art;

public class ArtMakerInClass : ArtMakerTemplate
{
    // public Color[] colors;
    public GameObject objectToInstantiate;

    private Color getRandomColor() {
        Color c = Random.ColorHSV(0, 1, 0.3f, 0.5f, 0.5f, 1f);
        return c;
    }

    public override void MakeArt() {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 6; j++) {
                GameObject g = Instantiate(objectToInstantiate);
                g.GetComponent<Renderer>().material.color = getRandomColor();
                g.transform.localPosition = new Vector3(i+Random.Range(0,10), j+Random.Range(0,10), 0);
                AddToRoot(g.transform);
            }
        }
    }
}