using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurCompresseur : MonoBehaviour
{
    private GameObject objectFind;
    public Compresseur compresseur;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Compresseur" && other.transform.tag != "Player")
        {
            Portable p = other.transform.root.GetComponentInChildren<Portable>();
            if(p!= null)
            {
                p.OnPlaced += compresseur.ObjectPlaced;
            }
            objectFind = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == objectFind)
        {
            objectFind = null;
        }
    }

    public GameObject ObjectFind()
    {
        if (objectFind != null)
            return objectFind;
        else
            return GameObject.FindGameObjectWithTag("Atmosphere");
    }
}
