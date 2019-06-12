using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutilDeplacementPortable : MonoBehaviour
{
    [Header("Developper")]
    public bool debugIsActive = false;
    public bool checkCanBePlaced = true;

    [Header("Paramètres")]
    public LayerMask layerMask;
    public float maxRange = 50;
    public float rotationSpeed = 8000;

    private GameObject mainGameObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool canUpdatePosObject = false;
    private Quaternion lastRotation;
    private bool inDeplacement;

    private RaycastHit hit;
    private Portable portableObject;

    private void Update()
    {
        if (inDeplacement)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange, layerMask))
            {
                if (debugIsActive) Debug.DrawRay(transform.position, transform.forward * maxRange, Color.green, 1f);
                mainGameObject.transform.position = hit.point; // On met a jour sa position 
                mainGameObject.transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.fixedDeltaTime);//On met a jour sa position avec la molette de la souris
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (portableObject.canBeConstruct)//Si l'objet nous dit qu'on peut le placer ou il est alors on le place
                    TerminateDeplacement();
                else
                    CancelDeplacement();

            }

            if (Input.GetButtonDown("OutilDeplacementCancel") && mainGameObject != null)
            {
                CancelDeplacement();
            }
        }
    }

    private void CancelDeplacement()
    {
        mainGameObject.transform.position = originalPosition;
        mainGameObject.transform.rotation = originalRotation;
        TerminateDeplacement();
    }

    private void TerminateDeplacement()
    {
        portableObject.ObjectPlaced();
        inDeplacement = false;
        mainGameObject = null;
    }

    /// <summary>
    /// Récupère l'objet touché inGame
    /// </summary>
    /// <param name="portable"></param>
    public void AimingPortable(Portable portable)
    {
        if (Input.GetButtonDown("OutilDeplacement"))
        {
            inDeplacement = true;
            portable.ObjectInDeplacement();

            mainGameObject = portable.gameObject;
            originalPosition = mainGameObject.transform.position;
            originalRotation = mainGameObject.transform.rotation;

            portableObject = portable;
        }
    }
}
