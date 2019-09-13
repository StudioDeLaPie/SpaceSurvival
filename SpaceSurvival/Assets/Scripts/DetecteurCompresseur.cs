using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurCompresseur : MonoBehaviour
{
    public Compresseur compresseur;

    private Conteneur conteneurFind;
    private Conteneur previousConteneur;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Compresseur" && other.transform.tag != "Player")
        {
            previousConteneur = conteneurFind;
            Conteneur conteneurTouche = other.GetComponent<Conteneur>();
            DomeElec dome = other.transform.root.GetComponentInChildren<DomeElec>();

            if (conteneurTouche != null)
            {
                if (dome != null) //Si le conteneur touché est un dome, il doit être fonctionnel
                {
                    if (dome.Fonctionnel)
                        conteneurFind = conteneurTouche;
                    else
                        conteneurFind = null;
                }
                else
                    conteneurFind = conteneurTouche;
            }

            if (conteneurFind != previousConteneur)
            {
                compresseur.FindObjectInOut();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Conteneur exitConteneur = other.GetComponent<Conteneur>();
        if (exitConteneur != null && exitConteneur == conteneurFind)
        {
            conteneurFind = null;
            compresseur.FindObjectInOut();
        }
    }

    public Conteneur ConteneurFind()
    {
        if (conteneurFind != null)
            return conteneurFind;
        else
            return GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Conteneur>();
    }
}
