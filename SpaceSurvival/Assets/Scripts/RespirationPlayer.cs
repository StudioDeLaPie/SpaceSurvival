using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespirationPlayer : MonoBehaviour
{
    public float maxOxygen;

    [SerializeField] private float _oxygenConsommation;
    private EnvironmentPlayerDetector _environment;
    private Conteneur _conteneurEnvironnement;
    private float _oxygen;


    private void Start()
    {
        _environment = GetComponentInParent<EnvironmentPlayerDetector>();
        _environment.OnConteneurChange += ConteneurChange;
        _conteneurEnvironnement = _environment.EnvironmentConteneur;
        _oxygen = maxOxygen;
    }

    private void Update()
    {
        ConsommeOxygen();
        if (_conteneurEnvironnement.Pressure > 80000 && _conteneurEnvironnement.Pressure < 120000)
        {
            Debug.Log("Bonne pression");
            InspireOxygen();
        }

        Debug.Log("Oxygen : " + _oxygen);
    }

    /// <summary>
    /// Consomme l'oxygène en stock dans la bombonne
    /// </summary>
    private void ConsommeOxygen()
    {
        float consommation = 0;
        if (_oxygen > 0)
        {
            consommation = Mathf.Clamp(_oxygenConsommation * Time.deltaTime, 0, _oxygen);
            _oxygen -= consommation;
        }
        else
            PerteVie();
        _conteneurEnvironnement.AddGas(EGases.Gaz, consommation);
    }

    /// <summary>
    /// Remplit la bombonne avec l'oxygène ambiant à une vitesse proportionnelle au ratio d'oxygène
    /// </summary>
    private void InspireOxygen()
    {
        //Remplit la jauge d'oxygène si besoin
        if (_oxygen < maxOxygen)
        {
            Debug.Log("Remplissage bonbonne");

            float oxygenAspire = maxOxygen * Time.deltaTime * _conteneurEnvironnement.Ratio(EGases.Oxygene);
            oxygenAspire = Mathf.Clamp(oxygenAspire, 0, maxOxygen - _oxygen);
            _oxygen += Mathf.Abs(_conteneurEnvironnement.AddGas(EGases.Oxygene, -oxygenAspire));
        }
    }

    private void PerteVie()
    {
        Debug.Log("bleurg (plus d'air)");
    }

    /// <summary>
    /// Appelé par le Environment PlayerDetector lorsque le conteneur est modifié
    /// </summary>
    private void ConteneurChange()
    {
        _conteneurEnvironnement = _environment.EnvironmentConteneur;
    }
}
