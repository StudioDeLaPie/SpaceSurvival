using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseProp", menuName = "Props/Base")]
public class Prop_SO : ScriptableObject
{
    public GameObject prefab;
    public int maxQuantity;
    public float envergure;
}
