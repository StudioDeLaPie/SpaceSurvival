using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float vitesse;
    public Transform cible;

    void Update()
    {
        transform.RotateAround(cible.position, new Vector3(0,1,0), vitesse * Time.deltaTime);
        transform.LookAt(cible.transform);
    }
}
