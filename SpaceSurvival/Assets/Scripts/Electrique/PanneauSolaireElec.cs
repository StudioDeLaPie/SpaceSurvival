using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanneauSolaireElec : GenerateurElec
{
    public override void SwitchON_OFF()
    {
        ON_OffElec = !ON_OffElec;
    }
}
