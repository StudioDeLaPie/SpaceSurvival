using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Craft_SO;

public class Imprimante : MonoBehaviour
{
    public bool ON_OFF = false;
    public List<Craft_SO> craftPossibles;

    [SerializeField] private Transform spawnPoint;
    private Connexion connexion;
    private List<ComposantRecette> composantsDisponibles;

    private void Start()
    {
        connexion = GetComponent<Connexion>();
    }

    public int QuantiteDisponible(Recoltable_SO recoltable)
    {
        foreach (ComposantRecette comp in composantsDisponibles)
        {
            if (comp.recoltable == recoltable)
                return comp.quantity;
        }
        return 0;
    }

    public void Activate()
    {
        Debug.Log("Activation Imprimante script");
        composantsDisponibles = GetComposantsDisponibles();
    }

    public void Deactivate()
    {
        Debug.Log("Désactivation imprimante");
    }


    public void Craft(Craft_SO recette)
    {
        if (TestRessourcesNecessaires(recette))
        {
            Debug.Log("Assez de ressources, craft");
            SuppressionRessourcesNecessaires(recette.composants);
            Spawn(recette.prefab);
            composantsDisponibles = GetComposantsDisponibles(); //Refresh des quantités de ressources dispo
        }
        else
            Debug.Log("Manque ressources");
    }

    private List<ComposantRecette> GetComposantsDisponibles()
    {
        Dictionary<Recoltable_SO, int> _composantsDispos = new Dictionary<Recoltable_SO, int>();
        List<Coffre> coffresVus = new List<Coffre>();
        foreach (Coffre coffre in CoffresConnected()) //Pour chaque coffre connecté à l'imprimante
        {
            if (!coffresVus.Contains(coffre)) //Si on n'a pas encore parcouru ce coffre
                coffre.GetComposantsDisponibles(_composantsDispos, coffresVus); //On va chercher dans ce coffre
        }

        List<ComposantRecette> result = new List<ComposantRecette>();
        foreach (KeyValuePair<Recoltable_SO, int> pair in _composantsDispos)
        {
            ComposantRecette composant = new ComposantRecette(pair.Key, pair.Value);
            result.Add(composant);
        }

        return result;
    }

    //Test si les composants nécessaires à cette recette se trouvent dans les coffres connectés
    public bool TestRessourcesNecessaires(Craft_SO recette)
    {
        List<ComposantRecette> composantsRecette = recette.composants;
        bool result = true;
        List<ComposantRecette> composantsPossedes = new List<ComposantRecette>(); //Liste des composants correspondants entre la recette et ceux dispos
        int nbComposantTestes = 0;
        foreach (ComposantRecette composantRecette in composantsRecette) //Pour chacun des composants de la recette
        {
            nbComposantTestes++;
            if (result) //Si le test n'a pas encore été invalidé
            {
                foreach (ComposantRecette composantDispo in composantsDisponibles)
                {
                    if (composantDispo.recoltable == composantRecette.recoltable) //Si on a ce composant dans les composants dispos
                        if (composantDispo.quantity >= composantRecette.quantity) //Si on l'a en assez grande quantité
                            composantsPossedes.Add(composantDispo);
                        else
                            result = false;
                }
                if (composantsPossedes.Count < nbComposantTestes) //La boucle précédente n'a pas trouvé le composant voulu, le test est raté
                    result = false;
            }
        }
        return result;
    }


    //Supprime les ressources nécessaires au craft. La variable "quantité" du composant sert à déterminer combien de ressources il reste à supprimer
    private void SuppressionRessourcesNecessaires(List<ComposantRecette> composants)
    {
        foreach (ComposantRecette comp in composants) //Pour chacun des composants de la recette
        {
            ComposantRecette composantASupprimer = new ComposantRecette(comp.recoltable, comp.quantity); //On copie le composant
            List<Coffre> coffresVus = new List<Coffre>();
            foreach (Coffre coffre in CoffresConnected()) //Pour chaque coffre connecté à l'imprimante
            {
                if (!coffresVus.Contains(coffre) && composantASupprimer.quantity > 0) //S'il reste des ressources à supprimer
                    coffre.SuppressionRessources(ref composantASupprimer, coffresVus); //On va chercher dans ce coffre
            }
        }
    }

    private void Spawn(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    private List<Coffre> CoffresConnected()
    {
        List<Coffre> coffres = new List<Coffre>();
        foreach (Connexion co in connexion.connexions)
        {
            Coffre coffre = co.GetComponent<Coffre>();
            if (coffre != null && !coffres.Contains(coffre))
                coffres.Add(coffre);
        }
        return coffres;
    }

    public void TurnON()
    {
        ON_OFF = true;
    }

    public void TurnOFF()
    {
        ON_OFF = false;
    }
}
