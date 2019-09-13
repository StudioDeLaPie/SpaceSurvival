using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur : MonoBehaviour, I_Elec
{
    [SerializeField] private bool fonctionnel = false; //■■■ Il serait cool d'utiliser Fonctionnel de EnginElec à la place ;) (Si t'as un peu de temps fais ça)

    [Space]
    public DetecteurCompresseur detecteurIn;
    public DetecteurCompresseur detecteurOut;
    public GameObject particules;

    public float nbParticulesBySecond = 50;

    [Space]
    [SerializeField] private Conteneur conteneurIN;
    [SerializeField] private Conteneur conteneurOUT;


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
        if (fonctionnel)
        {
            if (Time.time > lastTime + delay && conteneurIN != conteneurOUT)
            {
                lastTime = Time.time;
                Transfert();
            }
        }
    }

    /// <summary>
    /// Méthode lancé par le domePortable et le compresseurPortable lorsque l'objet est placé
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
        conteneurIN = detecteurIn.ConteneurFind();
        conteneurOUT = detecteurOut.ConteneurFind();
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
        fonctionnel = true;
        FindObjectInOut();
        particules.SetActive(true);
    }

    public void TurnOff()
    {
        fonctionnel = false;
        particules.SetActive(false);
    }
}
