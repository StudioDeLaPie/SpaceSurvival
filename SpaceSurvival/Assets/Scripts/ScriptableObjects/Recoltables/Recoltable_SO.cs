using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "BaseRecoltable", menuName = "Recoltable/Base")]
public class Recoltable_SO : ScriptableObject
{
    public List<GameObject> prefabs;
    public Sprite sprite;
    public string nom;
    [TextArea]
    public string description;

    public GameObject GetPrefab()
    {
        if (prefabs.Count > 0)
        {
            return prefabs[Aleatoire.AleatoireBetween(0, prefabs.Count)];
        }
        else
        {
            Debug.LogError("Aucun prefab attribué dans le minerai " + nom);
            return null;
        }
    }
}
