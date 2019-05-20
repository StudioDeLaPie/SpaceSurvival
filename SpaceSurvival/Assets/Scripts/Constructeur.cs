using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructeur : MonoBehaviour
{
    public LayerMask layerMask;
    public bool debugIsActive = false;
    public GameObject prefabDome;
    public float maxRange = 50;
    public float scaleSpeed = 5;

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
                mainGameObject = GameObject.Instantiate(prefabDome);
                canUpdatePosObject = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            canUpdatePosObject = false;
            mainGameObject = null;
        }

        if (Input.GetMouseButtonDown(0) && mainGameObject != null)
        {
            Destroy(mainGameObject);
            mainGameObject = null;
            canUpdatePosObject = false;
        }

        UpdatePosObject();

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
}
