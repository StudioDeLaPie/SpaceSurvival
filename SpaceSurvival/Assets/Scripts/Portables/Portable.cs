﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Portable : MonoBehaviour
{
    public bool canBeConstruct = true;
    public delegate void PlacedEvent();
    public event PlacedEvent OnPlaced;

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
    public virtual void ObjectPlaced()
    {
        OnPlaced?.Invoke();
    }

    /// <summary>
    /// méthode lancé par l'outil de déplacment lorsqe le déplacement de l'object est lancé
    /// </summary>
    public abstract void ObjectInDeplacement();

    /// <summary>
    /// Oriente l'objet correctement pour qu'il soit droit sur la planète
    /// </summary>
    protected virtual void OrienteObject()
    {
        _gravityUp = (transform.position - _planetePosition).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(_transform.up, _gravityUp) * _transform.rotation;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}