using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionHelper : MonoBehaviour
{
    public bool canBeConstruct = true;

    private void OnTriggerStay(Collider other)
    {
        canBeConstruct = false;
    }

    private void OnTriggerExit(Collider other)
    {
        canBeConstruct = true;
    }
}
