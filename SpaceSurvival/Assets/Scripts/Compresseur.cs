﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur : MonoBehaviour
{
    public DetecteurCompresseur detecteurIn;
    public DetecteurCompresseur detecteurOut;

    public GameObject gameObjectIn;
    public GameObject gameObjectOut;

    public Conteneur conteneurIN;
    public Conteneur conteneurOUT;

    public bool ON_OFF = false;

    public float nbParticulesBySecond = 50;
    private float lastTime;
    private float delay = 1;

    private void Start()
    {
        detecteurIn.compresseur = this;
        detecteurOut.compresseur = this;
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
        TurnON_OFF(true);
        lastTime = Time.time;
    }

    public void ObjectInDeplacement()
    {
        TurnON_OFF(false);
    }

    /// <summary>
    /// Permet de récupérer les objets que les detecteurs ont trouvé
    /// </summary>
    public void FindObjectInOut()
    {
        gameObjectIn  = detecteurIn.ObjectFind();
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

    private void TurnON_OFF(bool active)
    {
        if (active)
        {
            ON_OFF = true;
            conteneurIN = gameObjectIn.GetComponent<Conteneur>();
            conteneurOUT = gameObjectOut.GetComponent<Conteneur>();
        }
        else
        {
            ON_OFF = false;
        }
    }
}
