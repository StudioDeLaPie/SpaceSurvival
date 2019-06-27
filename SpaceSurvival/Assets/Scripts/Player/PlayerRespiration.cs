using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespiration : MonoBehaviour
{
    public float maxOxygen;
    public float _currentOxygen;
    public float _oxygenConsommation = 0.01f;
    public float _speedFillOxygen = 1;
    public float _loseHealthMultiplayer = 20;

    public float _minPressureBreathable = 80000;
    public float _maxPressureBreathable = 120000;

    private EnvironmentPlayerDetector _environmentDetector;
    private Conteneur _conteneurEnvironnement;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _environmentDetector = GetComponentInParent<EnvironmentPlayerDetector>();
        _environmentDetector.OnConteneurChange += ConteneurChange;
        _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;
        _playerHealth = GetComponentInParent<PlayerHealth>();
        _currentOxygen = maxOxygen;
    }

    private void FixedUpdate()
    {
        if (_conteneurEnvironnement == null)
            _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;

        ConsommeOxygen();

        //Si l'environement est respirable
        if (_conteneurEnvironnement.Pressure > _minPressureBreathable && _conteneurEnvironnement.Pressure < _maxPressureBreathable)
            FillBonbonne();
    }

    /// <summary>
    /// Consomme l'oxygène en stock dans la bombonne
    /// </summary>
    private void ConsommeOxygen()
    {
        if (_currentOxygen > _oxygenConsommation && _currentOxygen > 0) //Si il nous reste assez d'oxygène dans le bonbonne
        {
            _currentOxygen -= _oxygenConsommation;
            _conteneurEnvironnement.AddGas(EGases.Gaz, _oxygenConsommation);
        }
        else
            _playerHealth.PerteVie(_oxygenConsommation * _loseHealthMultiplayer);
    }

    /// <summary>
    /// Remplit la bonbonne avec l'oxygène ambiant à une vitesse proportionnelle au ratio d'oxygène
    /// </summary>
    private void FillBonbonne()
    {
        //Remplit la jauge d'oxygène si besoin
        if (_currentOxygen < maxOxygen)
        {
            float oxygenAspire = _speedFillOxygen * _conteneurEnvironnement.Ratio(EGases.Oxygene);
            oxygenAspire = Mathf.Clamp(oxygenAspire, 0, (float)((decimal)maxOxygen - (decimal)_currentOxygen));
            _currentOxygen += Mathf.Abs(_conteneurEnvironnement.RemoveGas(EGases.Oxygene, oxygenAspire));
        }
    }

    /// <summary>
    /// Appelé par le Environment PlayerDetector lorsque le conteneur est modifié
    /// </summary>
    private void ConteneurChange()
    {
        _conteneurEnvironnement = _environmentDetector.EnvironmentConteneur;
    }

    public float Oxygen { get => _currentOxygen; set => _currentOxygen = value; }
}
