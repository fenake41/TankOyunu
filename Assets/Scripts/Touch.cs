using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Sense
{
    public override void initiliaze()
    {
       
    }

    public override void updateSense()
    {
        //if (aspect)
        //    Debug.Log(aspect.aspectType + "Has Been Touched");
    }

    private void OnTriggerEnter(Collider other)
    {
        aspect = other.GetComponent<Aspect>();
    }

    private void OnTriggerExit(Collider other)
    {
        aspect = null;
    }
}
