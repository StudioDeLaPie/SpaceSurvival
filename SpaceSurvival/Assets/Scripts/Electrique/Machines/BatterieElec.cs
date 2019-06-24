using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterieElec : EnginElec
{
    private float quantiteElectricite = 0; //Stockage
    private float debitElectrique = 10; //Vitesse de transfert electrique
    private float quantiteElectriciteMax = 1000;

    /// <summary>
    /// Dépend de du débit et de la charge actuel
    /// </summary>
    /// <returns></returns>
    public float GetProd()
    {
        if (ON_OffElec)
            return quantiteElectricite;
        else
            return 0;
    }
    /// <summary>
    /// Permet de consommer la batterie
    /// </summary>
    /// <param name="consumationCharge"></param>
    /// <returns>Return la quantité vraiment consommé</returns>
    public float Consumation(float consumationCharge)
    {
        if (ON_OffElec)
        {
            float energieAConsomme = Mathf.Clamp(consumationCharge, 0, debitElectrique);
            if (energieAConsomme <= quantiteElectricite) //Si j'ai assez d'energie
            {
                quantiteElectricite -= energieAConsomme;
                return energieAConsomme;
            }
            else //if (energieAConsomme > quantiteElectricite) //Si j'enlève plus d'électricité que ce j'ai a disponibilité
            {
                float energieVraimentConsomme = quantiteElectricite;
                quantiteElectricite = 0;
                return energieVraimentConsomme;
            }
        }
        else
            return 0;
    }

    /// <summary>
    /// Permet de recharger la batterie
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns>return la quantité d'électricité stocké</returns>
    public float Recharge(float quantity)
    {
        if(quantity > debitElectrique) //Si mon débit ne me permet pas de tout prendre
        {
            quantity = Mathf.Clamp(quantity, 0, debitElectrique);
        }
        
        if (quantity + quantiteElectricite < quantiteElectriciteMax) //Si je remplie pas ma batterie au max si je prend tout
        {
            quantiteElectricite += quantity;
            return quantity;
        }
        else
        {
            float reelQuantity = quantity - ((quantity + quantiteElectricite) - quantiteElectriciteMax);
            quantiteElectricite += reelQuantity;
            return reelQuantity;
        }
    }

    public override void SwitchON_OFF()
    {
        throw new System.NotImplementedException();
    }
}
