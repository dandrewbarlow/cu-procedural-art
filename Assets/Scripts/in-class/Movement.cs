using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool worldSpace;

    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        // Different transformation options:

        if (worldSpace) {
            this.transform.position = position;
            this.transform.eulerAngles = eulerAngles;

            // only accessible as get method, no setting; use localScale instead
            // this.transform.lossyScale = ;
        } 
        else { //local space
        this.transform.localPosition = position;
        this.transform.localEulerAngles = eulerAngles;
        this.transform.localScale = scale;
        }
   }
}
