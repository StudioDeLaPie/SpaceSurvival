using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compresseur_Portable : Portable
{
    public Compresseur compresseur;

    [Space]
    [Header("Renderers")]
    public Renderer detecteurIN;
    public Renderer detecteurOUT;

    [Space]
    [Header("Materials")]
    public Material matRightFront;
    public Material matWrongFront;
    public Material matDetecteur;
    public Material matCorpCompresseur;

    private Renderer _renderer;

    protected override void Start()
    {
        base.Start();
        _renderer = GetComponent<Renderer>();
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
            _renderer.sharedMaterial = matWrongFront;
            detecteurIN.sharedMaterial = matWrongFront;
            detecteurOUT.sharedMaterial = matWrongFront;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            _renderer.sharedMaterial = matRightFront;
            detecteurIN.sharedMaterial = matRightFront;
            detecteurOUT.sharedMaterial = matRightFront;
        }
    }

    public override void ObjectPlaced()
    {
        _renderer.sharedMaterial = matCorpCompresseur;
        detecteurIN.sharedMaterial = matDetecteur;
        detecteurOUT.sharedMaterial = matDetecteur;
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
        _renderer.sharedMaterial = matRightFront;
        detecteurIN.sharedMaterial = matRightFront;
        detecteurOUT.sharedMaterial = matRightFront;
    }
}
