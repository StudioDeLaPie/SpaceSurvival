using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPlayerDetector : MonoBehaviour
{
    public delegate void ConteneurChange();
    public event ConteneurChange OnConteneurChange; //Event de MAJ du conteneur

    [SerializeField] private LayerMask breathableLayers;

    [SerializeField] private Conteneur _environmentConteneur = null;
    [SerializeField] private Collider _colliderConteneur = null;
    [SerializeField] private Dome _dome = null;
    private Conteneur _atmosphereConteneur;

    public Conteneur EnvironmentConteneur { get => _environmentConteneur; private set => _environmentConteneur = value; }

    private void Awake()
    {
        _atmosphereConteneur = GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Conteneur>();
        _environmentConteneur = _atmosphereConteneur;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((breathableLayers & (1 << other.gameObject.layer)) != 0) //Test si le trigger touché est une des layers respirables
        {
            _dome = other.transform.root.GetComponentInChildren<Dome>();

            if (other.GetComponent<Dome_Conteneur>() == null) //Si c'est respirable mais pas un dome (le vaisseau)
            {
                if (_environmentConteneur != other.GetComponent<Conteneur>())
                {
                    _environmentConteneur = other.GetComponent<Conteneur>();
                    _colliderConteneur = other;
                    OnConteneurChange();
                }
            }
            else if (other.GetComponent<Dome_Conteneur>() != null && _dome.fonctionnel) //Pas besoin de tester si _dome est null, s'il y a un dome conteneur c'est que c'est un dome
            {
                if (_environmentConteneur != other.GetComponent<Conteneur>())
                {
                    _environmentConteneur = other.GetComponent<Conteneur>();
                    _colliderConteneur = other;
                    OnConteneurChange();
                }
            }
            else
                SetEnvironmentAtmosphere();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SetEnvironmentAtmosphere();
    }

    private void SetEnvironmentAtmosphere()
    {
        if (_environmentConteneur != _atmosphereConteneur)
        {
            _environmentConteneur = _atmosphereConteneur;
            _colliderConteneur = null;
            OnConteneurChange();
        }
    }
}
