using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaisseau : Conteneur
{

    protected override void CalculationVolume()
    {
        volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        CalculationPressure();
    }
}
