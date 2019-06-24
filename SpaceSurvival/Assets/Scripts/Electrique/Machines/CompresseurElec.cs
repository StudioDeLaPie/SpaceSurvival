using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompresseurElec : ConsoElec
{
    private Compresseur compresseur;

    private void Start()
    {
        compresseur = GetComponent<Compresseur>();
    }

    public override void AlimentationSuffisante(bool suffisant)
    {
        //Si on rentre ici c'est que le reseau vient de changer d'état
        alimentationSuffisante = suffisant;

        if (ON_OffElec && suffisant)
        {
            compresseur.TurnON();
        }
        else
        {
            compresseur.TurnOFF();
        }
    }

    public override void SwitchON_OFF()
    {
        ActiveEngin(!ON_OffElec);
    }
}
