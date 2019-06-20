﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseauElec : MonoBehaviour
{
   [SerializeField] private List<GenerateurElec> generateurElecs = new List<GenerateurElec>();
   [SerializeField] private List<ConsoElec> consoElecs = new List<ConsoElec>();
   [SerializeField] private List<BatterieElec> batteries = new List<BatterieElec>();
    
   [SerializeField] private bool actif = false; //Est ce que ce script est le reseau maitre
   [SerializeField] private bool etatFonctionnementReseau = false; //Est ce que le reseau fonctionne ou non (pas assez de courant, ou pas de générateur, ...)
    
   [SerializeField] private float consoTotal;
   [SerializeField] private float prodTotal;
   [SerializeField] private float sommeProdBatteries;

    public int nbEngins;

    private void Start()
    {
        //AddEnginToLists(GetComponent<EnginElec>());
    }

    private void FixedUpdate()
    {
        //Si le reseau doit faire les calculs
        //ET qu'on a au moins un générateur et (une batterie ou un consommateur)
        //OU qu'on a une batterie et un consommateur
        if (actif && ((generateurElecs.Count > 0 && (batteries.Count > 0 || consoElecs.Count > 0)) || (batteries.Count > 0 && consoElecs.Count > 0)))
        {
            if (ConsoTotal() <= ProdTotal())
            {
                ChangeEtatReseau(true);
                float surplus = prodTotal - consoTotal;
                if (surplus > 0)
                {
                    RechargeBatteries(surplus);
                }
            }
            //Ou SI on consomme plus que ce qu'on prosuit ET qu'on a au moins une batterie ET que la ou les batteries fournissent assez d'electricité
            else if (consoTotal > prodTotal && batteries.Count > 0 && (SommeProdBatteries() + prodTotal) > consoTotal)
            {
                ChangeEtatReseau(true);

                //On demande à toutes les batterie de se vider
                float consumationBatterie = sommeProdBatteries - (consoTotal - (sommeProdBatteries + prodTotal));
                foreach (BatterieElec batterie in batteries)
                {
                    consumationBatterie -= batterie.Consumation(consumationBatterie);
                    if (consumationBatterie >= 0)
                        break;
                }
            }
            else
            {
                ChangeEtatReseau(false);
            }
        }
    }

    private void ChangeEtatReseau(bool toutFonctionne)
    {
        if (etatFonctionnementReseau != toutFonctionne)
        {
            etatFonctionnementReseau = toutFonctionne;
        }

        foreach (ConsoElec consoElec in consoElecs)
        {
            consoElec.AlimentationSuffisante(etatFonctionnementReseau);
        }

        /*  EVENTS FEEDBACK
        if (etatFonctionnementReseau)
        //Reseau de nouveau fonctionnel
        else
        //pénurie electrique
        */
    }

    /// <summary>
    /// Active le reseau et récupère tout les engins donnés en paramètre
    /// </summary>
    /// <param name="engins">Paramètre a assimiler </param>
    public void CreationReseau(List<EnginElec> engins)
    {
        this.actif = true;
        foreach (EnginElec engin in engins)
        {
            this.AddEnginToLists(engin);
            engin.reseauElec = this;
        }
    }

    private float SommeProdBatteries()
    {
        float result = 0;
        foreach (BatterieElec bat in batteries)
        {
            result += bat.GetProd();
        }
        return result;
    }

    /// <summary>
    /// Demande a toutes les batteries de se recharger en fonction que ce qu'elle peuvent
    /// </summary>
    /// <param name="surplus"></param>
    private void RechargeBatteries(float surplus)
    {
        float compteur = surplus;
        foreach (BatterieElec batterie in batteries)
        {
            compteur -= batterie.Recharge(compteur);
        }
    }

    /// <summary>
    /// Ne prend pas en compte les batteries
    /// </summary>
    /// <returns></returns>
    private float ConsoTotal()
    {
        float result = 0;
        foreach (ConsoElec engin in consoElecs)
        {
            result += engin.GetConsommation();
        }
        consoTotal = result;
        return result;
    }

    /// <summary>
    /// Ne prend pas en compte les batteries
    /// </summary>
    /// <param name="engin"></param>
    private float ProdTotal()
    {
        float result = 0;
        foreach (GenerateurElec engin in generateurElecs)
        {
            result += engin.GetProduction();
        }
        consoTotal = result;
        return result;
    }

    public void AddEnginToLists(EnginElec engin)
    {
        if (engin is GenerateurElec)
        {
            generateurElecs.Add((GenerateurElec)engin);
        }
        else if (engin is ConsoElec)
        {
            consoElecs.Add((ConsoElec)engin);
        }
        else if (engin is BatterieElec)
        {
            batteries.Add((BatterieElec)engin);
        }
    }

    public void DeleteEnginToLists(EnginElec engin)
    {
        if (engin is GenerateurElec)
        {
            generateurElecs.Remove((GenerateurElec)engin);
        }
        else if (engin is ConsoElec)
        {
            consoElecs.Remove((ConsoElec)engin);
        }
        else if (engin is BatterieElec)
        {
            batteries.Remove((BatterieElec)engin);
        }
    }

    //A OPTIMISER ET/OU FACTORISER
    public void ChangementReseau(ReseauElec nouveauReseau)
    {
        if(nouveauReseau != this)
        {
            foreach (ConsoElec consoElec in consoElecs)
            {
                consoElec.GetComponent<EnginElec>().reseauElec = nouveauReseau;
                nouveauReseau.AddEnginToLists(consoElec);
            }

            foreach (GenerateurElec generateur in generateurElecs)
            {
                generateur.GetComponent<EnginElec>().reseauElec = nouveauReseau;
                nouveauReseau.AddEnginToLists(generateur);
            }

            foreach (BatterieElec bat in batteries)
            {
                bat.GetComponent<EnginElec>().reseauElec = nouveauReseau;
                nouveauReseau.AddEnginToLists(bat);
            }
        }

        /*Connexion myConnexion = GetComponent<Connexion>();
        List<Connexion> allConnexions = myConnexion.GetConnexions();

        bool doChangementReseau = false;

        foreach (Connexion co in allConnexions)
        {
            if()
        }*/

    }
}