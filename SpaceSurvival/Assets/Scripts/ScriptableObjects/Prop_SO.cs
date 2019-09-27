using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseProp", menuName = "Props/Base")]
public class Prop_SO : ScriptableObject
{
    public GameObject prefab;
    public float minQuantity;
    public float maxQuantity;
    public int fixQuantity = -1; //-1 pour faire de l'aleatoire. Si une valeur autre est fixé, cette valeur serra utilisé pour le nombre de prop
    public float envergure;
}
