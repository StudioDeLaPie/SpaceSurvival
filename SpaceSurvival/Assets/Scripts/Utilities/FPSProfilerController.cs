using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPSProfilerController : MonoBehaviour
{
    public GameObject profiler;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.N))
        {
            profiler.SetActive(!profiler.activeSelf);
        }
    }
}
