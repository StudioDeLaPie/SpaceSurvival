using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPlayerDetector : MonoBehaviour
{
    public delegate void ConteneurChange();
    public event ConteneurChange OnConteneurChange; //Event de MAJ du conteneur

    [SerializeField] private LayerMask breathableLayers;

    private Conteneur _environmentConteneur;
    private Collider _colliderConteneur = null;
    private Conteneur _atmosphereConteneur;

    public Conteneur EnvironmentConteneur { get => _environmentConteneur; private set => _environmentConteneur = value; }

    private void Awake()
    {
        _atmosphereConteneur = GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Conteneur>();
        _environmentConteneur = _atmosphereConteneur;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((breathableLayers & (1 << other.gameObject.layer)) != 0) //Test si le trigger touché est une des layers respirables
        {
            _environmentConteneur = other.GetComponent<Conteneur>();
            _colliderConteneur = other;
            OnConteneurChange();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_colliderConteneur == null || _colliderConteneur != other)
        {
            _environmentConteneur = other.GetComponent<Conteneur>();
            _colliderConteneur = other;
            OnConteneurChange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _environmentConteneur = _atmosphereConteneur;
        _colliderConteneur = null;
        OnConteneurChange();
    }
}
