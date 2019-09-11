using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilDeplacementPortable : MonoBehaviour
{
    [Header("Paramètres")]
    public List<MonoBehaviour> scriptsADesactiverEnDeplacement;
    public LayerMask layersSurLesquellesPoser;
    public float maxRange = 50;
    public float rotationSpeed = 8000;

    private Portable portableObject;
    private GameObject mainGameObject;
    private Connexion connexionMainGameObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool inDeplacement;

    private RaycastHit hit;

    private void Update()
    {
        if (inDeplacement)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange, layersSurLesquellesPoser))
            {
                mainGameObject.transform.position = hit.point; // On met a jour sa position 
                mainGameObject.transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.fixedDeltaTime);//On met a jour sa position avec la molette de la souris
            }

            if (Input.GetButtonDown("MouseLeft")) //Confirmation du placement
            {
                if (portableObject.canBeConstruct && CheckDistanceOfAllLink())//Si l'objet nous dit qu'on peut le placer ou il est alors on le place
                    TerminateDeplacement();
                else
                    CancelDeplacement();

            }

            if (Input.GetButtonDown("MouseRight") && mainGameObject != null) //Annulation placement
            {
                CancelDeplacement();
            }
        }
    }

    private bool CheckDistanceOfAllLink()
    {
        foreach (Link l in connexionMainGameObject.GetLinks())
        {
            if(!l.LinkHasGoodSize())
            {
                return false;
            }
        }
        return true;
    }

    private void CancelDeplacement()
    {
        mainGameObject.transform.position = originalPosition;
        mainGameObject.transform.rotation = originalRotation;
        connexionMainGameObject.AllLinksInstantRefresh();
        TerminateDeplacement();
    }

    private void TerminateDeplacement()
    {
        portableObject.ObjectPlaced();
        inDeplacement = false;
        mainGameObject = null;
        scriptsADesactiverEnDeplacement.ForEach(mono => mono.enabled = true);
        connexionMainGameObject = null;
    }

    /// <summary>
    /// Récupère l'objet touché inGame
    /// </summary>
    /// <param name="portable"></param>
    public void AimingPortable(Portable portable)
    {
        if (InputCustomMethods.GetButtonHold("Action"))
        {
            scriptsADesactiverEnDeplacement.ForEach(mono => mono.enabled = false);
            inDeplacement = true;
            portable.ObjectInDeplacement();

            mainGameObject = portable.gameObject;
            originalPosition = mainGameObject.transform.position;
            originalRotation = mainGameObject.transform.rotation;

            portableObject = portable;

            connexionMainGameObject = mainGameObject.GetComponent<Connexion>();
        }
    }
}
