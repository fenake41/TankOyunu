using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    Animator fsm;
    Aspect asp;
    Vector3 dir;
    public Transform player;
    public float fieldOfView = 60; 

    public override void initiliaze()
    {
        
    }

    public override void updateSense()
    {
        fsm = GetComponent<Animator>();

        if(player != null)
        {
             dir = player.position - transform.position;
        }
                

        //cosAngle = a.b / |a|*|b|
        float cosAngle = Vector3.Dot(transform.forward, dir) / (transform.forward.magnitude * dir.magnitude);
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
       

        if (angle < fieldOfView / 2f)
        {
            Debug.DrawRay(transform.position, dir, Color.red);
            if(Physics.Raycast(transform.position, dir , out RaycastHit info,20f))
            {
                asp = info.transform.GetComponent<Aspect>();
                if (asp)
                    fsm.SetBool("isVisible", true);

                else
                    fsm.SetBool("isVisible", false);
            }

            else
                fsm.SetBool("isVisible", false);
        }
       
    }

}
