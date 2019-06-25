using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur_Portable : Portable
{
    public Compresseur compresseur;
    public CompresseurElec compresseurElec;

    [Space]
    [Header("Renderers")]
    public Renderer detecteurs;
    public Renderer meshCompresseur;

    [Space]
    [Header("Materials")]
    public Material matRightFront;
    public Material matWrongFront;
    public Material matDetecteur;
    public Material matCorpCompresseur;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnTriggerStay(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = false;
            meshCompresseur.sharedMaterial = matWrongFront;
            detecteurs.sharedMaterial = matWrongFront;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            meshCompresseur.sharedMaterial = matRightFront;
            detecteurs.sharedMaterial = matRightFront;
        }
    }

    public override void ObjectPlaced()
    {
        meshCompresseur.sharedMaterial = matCorpCompresseur;
        detecteurs.sharedMaterial = matDetecteur;
        inDeplacement = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Connexion>().AllLinksDoUpdate(false);
        compresseur.ObjectPlaced();     //Permet de dire au compresseur de checker ses detecteurs
        if (compresseurElec.ON_OffElec && compresseurElec.GetAlimentationSuffisante())
        {
            compresseur.TurnOn();
        }
        else
            compresseur.TurnOff();
    }

    public override void ObjectInDeplacement()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        inDeplacement = true;
        canBeConstruct = true;
        meshCompresseur.sharedMaterial = matRightFront;
        detecteurs.sharedMaterial = matRightFront;
        GetComponent<Connexion>().AllLinksDoUpdate(true);
        compresseur.TurnOff();
    }
}
