using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using SRandom = System.Random;

public class MinesSpawner : MonoBehaviour
{
    public List<Prop_SO> mines;
    public List<int> fixQuantities = new List<int>(); //Quantitié de chaque mine
    public delegate void PropsPlaced();
    public event PropsPlaced OnPropsPlaced;
    public LayerMask layerMask;

    private MinMax _planetMinMax;
    private LoadingScreen _loadingScreen;
    private Transform _parentPropsTransform;
    private int _nbPlaced;
    private int _nbMinesPerIteration = 500; //Combien de spawns à faire avant de yield dans la coroutine

    private List<int> mineraisParMine; //#OPTI (Changer en dictionnaire<int indexMine, int nbMinerais> et stocker que ce qui a été modifié
    private SRandom rand;
    private int seedMine;

    // Start is called before the first frame update
    public void GameStart(Planet planet, LoadingScreen loadingScreen)
    {
        _planetMinMax = planet.GetMinMax;
        _loadingScreen = loadingScreen;

        _parentPropsTransform = GameObject.Find("_Props_").transform;

        InitSeedVariables();
        

        //if (_propsTransform.childCount == 0) //S'il n'y a pas encore d'enfants (s'il y en a, c'est que la partie a déjà été lancée puis sauvegardée)
        if (_parentPropsTransform.GetComponentsInChildren<Transform>().Length == 1) //Donc pas d'enfants
        {
            StartCoroutine(PlaceObjects());
        }
        else
        {
            OnPropsPlaced(); //On fait rien et prévient le manager qu'on a finit
        }
    }

    /// <summary>
    /// Demande au SeedManager le seed généré pour les mines
    /// Et génère la quantité de chaque mine
    /// </summary>
    /// <returns></returns>
    private void InitSeedVariables()
    {
        seedMine = GameObject.Find("SeedManager").GetComponent<SeedManager>().GetOtherSeed(SeedType.SpawnMines);
        rand = new SRandom(seedMine);

        //On set la quantité à faire spawner
        foreach (Prop_SO mine in mines)
        {
            if (mine.fixQuantity == -1) //Si la quantité fix n'est pas définit
            {
                fixQuantities.Insert(mines.IndexOf(mine), (int)(((float)rand.NextDouble() * (mine.maxQuantity - mine.minQuantity)) + mine.minQuantity)); //On lui attribut une quantité aléatoire
            }
            else
            {
                rand.NextDouble(); //On fait passer cette valeur de rand
                fixQuantities.Insert(mines.IndexOf(mine), mine.fixQuantity);
            }
        }
    }

    private IEnumerator PlaceObjects()
    {
        int nbMineSpawnes = 0;

        int nbMineTotal = 0;
        mines.ForEach(p => nbMineTotal += fixQuantities[mines.IndexOf(p)]);

        Debug.Log(nbMineTotal);

        mineraisParMine = ES3.Load<List<int>>("MineraisParMine", new List<int>());
        if (mineraisParMine.Count == 0) //Si pas encore sauvegardé
        {
            for (int i = 0; i < nbMineTotal; i++)
            {
                mineraisParMine.Add(6);
            }
        }

        foreach (Prop_SO mine in mines) //pour chaque type de mine
        {
            int nbMinesAvantPauseSpawn = _nbMinesPerIteration;
            int counter = 0;

            while (counter < fixQuantities[mines.IndexOf(mine)]) //Spawn de la quantité voulue de cette mine
            {
                Vector3 randomPosition = RandomDirection() * (_planetMinMax.Max + 10);

                RaycastHit hit;

                if (TestPlacement(randomPosition, out hit))
                {
                    PlacementMine(mine, hit.point, mineraisParMine[nbMineSpawnes], nbMineSpawnes);

                    counter++;
                    nbMinesAvantPauseSpawn--;
                    nbMineSpawnes++;

                    if (nbMinesAvantPauseSpawn == 0) //Si on a spawné _nbPropsPerIteration
                    {
                        nbMinesAvantPauseSpawn = _nbMinesPerIteration;
                        _loadingScreen.PercentPropsPlaced(nbMineSpawnes / (float)nbMineTotal);
                        yield return null; //on laisse Unity passer à la frame suivante
                    }
                }
            }
        }

        OnPropsPlaced();//On averti le GameStarter que tous les props sont placés
    }

    /// <summary>
    /// Vector3 aléatoire et normalisé.
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomDirection()
    {
        Vector3 result = new Vector3((float)(rand.NextDouble() * 2) - 1, (float)(rand.NextDouble() * 2) - 1, (float)(rand.NextDouble() * 2) - 1);
        return result.normalized;
    }

    private bool TestPlacement(Vector3 startPoint, out RaycastHit hit)
    {
        if (Physics.Raycast(startPoint, -startPoint, out hit, startPoint.magnitude, layerMask))
        {
            if (hit.collider.tag == "Ground" && hit.point.magnitude < _planetMinMax.Min + 3) //Si ce qu'on a touché est le sol et que ca se touve à moins de 3 mètres au dessus de la surface
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    private void PlacementMine(Prop_SO mine, Vector3 pos, int nbMinerais, int indexMine)
    {
        GameObject obj = Instantiate(mine.prefab, pos, Quaternion.identity);

        obj.transform.up = pos; //Oriente

        obj.transform.parent = _parentPropsTransform;

        obj.GetComponent<Mine>().Init(nbMinerais, indexMine, this);

        gameObject.isStatic = true;
    }

    /// <summary>
    /// Appelé par une mine quand elle perd un minerais
    /// </summary>
    public void MineLostMinerai(int indexMine)
    {
        mineraisParMine[indexMine]--;
        ES3.Save<List<int>>("MineraisParMine", mineraisParMine);
    }
}
