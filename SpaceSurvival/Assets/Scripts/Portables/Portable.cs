using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Portable : MonoBehaviour
{
    public bool canBeConstruct = true;

    private Vector3 _gravityUp;
    private Vector3 _planetePosition;
    private Transform _transform;

    [HideInInspector] public bool inDeplacement = false;

    protected virtual void Start()
    {
        _transform = transform;
        _planetePosition = GameObject.FindGameObjectWithTag("Planete").GetComponent<Transform>().position;
    }

    /// <summary>
    /// Base: oriente l'objet au sol
    /// </summary>
    protected virtual void Update()
    {
        if (inDeplacement)
        {
            OrienteObject();
        }
    }

    /// <summary>
    /// Méthode lancé par le constructeur/outil de déplacement lorsque l'objet est placé
    /// </summary>
    public abstract void ObjectPlaced();

    /// <summary>
    /// méthode lancé par l'outil de déplacment lorsqe le déplacement de l'object est lancé
    /// </summary>
    public abstract void ObjectInDeplacement();

    /// <summary>
    /// Oriente l'objet correctement pour qu'il soit droit sur la planète
    /// </summary>
    
    protected virtual void OrienteObject()
    {
        _gravityUp = (_transform.position - _planetePosition).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(_transform.up, _gravityUp) * _transform.rotation;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    [ContextMenu("Mise à plat sur le sol")]
    protected void MiseAPlatInEditMode()
    {
        Vector3 planetePos = GameObject.FindGameObjectWithTag("Planete").GetComponent<Transform>().position;
        Vector3 gravityUp = (transform.position - planetePos).normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;

        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, -transform.up, out hitInfo, 50f);
        transform.position = hitInfo.point;
    }
}
