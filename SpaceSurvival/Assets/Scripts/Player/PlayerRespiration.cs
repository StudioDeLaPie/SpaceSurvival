using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespiration : MonoBehaviour
{
    public float maxOxygen;

    [SerializeField] private float _oxygenConsommation;
    private EnvironmentPlayerDetector _environmentDetector;
    private Conteneur _conteneurEnvironnement;
    private PlayerHealth _playerHealth;
    private float _oxygen;

    public float Oxygen { get => _oxygen; set => _oxygen = value; }

    private void Start()
    {
        _environmentDetector = GetComponentInParent<EnvironmentPlayerDetector>();
        _environmentDetector.OnConteneurChange += ConteneurChange;
        _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;
        _playerHealth = GetComponentInParent<PlayerHealth>();
        _oxygen = maxOxygen;
    }

    private void FixedUpdate()
    {
        if (_conteneurEnvironnement == null)
            _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;
        ConsommeOxygen();
        if (_conteneurEnvironnement.Pressure > 80000 && _conteneurEnvironnement.Pressure < 120000)
        {
            InspireOxygen();
        }
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
            _playerHealth.PerteVie(_oxygenConsommation * Time.deltaTime);
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
            float oxygenAspire = maxOxygen * Time.deltaTime * _conteneurEnvironnement.Ratio(EGases.Oxygene);
            oxygenAspire = Mathf.Clamp(oxygenAspire, 0, maxOxygen - _oxygen);
            _oxygen += Mathf.Abs(_conteneurEnvironnement.RemoveGas(EGases.Oxygene, oxygenAspire));
        }
    }

    /// <summary>
    /// Appelé par le Environment PlayerDetector lorsque le conteneur est modifié
    /// </summary>
    private void ConteneurChange()
    {
        _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;
    }
}
