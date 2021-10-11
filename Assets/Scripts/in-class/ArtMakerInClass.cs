using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art;

public class ArtMakerInClass : ArtMakerTemplate
{
    public GameObject objectToInstantiate;

    public override void MakeArt() {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 6; j++) {
                GameObject g = Instantiate(objectToInstantiate);
                g.transform.localPosition = new Vector3(i+Random.Range(0,10), j+Random.Range(0,10), 0);
                AddToRoot(g.transform);
            }
        }
    }
}