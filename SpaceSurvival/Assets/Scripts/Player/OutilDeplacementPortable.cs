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
    public float rotationSpeed = 5;

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
                mainGameObject.transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);//On met a jour sa position avec la molette de la souris
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (portableObject.canBeConstruct)//Si l'objet nous dit qu'on peut le placer ou il est alors on le place
                    ValidateMainGameObject();
                else
                    CancelDeplacement();

            }

            if (Input.GetMouseButtonDown(0) && mainGameObject != null)
            {
                CancelDeplacement();
            }
        }
    }

    private void CancelDeplacement()
    {
        mainGameObject.transform.position = originalPosition;
        mainGameObject.transform.rotation = originalRotation;
        ValidateMainGameObject();
    }

    private void StopDeplacement()
    {
        inDeplacement = false;
        mainGameObject = null;
    }

    private void ValidateMainGameObject()
    {
        portableObject.ObjectPlaced();
        StopDeplacement();
    }

    /// <summary>
    /// Récupère l'objet touché inGame
    /// </summary>
    /// <param name="portable"></param>
    public void AimingPortable(Portable portable)
    {
        if (Input.GetMouseButtonDown(1))
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
