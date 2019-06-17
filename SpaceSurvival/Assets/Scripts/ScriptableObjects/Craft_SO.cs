using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recette", menuName = "Craft")]
public class Craft_SO : ScriptableObject
{
    [System.Serializable]
    public class ComposantRecette
    {
        public Recoltable_SO recoltable;
        public int quantity;
        public ComposantRecette(Recoltable_SO recoltable, int quantity)
        {
            this.recoltable = recoltable;
            this.quantity = quantity;
        }
    }

    public GameObject prefab;
    public Sprite sprite;
    public string nom;
    [TextArea] public string description;
    public List<ComposantRecette> composants = new List<ComposantRecette>();
}
