using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCast : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            bool clicked = Physics.Raycast(Camera.main.transform.position, ray.direction, out hit);
            // hit.

            Debug.Log(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward));

            if (clicked)
            {
                target.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                target.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
