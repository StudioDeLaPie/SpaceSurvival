﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_Portable : Portable
{
    public Dome_Conteneur dome_Conteneur;

    [Space]
    [Header("Renderers")]
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

    private Renderer renderer;

    private void Start()
    {
        base.Start();
        renderer = GetComponent<Renderer>();
        matDefault = renderer.sharedMaterial;
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
        renderer.sharedMaterial = matDefault;
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
        renderer.sharedMaterial = matRightFront;
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
        renderer.sharedMaterial = matWrongFront;
        backSphere.sharedMaterial = matWrongBackFace;
        frontSphere.sharedMaterial = matWrongFront;
        rayon.sharedMaterial = matWrongFront;
    }

    
}