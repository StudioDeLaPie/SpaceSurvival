using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    Planet, SpawnMines, SpawnProps, SpawnVaisseau
}

public class SeedManager : MonoBehaviour
{
    [SerializeField] private int seedMonde = 0;
    [SerializeField] private int[] seeds = null; 

    /// <summary>
    /// Renvoie la seed Monde. Cette seed ne change jamais dans la même sauvegarde. Il faut supprimer la save pour la changer.
    /// </summary>
    /// <returns></returns>
    public int GetSeedMonde()
    {
        if (seedMonde == 0)
        {
            seedMonde = ES3.Load<int>("SeedMonde", 0);
            if (seedMonde == 0)
            {
                seedMonde = Utilities.Aleatoire.AleatoireBetween(1, 255) * Utilities.Aleatoire.AleatoireBetween(1, 255) * Utilities.Aleatoire.AleatoireBetween(1, 255); //Génération du seed
                ES3.Save<int>("SeedMonde", seedMonde);
            }
        }
        return seedMonde;
    }


    /// <summary>
    /// Renvoie un autre type de seed comme par exemple de la planète ou celle du spawn des mines.
    /// </summary>
    /// <param name="seedType"></param>
    /// <returns></returns>
    public int GetOtherSeed(SeedType seedType)
    {
        if (seeds.Length == 0)
        {
            GenerateSeeds();
        }
        return seeds[(int)seedType];
    }

    /// <summary>
    /// Génère les OtherSeeds.
    /// </summary>
    private void GenerateSeeds()
    {
        System.Random rand = new System.Random(GetSeedMonde());
        int nbTypes = System.Enum.GetNames(typeof(SeedType)).Length;
        seeds = new int[nbTypes];
        for (int i = 0; i < nbTypes; i++)
        {
            seeds[i] = rand.Next();
        }
    }
}
