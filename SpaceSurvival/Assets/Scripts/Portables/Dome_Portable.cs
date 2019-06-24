using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome_Portable : Portable
{
    public Dome_Conteneur dome_Conteneur;
    public DomeElec domeElec;

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

    [Space]
    public Collider sphereContenerCollider;

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
        inDeplacement = false;

        mainMesh.sharedMaterial = matDefault;                   //On place les materiaux par defaut sur tout les objets
        backSphere.sharedMaterial = matShieldBackFace;
        frontSphere.sharedMaterial = matShieldFront;
        rayon.sharedMaterial = matShieldFront;

        GetComponent<Collider>().isTrigger = false;             //On remet le collider du cube normale
        GetComponent<Connexion>().AllLinksDoUpdate(false);      //On dit à tout les links qu'on va arrêter de bouger

        if (domeElec.ON_OffElec && domeElec.GetAlimentationSuffisante())
        {
            backSphere.enabled = true;                              //On reactive les meshs des spheres pour le feedback
            frontSphere.enabled = true;
            sphereContenerCollider.enabled = true;                  //Ainsi que le collider pour tester si on peut se placer

            dome_Conteneur.Initialize();//Si on le place et qu'il a de l'energie et qu'il est allumé ou initialise le contener
        }
        else
        {
            backSphere.enabled = false;                              //On désactive les meshs des spheres parceque le dome est normalement éteint
            frontSphere.enabled = false;
            sphereContenerCollider.enabled = false;                  //Ainsi que le collider des spheres  
        }

        base.ObjectPlaced();
    }

    public override void ObjectInDeplacement()
    {
        inDeplacement = true;

        backSphere.enabled = true;                              //On reactive les meshs des spheres pour le feedback
        frontSphere.enabled = true;
        sphereContenerCollider.enabled = true;                  //Ainsi que le collider pour tester si on peut se placer

        GetComponent<Collider>().isTrigger = true;              //Passe en trigger le cude du dome

        ChangeRightObject();                                    //On place les materiaux Verts

        GetComponent<Connexion>().AllLinksDoUpdate(true);       //On pense a dire aux links de l'objet qu'on va bouger maintenant
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
