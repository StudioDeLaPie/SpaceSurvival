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
    public GameObject prefabDome;
    public float maxRange = 50;
    public float scaleSpeed = 5;

    [Header("Materials")]
    public Material matWrong;
    public Material matRight;
    private Material matObject;
    private ConstructionHelper constructionHelper;

    private GameObject mainGameObject;
    private bool canUpdatePosObject = false;
    private RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward * maxRange, out hit, 20f, layerMask))
            {
                mainGameObject = GameObject.Instantiate(prefabDome); //On instantie l'objet
                matObject = mainGameObject.GetComponent<MeshRenderer>().material; //On récupère son méterial pour lui remettre quand il serra posé
                constructionHelper = mainGameObject.GetComponent<ConstructionHelper>(); //On résupère son construction helper pour connaitre son état
                canUpdatePosObject = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (!constructionHelper.canBeConstruct && checkCanBePlaced)
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

    private void UpdatePosObject()
    {
        if (canUpdatePosObject)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward * maxRange, out hit, 20f, layerMask))
            {
                if (debugIsActive) Debug.DrawRay(transform.position, transform.forward * maxRange, Color.green, 1f);
                mainGameObject.transform.position = hit.point;
                mainGameObject.transform.localScale += Vector3.one * scaleSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
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
        mainGameObject = null;
    }
}
