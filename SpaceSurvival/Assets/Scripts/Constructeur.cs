using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructeur : MonoBehaviour
{
    [Header("Developper")]
    public bool debugIsActive = false;
    public bool checkCanBePlaced = true;

    [Header("Paramètres")]
    public LayerMask layerMask;
    public List<GameObject> prefabGameObject = new List<GameObject>();
    public float maxRange = 50;
    public float rotationSpeed = 5;

    [Header("Materials")]
    public Material matWrong;
    public Material matRight;
    private Material matObject;
    private ConstructionHelper constructionHelper;

    private int indexObjectSelected = 1;
    private GameObject mainGameObject;
    private bool canUpdatePosObject = false;
    private RaycastHit hit;
    private Quaternion lastRotation;

    public GameObject SelectedObject { get => prefabGameObject[indexObjectSelected]; }

    private void Update()
    {
        SelectObject();

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRange, layerMask))
            {
                mainGameObject = GameObject.Instantiate(prefabGameObject[indexObjectSelected]); //On instantie l'objet
                matObject = mainGameObject.GetComponent<MeshRenderer>().material; //On récupère son material pour lui remettre quand il serra posé
                mainGameObject.transform.rotation = lastRotation; // On lui donne la rotation du dernier objet placé
                constructionHelper = mainGameObject.GetComponent<ConstructionHelper>(); //On résupère son construction helper pour connaitre son état
                canUpdatePosObject = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (!constructionHelper.canBeConstruct && checkCanBePlaced) //CheckCanBePlaced est une option pour poser les object partout sans vérification
                DestructMainGameObject();
            else
                ValidateMainGameObject();
        }

        if (Input.GetMouseButtonDown(0) && mainGameObject != null)
        {
            DestructMainGameObject();
        }

        if (mainGameObject != null)
        {
            UpdatePosObject();
            UpdateMaterialObject();
        }
    }

    private void SelectObject()
    {
        if (Input.GetKeyDown("1"))
        {
            indexObjectSelected = 0;
        }
        else if (Input.GetKeyDown("2"))
        {
            indexObjectSelected = 1;
        }
    }

    private void UpdatePosObject()
    {
        if (canUpdatePosObject)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward * maxRange, out hit, 20f, layerMask))
            {
                if (debugIsActive) Debug.DrawRay(transform.position, transform.forward * maxRange, Color.green, 1f);
                mainGameObject.transform.position = hit.point;
                mainGameObject.transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);
                lastRotation = mainGameObject.transform.rotation;
            }
        }
    }

    private void DestructMainGameObject()
    {
        Destroy(mainGameObject);
        mainGameObject = null;
    }

    private void UpdateMaterialObject()
    {
        if (constructionHelper.canBeConstruct)
        {
            mainGameObject.GetComponent<MeshRenderer>().material = matRight;
        }
        else
        {
            mainGameObject.GetComponent<MeshRenderer>().material = matWrong;
        }
    }

    private void ValidateMainGameObject()
    {
        mainGameObject.GetComponent<MeshRenderer>().material = matObject; //On lui remet son material
        Destroy(mainGameObject.GetComponent<ConstructionHelper>()); //On detruit le construction helper pour qu'il ne tourne pas pour rien
        canUpdatePosObject = false;
        mainGameObject.SendMessage("ObjectPlaced");
        mainGameObject = null;
    }
}
