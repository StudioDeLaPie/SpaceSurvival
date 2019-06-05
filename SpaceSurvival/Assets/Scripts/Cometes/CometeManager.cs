using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class CometeManager : MonoBehaviour
{
    public GameObject cometePrefab;
    public int maxCometes;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    public float hauteurSpawnComete = 500;

    private int _nbCometes = 0;
    private float _lastSpawn;
    private float _delay;
    private Transform _spawnPoint;
    private Planet _planet;

    void Start()
    {
        _lastSpawn = Time.time;
        _delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        _planet = GameObject.FindGameObjectWithTag("Planete").GetComponent<Planet>();

        //Instantie le spawn point
        GameObject obj = new GameObject();
        obj.transform.SetParent(transform);
        _spawnPoint = obj.transform;
    }

    void Update()
    {
        if (_nbCometes < maxCometes && Time.time > _lastSpawn + _delay)
        {
            SpawnComete();
        }
    }


    /// <summary>
    /// Fait apparaitre une comète à une position aléatoire autour de la planète
    /// </summary>
    private void SpawnComete()
    {
        _spawnPoint.position = _planet.transform.position;
        _spawnPoint.Translate(0, _planet.shapeSettings.planetRadius + hauteurSpawnComete, 0);
        _spawnPoint.RotateAround(transform.position, Random.onUnitSphere, Aleatoire.AleatoireBetweenFloat(0, 360));
        GameObject comete = Instantiate(cometePrefab, _spawnPoint.position, Quaternion.identity);

        comete.GetComponent<Comete>().OnCometeDestroyed += CometeDestroyed;

        _nbCometes++;
        _lastSpawn = Time.time;
        _delay = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void CometeDestroyed()
    {
        _nbCometes--;
    }
}
