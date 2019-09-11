using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurForeuse : MonoBehaviour
{
    public GameObject prefabMinerais;
    
    /// <summary>
    /// Detecte si on est sur une mine
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Mine")
        {
            //haveMine = true;
            prefabMinerais = other.gameObject.GetComponentInParent<Mine>().typeMinerais.prefabs[0];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mine")
        {
            //haveMine = false;
            prefabMinerais = null;
        }
    }
}
