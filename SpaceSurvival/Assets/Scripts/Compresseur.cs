using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur : MonoBehaviour, I_Elec
{
    [Space]
    public DetecteurCompresseur detecteurIn;
    public DetecteurCompresseur detecteurOut;
    public GameObject particules;

    [Space]
    [SerializeField] private GameObject gameObjectIn;
    [SerializeField] private GameObject gameObjectOut;

    [SerializeField] private Conteneur conteneurIN;
    [SerializeField] private Conteneur conteneurOUT;

    [SerializeField] private bool ON_OFF = false;

    public float nbParticulesBySecond = 50;
    private float lastTime;
    private float delay = 1;

    private void Start()
    {
        detecteurIn.compresseur = this;
        detecteurOut.compresseur = this;
        particules.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (ON_OFF)
        {
            if (Time.time > lastTime + delay && gameObjectIn != gameObjectOut)
            {
                lastTime = Time.time;
                Transfert();
            }
        }
    }

    /// <summary>
    /// Méthode lancé par le compresseurPortable lorsque l'objet est placé
    /// </summary>
    public void ObjectPlaced()
    {
        FindObjectInOut();
        lastTime = Time.time;
    }

    /// <summary>
    /// Permet de récupérer les objets que les detecteurs ont trouvé
    /// </summary>
    public void FindObjectInOut()
    {
        gameObjectIn = detecteurIn.ObjectFind();
        gameObjectOut = detecteurOut.ObjectFind();
    }

    /// <summary>
    /// Déplace les particules au bon ratio de IN à OUT
    /// </summary>
    private void Transfert()
    {
        Gases gasesTransfert = conteneurIN.AspireToutesParticules(nbParticulesBySecond);
        conteneurOUT.AddGases(gasesTransfert);
    }

    public void TurnOn()
    {
        ON_OFF = true;
        FindObjectInOut();
        conteneurIN = gameObjectIn.GetComponent<Conteneur>();
        conteneurOUT = gameObjectOut.GetComponent<Conteneur>();
        particules.SetActive(true);
        Debug.Log("Compresseur ON");
    }

    public void TurnOff()
    {
        ON_OFF = false;
        particules.SetActive(false);
        Debug.Log("Compresseur OFF");
    }
}
