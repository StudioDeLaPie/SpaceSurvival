using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour, I_Elec
{

    [Space]
    public GameObject brasAllume;
    public GameObject brasEteint;
    public List<Renderer> rendererSpheres;
    public SphereCollider sphereCollider;
    public Dome_Conteneur dome_Conteneur;

    private void Start()
    {
        brasAllume.SetActive(false);
        brasEteint.SetActive(true);

        rendererSpheres.ForEach(spheres => spheres.enabled = false);        //On desactive le rendu des spheres
    }

    public void TurnOn()
    {
        brasAllume.SetActive(true);
        brasEteint.SetActive(false);

        rendererSpheres.ForEach(spheres => spheres.enabled = true);         //On reactive les meshs des spheres pour le rendu
        dome_Conteneur.TurnOnDomeConteneur();                               //Et on dit on contener du dome de s'allumer pour qu'il recupère du gaz de l'atmosphère

    }

    public void TurnOff()
    {
        brasAllume.SetActive(false);
        brasEteint.SetActive(true);

        rendererSpheres.ForEach(spheres => spheres.enabled = false);        //On desactive le rendu des spheres
        dome_Conteneur.RelacheInAtmosphere();                               //On relache le gaz dans l'atmosphère

    }
}
