using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject player;
    public Planet planet;
    public PropsSpawner objectsInstantiator;
    public GameObject loadingScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        player.SetActive(false);
        loadingScreenCanvas.SetActive(true);
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
        player.SetActive(true);
        loadingScreenCanvas.SetActive(false);
    }
}
