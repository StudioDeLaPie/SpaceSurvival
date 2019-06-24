using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprimanteElec : ConsoElec
{
    private Imprimante imprimante;

    private void Start()
    {
        imprimante = GetComponent<Imprimante>();
    }

    public override void AlimentationSuffisante(bool suffisant)
    {
        alimentationSuffisante = suffisant;

        if (ON_OffElec && suffisant)
        {
            imprimante.TurnON();
        }
        else
        {
            imprimante.TurnOFF();
        }
    }

    public override void SwitchON_OFF()
    {
        ON_OffElec = !ON_OffElec;
    }
}
