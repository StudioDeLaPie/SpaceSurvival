using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public Planet planet;
    public PropsSpawner objectsInstantiator;

    // Start is called before the first frame update
    void Start()
    {
        planet.OnPlanetGenerationEnded += PlanetGenerationEnded;
        planet.GameStart();
    }

    private void PlanetGenerationEnded()
    {
        Debug.Log("Planet générée");
        objectsInstantiator.OnPropsPlaced += PropsPlaced;
        objectsInstantiator.GameStart();
    }

    private void PropsPlaced()
    {
        Debug.Log("Props placés");
    }
}
