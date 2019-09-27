using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject player;
    public Planet planet;
    public MinesSpawner objectsInstantiator;
    public LoadingScreen loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Debut du spawnage");
        StartCoroutine(GameStart());
    }

    private void PlanetGenerationEnded()
    {
        Debug.Log("Planet générée");
        objectsInstantiator.OnPropsPlaced += PropsPlaced;
        objectsInstantiator.GameStart(planet, loadingScreen);
    }

    private void PropsPlaced()
    {
        Debug.Log("Props placés");
        player.SetActive(true);
        loadingScreen.Hide();
    }

    private IEnumerator GameStart()
    {
        player.SetActive(false);
        loadingScreen.Show();
        yield return new WaitForSeconds(1);
        planet.OnPlanetGenerationEnded += PlanetGenerationEnded;
        planet.GameStart();
    }
}
