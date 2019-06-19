using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneteReloader : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            GameObject.FindGameObjectWithTag("Planete").GetComponent<Planet>().RandomPlanete();
        }
    }
}
