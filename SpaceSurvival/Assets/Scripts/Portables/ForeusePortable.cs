using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeusePortable : Portable
{
    public ForeuseElec foreuseElec;
    private bool etatFonctionnement; //permet de remettre la foreuse dans le même état qu'avant le déplacement

    public List<MeshRenderer> meshRenderers;

    public Material baseMaterial;
    public Material rightMaterial;
    public Material wrongMaterial;

    public List<Collider> _colliders;

    public Collider triggerDetecteur; //Trigger sui permet à la foreuse de detecter une mine

    private void OnTriggerStay(Collider other)
    {
        if (inDeplacement)
        {
            if (other.tag != "Mine")
            {
                canBeConstruct = false;
                foreach (MeshRenderer meshRenderer in meshRenderers)
                    meshRenderer.sharedMaterial = wrongMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.sharedMaterial = rightMaterial;
        }
    }

    public override void ObjectInDeplacement()
    {
        etatFonctionnement = foreuseElec.ON_OffElec;
        foreuseElec.ActiveEngin(false);

        _colliders.ForEach(col => col.isTrigger = true); //

        //triggerDetecteur.enabled = true;

        inDeplacement = true;
        canBeConstruct = true;
        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = rightMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(true);

    }

    public override void ObjectPlaced()
    {
        inDeplacement = false;
        
        foreuseElec.ActiveEngin(etatFonctionnement);

        _colliders.ForEach(col => col.isTrigger = false);
        //triggerDetecteur.enabled = false;

        foreach (MeshRenderer meshRenderer in meshRenderers)
            meshRenderer.sharedMaterial = baseMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(false);

    }
}

