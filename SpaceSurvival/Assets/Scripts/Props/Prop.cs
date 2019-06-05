using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "Props")]
public class Prop : ScriptableObject
{
    public GameObject prefab;
    public int maxQuantity;
    public float envergure;
}
