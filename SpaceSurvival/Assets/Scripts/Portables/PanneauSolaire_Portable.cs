using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauSolaire_Portable : Portable
{
    public MeshRenderer meshRendererSocle;
    public MeshRenderer meshRendererCellule;

    public Material baseMaterial;
    public Material rightMaterial;
    public Material wrongMaterial;

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
            meshRendererSocle.sharedMaterial = wrongMaterial;
            meshRendererCellule.sharedMaterial = wrongMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDeplacement)
        {
            canBeConstruct = true;
            meshRendererSocle.sharedMaterial = rightMaterial;
            meshRendererCellule.sharedMaterial = rightMaterial;
        }
    }

    public override void ObjectInDeplacement()
    {
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        inDeplacement = true;
        canBeConstruct = true;
        meshRendererSocle.sharedMaterial = rightMaterial;
        meshRendererCellule.sharedMaterial = rightMaterial;
        GetComponent<Connexion>().AllLinksDoUpdate(true);
    }

    public override void ObjectPlaced()
    {
        meshRendererSocle.sharedMaterial = baseMaterial;
        meshRendererCellule.sharedMaterial = baseMaterial;
        inDeplacement = false;
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = false;
        GetComponent<Connexion>().AllLinksDoUpdate(false);
    }
}
