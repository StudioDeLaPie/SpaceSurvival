using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauSolaireElec : GenerateurElec
{
    [SerializeField] private Transform panneau;
    [SerializeField, Tooltip("Angle = 0 quand soleil au dessus et 90 à l'horizon")] private float maxWorkingAngle;
    [SerializeField, Tooltip("Angle = 0 quand soleil au dessus et 90 à l'horizon")] private float maxInclinaisonAngle;
    private Transform sun;
    private float defaultPuissance;

    private void OnEnable()
    {
        sun = GameObject.Find("Sun").transform;
        defaultPuissance = puissance;
        CheckSunlight();
        InvokeRepeating("CheckSunlight", Random.value * 10, 3);
    }

    private void CheckSunlight()
    {
        float angle = Vector3.Angle(-transform.up, -sun.position);
        if (angle <= maxWorkingAngle)
        {
            if (angle <= maxInclinaisonAngle)
                panneau.forward = sun.position - panneau.position;
            puissance = defaultPuissance;
        }
        else
        {
            puissance = 0;
        }
    }
}
