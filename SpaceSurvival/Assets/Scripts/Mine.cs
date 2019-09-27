using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utile pour la foreuse pour détecter qu'elle touche une mine et quel type de minerais elle produit.
/// </summary>
public class Mine : MonoBehaviour
{

    public Minerai_SO typeMinerais;
    private int nbMinerais = 6;
    [SerializeField] private List<GameObject> prefabsMinerais;
    [SerializeField] private List<Transform> positionsMinerais;
    private int indexMine;
    private MinesSpawner spawner;

    public void Init(int nbMinerais, int indexMine, MinesSpawner spawner)
    {
        this.nbMinerais = nbMinerais;
        this.indexMine = indexMine;
        this.spawner = spawner;

        //Créer les minerais
        for (int i = 0; i < nbMinerais ; i++)
        {
            Instantiate(prefabsMinerais[i % prefabsMinerais.Count],positionsMinerais[i].position, positionsMinerais[i].rotation, transform).GetComponent<Minerai>().Init(this);
        }

        GameObject positionsParent = positionsMinerais[0].parent.gameObject;
        for (int i = positionsMinerais.Count -1 ; i >= 0; i--)
        {
            Destroy(positionsMinerais[i].gameObject);
            positionsMinerais.RemoveAt(i);
        }
        Destroy(positionsParent);
    }

    public void RetraitMinerai()
    {
        spawner.MineLostMinerai(indexMine);
    }
}
