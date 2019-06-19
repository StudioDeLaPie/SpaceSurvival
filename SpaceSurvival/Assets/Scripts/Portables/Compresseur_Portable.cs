using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur_Portable : Portable
{
    public Compresseur compresseur;

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
        compresseur.ObjectPlaced();
    }

    public override void ObjectInDeplacement()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        inDeplacement = true;
        canBeConstruct = true;
        compresseur.ObjectInDeplacement();
        meshCompresseur.sharedMaterial = matRightFront;
        detecteurs.sharedMaterial = matRightFront;
    }
}
