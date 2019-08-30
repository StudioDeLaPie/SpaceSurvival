using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterieElec : EnginElec
{
    [SerializeField]private float quantiteElectricite = 0; //Stockage
    [SerializeField]private float debitElectrique = 10; //Vitesse de transfert electrique
    [SerializeField]private float quantiteElectriciteMax = 1000;

    public bool canRecharge = true; //Définit si on peut Recharger la batterie
    public bool canConsume = true; //Et si on peut consommer son energie

    public float QuantiteElectricite { get => quantiteElectricite; set => quantiteElectricite = value; }
    public float QuantiteElectriciteMax { get => quantiteElectriciteMax; set => quantiteElectriciteMax = value; }

    /// <summary>
    /// Dépend de du débit et de la charge actuel
    /// </summary>
    /// <returns></returns>
    public float GetProd()
    {
        if (canConsume)
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
        if (canRecharge)
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
        if (canRecharge)
        {
            if (quantity > debitElectrique) //Si mon débit ne me permet pas de tout prendre
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
        else
            return 0;
    }

    override
    public void SwitchON_OFF()
    {
        ON_OffElec = !ON_OffElec; //On switch

        if(ON_OffElec) //Si on est allumé
        {
            canConsume = true;
            canRecharge = true;
        }
        else //Si on est etteint 
        {
            canConsume = false;
            canRecharge = true;
        }
    }
}
