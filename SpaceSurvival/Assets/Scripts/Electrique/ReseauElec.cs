using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseauElec : MonoBehaviour
{
    [SerializeField] private List<GenerateurElec> generateurElecs = new List<GenerateurElec>();
    [SerializeField] private List<ConsoElec> consoElecs = new List<ConsoElec>();
    [SerializeField] private List<BatterieElec> batteries = new List<BatterieElec>();
    private List<EnginElec> allEngins = new List<EnginElec>();

    [Space]
    public bool actif = false; //Est ce que ce script est le reseau maitre
    [SerializeField] private bool etatFonctionnementReseau = false; //Est ce que le reseau fonctionne ou non (pas assez de courant, ou pas de générateur, ...)
    private bool creationFrame = false; //Passe à true à la création du réseau et false ensuite
    private bool processedThisFrame = false;

    [Space]
    [SerializeField] private float consoTotale;
    [SerializeField] private float prodTotale;
    [SerializeField] private float sommeProdBatteries;

    [Space]
    [SerializeField] private int nbEngins;

    private void Start()
    {
        Init();
        ChangeEtatReseau(false);
    }

    private void FixedUpdate()
    {
        ProcessReseau();
        processedThisFrame = false;
    }

    /// <summary>
    /// Fonction principale du réseau. Refresh l'état et fait tout.
    /// (anciennement dans le Fixed Update. Ne fait rien si déjà processed dans cette frame)
    /// </summary>
    public void ProcessReseau()
    {
        if (!processedThisFrame)
        {
            //Si le reseau doit faire les calculs
            if (actif)
            {
                //ET qu'on a au moins un générateur et (une batterie ou un consommateur)
                //OU qu'on a une batterie et un consommateur
                if (((generateurElecs.Count > 0 && (batteries.Count > 0 || consoElecs.Count > 0)) || (batteries.Count > 0 && consoElecs.Count > 0)))
                {
                    if (RefreshConsoTotale() <= RefreshProdTotale() && prodTotale > 0)
                    {
                        ChangeEtatReseau(true);
                        float surplus = prodTotale - consoTotale;
                        if (surplus > 0)
                        {
                            RechargeBatteries(surplus);
                        }
                    }
                    //Ou SI on consomme plus que ce qu'on produit ET qu'on a au moins une batterie ET que la ou les batteries fournissent assez d'electricité
                    else if (consoTotale > prodTotale && batteries.Count > 0 && (SommeProdBatteries() + prodTotale) > consoTotale)
                    {
                        ChangeEtatReseau(true);

                        //On demande à toutes les batterie de se vider
                        float consumationBatterie = consoTotale - prodTotale; //Energie, hors prod, à tirer des batteries
                        foreach (BatterieElec batterie in batteries)
                        {
                            consumationBatterie -= batterie.Consumation(consumationBatterie);
                            if (consumationBatterie <= 0)
                                break;
                        }
                    }
                    else
                    {
                        ChangeEtatReseau(false);
                    }
                }
                else
                {
                    RefreshConsoTotale();
                    RefreshProdTotale();
                    ChangeEtatReseau(false);
                    this.actif = false;
                }
            }
            processedThisFrame = true;
        }
    }

    private void ChangeEtatReseau(bool toutFonctionne)
    {
        if (etatFonctionnementReseau != toutFonctionne || creationFrame)
        {
            creationFrame = false;
            etatFonctionnementReseau = toutFonctionne;
            ChangeEtatAllEngins(etatFonctionnementReseau);
        }
    }

    private void ChangeEtatAllEngins(bool etat)
    {
        consoElecs.ForEach(c => c.AlimentationSuffisante(etat));
        generateurElecs.ForEach(g => g.AlimentationSuffisante(etat));
        batteries.ForEach(b => b.AlimentationSuffisante(etat));
    }

    private void Init()
    {
        generateurElecs.Clear();
        consoElecs.Clear();
        batteries.Clear();
        allEngins.Clear();
        nbEngins = 0;

        actif = false;
        AddEnginToLists(GetComponent<EnginElec>());
    }

    /// <summary>
    /// Active le reseau et récupère tout les engins donnés en paramètre
    /// </summary>
    /// <param name="engins">Paramètre a assimiler </param>
    public void CreationReseau(List<EnginElec> engins)
    {
        Init();
        this.actif = true;
        AddEnginToLists(engins);
        creationFrame = true;
        ProcessReseau();
    }

    private float SommeProdBatteries()
    {
        float result = 0;
        foreach (BatterieElec bat in batteries)
        {
            result += bat.GetProd();
        }
        sommeProdBatteries = result;
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
    private float RefreshConsoTotale()
    {
        float result = 0;
        foreach (ConsoElec engin in consoElecs)
        {
            result += engin.GetConsommation();
        }
        consoTotale = result;
        return result;
    }

    /// <summary>
    /// Ne prend pas en compte les batteries
    /// </summary>
    /// <param name="engin"></param>
    private float RefreshProdTotale()
    {
        float result = 0;
        foreach (GenerateurElec engin in generateurElecs)
        {
            result += engin.GetProduction();
        }
        prodTotale = result;
        return result;
    }

    public void AddEnginToLists(EnginElec engin)
    {
        if (!allEngins.Contains(engin))
        {
            engin.reseauMaitre = this; //L'engin ajouté est averti que ce réseau est son nouveau maitre
            allEngins.Add(engin);
            nbEngins++;

            if (engin is GenerateurElec)
                generateurElecs.Add((GenerateurElec)engin);
            else if (engin is ConsoElec)
                consoElecs.Add((ConsoElec)engin);
            else if (engin is BatterieElec)
                batteries.Add((BatterieElec)engin);
        }
    }

    public void DeleteEnginFromLists(EnginElec engin)
    {
        if (allEngins.Contains(engin))
        {
            allEngins.Remove(engin);
            nbEngins--;

            if (engin is GenerateurElec) generateurElecs.Remove((GenerateurElec)engin);
            else if (engin is ConsoElec) consoElecs.Remove((ConsoElec)engin);
            else if (engin is BatterieElec) batteries.Remove((BatterieElec)engin);
        }
    }

    /// <summary>
    /// Si le réseau passé en paramètre est un nouveau réseau, l'ancien transmet ses engins et se reset
    /// </summary>
    /// <param name="nouveauReseau"></param>
    public void ChangementReseau(ReseauElec nouveauReseau)
    {
        if (nouveauReseau != this) //Si le reseau maitre change
        {
            List<EnginElec> enginsTemp = new List<EnginElec>(allEngins);
            Init();
            nouveauReseau.AddEnginToLists(enginsTemp); //Le nouveau réseau est informé
        }
        else //Si on est le reseau qui reste et qui a donc reçu de nouveaux engins
        {
            creationFrame = true; //On force la MAJ de l'état du réseau et de ses engins
        }
    }


    // ■■■  Demandent un Process avant de renvoyer la valeur. Assure que la valeur est à jour. ■■■
    public int NbEngins { get { ProcessReseau(); return nbEngins; } set => nbEngins = value; }
    public float ConsoTotale { get { ProcessReseau(); return consoTotale; } set => consoTotale = value; }
    public float ProdTotale { get { ProcessReseau(); return prodTotale; } set => prodTotale = value; }
    public bool EtatFonctionnementReseau { get { ProcessReseau(); return etatFonctionnementReseau; } set => etatFonctionnementReseau = value; }

    public void AddEnginToLists(List<EnginElec> engins)
    {
        engins.ForEach(engin => AddEnginToLists(engin));
    }

    public void DeleteEnginFromLists(List<EnginElec> engins)
    {
        engins.ForEach(engin => DeleteEnginFromLists(engin));
    }
}
