using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    [SerializeField] private bool ON_OFF = false;

    [Space]
    public GameObject brasAllume;
    public GameObject brasEteint;
    public List<Renderer> rendererSpheres;
    public SphereCollider sphereCollider;
    public Dome_Conteneur dome_Conteneur;

    private void Start()
    {
        TurnOFF();
    }

    public void TurnON()
    {
        ON_OFF = true;
        brasAllume.SetActive(true);
        brasEteint.SetActive(false);

        rendererSpheres.ForEach(spheres => spheres.enabled = true);         //On reactive les meshs des spheres pour le rendu
        sphereCollider.enabled = true;                                      //On reactive le collider de la sphere
        dome_Conteneur.Initialize();                                        //Et on dit on contener du dome de s'initialiser pour qu'il recupère du gaz de l'atmosphère
    }

    public void TurnOFF()
    {
        ON_OFF = false;
        brasAllume.SetActive(false);
        brasEteint.SetActive(true);

        rendererSpheres.ForEach(spheres => spheres.enabled = false);        //On desactive le rendu des spheres
        dome_Conteneur.RelacheInAtmosphere();                               //On relache le gaz dans l'atmosphère
        sphereCollider.enabled = false;                                     //On desactive le collider de la sphère
    }
}
