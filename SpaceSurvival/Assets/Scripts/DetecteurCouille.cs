using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurCouille : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Object Detecté: " + other.name);
    }
}
