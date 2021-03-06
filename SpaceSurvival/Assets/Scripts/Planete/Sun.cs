﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    Vector3 planetePos;
    Transform transformSun;

    [Range(0.1f,360)]
    public float degreParSeconde = 5;
    [Range(0,5000)]
    public float amplitudeOscillation = 100;

    [Range(0, 1)]
    public float vitesseOscillation = 1;

    private void Start()
    {
        transformSun = transform;
        planetePos = GameObject.FindGameObjectWithTag("Planete").transform.position;    
    }

    //Pour changer d'axe set l'axe voulue dans le transformSun.position et mettre l'axe voulu dans le Vecteur 3 du roteAround
    void Update()
    {
        Vector3 direction = (transformSun.position - planetePos).normalized;
        transformSun.position = new Vector3(Mathf.Sin(Time.time * vitesseOscillation) * amplitudeOscillation, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
        //Tourne l'objet autour de la planète à X Degré/Secondes
        transformSun.RotateAround(planetePos, new Vector3(1,0,0), degreParSeconde * Time.deltaTime); //new Vector3(Mathf.Abs(Mathf.Sin(Time.time)),0,0)
        transformSun.LookAt(planetePos);
    }
}
