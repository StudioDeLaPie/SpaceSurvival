using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseauElec : MonoBehaviour
{
    [SerializeField] private List<GenerateurElec> generateurElecs = new List<GenerateurElec>();
    [SerializeField] private List<ConsoElec> consoElecs = new List<ConsoElec>();
    [SerializeField] private List<BatterieElec> batteries = new List<BatterieElec>();

    [Space]
    public bool actif = false; //Est ce que ce script est le reseau maitre
    private bool etatFonctionnementReseau = false; //Est ce que le reseau fonctionne ou non (pas assez de courant, ou pas de générateur, ...)

    [Space]
    [SerializeField] private float consoTotale;
    [SerializeField] private float prodTotale;
    [SerializeField] private float sommeProdBatteries;

    [Space]
    [SerializeField] private int nbEngins;

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        //Si le reseau doit faire les calculs
        if (actif)
        {
            //ET qu'on a au moins un générateur et (une batterie ou un consommateur)
            //OU qu'on a une batterie et un consommateur
            if (((generateurElecs.Count > 0 && (batteries.Count > 0 || consoElecs.Count > 0)) || (batteries.Count > 0 && consoElecs.Count > 0)))
            {
                if (RefreshConsoTotale() <= RefreshProdTotale())
                {
                    ChangeEtatReseau(true);
                    float surplus = prodTotale - consoTotale;
                    if (surplus > 0)
                    {
                        RechargeBatteries(surplus);
                    }
                }
                //Ou SI on consomme plus que ce qu'on prosuit ET qu'on a au moins une batterie ET que la ou les batteries fournissent assez d'electricité
                else if (consoTotale > prodTotale && batteries.Count > 0 && (SommeProdBatteries() + prodTotale) > consoTotale)
                {
                    ChangeEtatReseau(true);

                    //On demande à toutes les batterie de se vider
                    float consumationBatterie = sommeProdBatteries - (consoTotale - (sommeProdBatteries + prodTotale));
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
            else
            {
                ChangeEtatReseau(false);
                this.actif = false;
            }
        }
    }

    private void ChangeEtatReseau(bool toutFonctionne)
    {
        if (etatFonctionnementReseau != toutFonctionne)
        {
            etatFonctionnementReseau = toutFonctionne;
        }

        consoElecs.ForEach(c => c.AlimentationSuffisante(etatFonctionnementReseau));
        generateurElecs.ForEach(g => g.AlimentationSuffisante(etatFonctionnementReseau));
        batteries.ForEach(b => b.AlimentationSuffisante(etatFonctionnementReseau));

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
        Init();
        foreach (EnginElec engin in engins)
        {
            this.AddEnginToLists(engin);
            engin.reseauMaitre = this;
        }
    }

    private void Init()
    {
        generateurElecs.Clear();
        consoElecs.Clear();
        batteries.Clear();
        nbEngins = 0;

        AddEnginToLists(GetComponent<EnginElec>());
        GetComponent<EnginElec>().reseauMaitre = this;
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
        if (engin is GenerateurElec)
        {
            if (!generateurElecs.Contains((GenerateurElec)engin))
            {
                generateurElecs.Add((GenerateurElec)engin);
                nbEngins++;
            }
        }
        else if (engin is ConsoElec)
        {
            if (!consoElecs.Contains((ConsoElec)engin))
            {
                consoElecs.Add((ConsoElec)engin);
                nbEngins++;
            }
        }
        else if (engin is BatterieElec)
        {
            if (!batteries.Contains((BatterieElec)engin))
            {
                batteries.Add((BatterieElec)engin);
                nbEngins++;
            }
        }
    }

    public void AddEnginToLists(List<EnginElec> engins)
    {
        engins.ForEach(engin => AddEnginToLists(engin));
    }

    public void DeleteEnginToLists(EnginElec engin)
    {
        if (engin is GenerateurElec)
        {
            if (generateurElecs.Remove((GenerateurElec)engin))
                nbEngins--;
        }
        else if (engin is ConsoElec)
        {
            if (consoElecs.Remove((ConsoElec)engin))
                nbEngins--;
        }
        else if (engin is BatterieElec)
        {
            if (batteries.Remove((BatterieElec)engin))
                nbEngins--;
        }
    }

    public void DeleteEnginToLists(List<EnginElec> engins)
    {
        engins.ForEach(engin => DeleteEnginToLists(engin));
    }

    //A OPTIMISER ET/OU FACTORISER
    public void ChangementReseau(ReseauElec nouveauReseau)
    {
        if (nouveauReseau != this)
        {
            foreach (ConsoElec consoElec in consoElecs)
            {
                consoElec.GetComponent<EnginElec>().reseauMaitre = nouveauReseau;
                nouveauReseau.AddEnginToLists(consoElec);
            }

            foreach (GenerateurElec generateur in generateurElecs)
            {
                generateur.GetComponent<EnginElec>().reseauMaitre = nouveauReseau;
                nouveauReseau.AddEnginToLists(generateur);
            }

            foreach (BatterieElec bat in batteries)
            {
                bat.GetComponent<EnginElec>().reseauMaitre = nouveauReseau;
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

    public int NbEngins { get => nbEngins; set => nbEngins = value; }
    public float ConsoTotale { get => consoTotale; set => consoTotale = value; }
    public float ProdTotale { get => prodTotale; set => prodTotale = value; }
}
