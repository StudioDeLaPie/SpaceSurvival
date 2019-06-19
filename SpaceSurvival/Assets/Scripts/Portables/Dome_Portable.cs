using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_Portable : Portable
{
    public Dome_Conteneur dome_Conteneur;

    [Space]
    [Header("Renderers")]
    public Renderer mainMesh;
    public Renderer backSphere;
    public Renderer frontSphere;
    public Renderer rayon;

    [Space]
    [Header("Materials")]
    public Material matRightFront;
    public Material matRightBackFace;
    public Material matWrongFront;
    public Material matWrongBackFace;
    public Material matShieldFront;
    public Material matShieldBackFace;

    private Material matDefault;

    private void Start()
    {
        base.Start();
        matDefault = mainMesh.sharedMaterial;
    }

    private void Update()
    {
        base.Update();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(other.tag == "Detecteur" || other.tag == "Player"))
        {
            if (inDeplacement)
            {
                ChangeWrongObject();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            ChangeRightObject();
        }
    }


    public override void ObjectPlaced()
    {
        mainMesh.sharedMaterial = matDefault;
        backSphere.sharedMaterial = matShieldBackFace;
        frontSphere.sharedMaterial = matShieldFront;
        rayon.sharedMaterial = matShieldFront;
        inDeplacement = false;
        GetComponent<Collider>().isTrigger = false;

        base.ObjectPlaced();
    }

    public override void ObjectInDeplacement()
    {
        inDeplacement = true;
        GetComponent<Collider>().isTrigger = true;
        ChangeRightObject();
    }


    /// <summary>
    /// Change tout sur l'objet quand il est bon
    /// </summary>
    private void ChangeRightObject()
    {
        canBeConstruct = true;
        mainMesh.sharedMaterial = matRightFront;
        backSphere.sharedMaterial = matRightBackFace;
        frontSphere.sharedMaterial = matRightFront;
        rayon.sharedMaterial = matRightFront;
    }

    /// <summary>
    /// Change tout sur lo'bjet quand il est pas bon
    /// </summary>
    private void ChangeWrongObject()
    {
        canBeConstruct = false;
        mainMesh.sharedMaterial = matWrongFront;
        backSphere.sharedMaterial = matWrongBackFace;
        frontSphere.sharedMaterial = matWrongFront;
        rayon.sharedMaterial = matWrongFront;
    }

    
}
