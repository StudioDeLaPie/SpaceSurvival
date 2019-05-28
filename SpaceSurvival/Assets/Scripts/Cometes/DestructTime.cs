using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructTime : MonoBehaviour
{
    public bool CoutDownStarted = false;
    public float delay = 120;

    private float lastTime;

    void Update()
    {
        if(CoutDownStarted)
        {
            if (lastTime == 0)
                lastTime = Time.time;

            if(Time.time > delay + lastTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
