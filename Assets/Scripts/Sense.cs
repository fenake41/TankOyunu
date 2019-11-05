using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISense
{
    void initiliaze();
    void updateSense();
}

public abstract class Sense : MonoBehaviour, ISense
{
    public abstract void initiliaze();
    public abstract void updateSense();
    protected Aspect aspect;
    public float checkFreq = 5;
    private float timeFromLastCheck;

    void start()
    {
        initiliaze();
    }
    void update()
    {
        if((timeFromLastCheck += Time.deltaTime) >= 1f/checkFreq)
        {
            updateSense();
            timeFromLastCheck = 0;
        }
    }
    
}
