using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SpawnerMinerais : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float delay = 120;
    [Range(0, 200)] public float forceExpulsionCailloux = 100;
    public int nbMaxMinerais = 6;

    [SerializeField] private int nbMinerais;
    private float lastTime;
    private Transform _transform;

    void Start()
    {
        lastTime = Time.time;
        _transform = gameObject.transform;
    }

    void Update()
    {
        if (Time.time > lastTime + delay)
        {
            lastTime = Time.time;
            SpawnCailloux();
        }
    }

    private void SpawnCailloux()
    {
        if (nbMinerais < nbMaxMinerais)
        {
            GameObject cailloux = GameObject.Instantiate(prefabs[Aleatoire.AleatoireBetween(0, prefabs.Count - 1)]);
            cailloux.transform.position = _transform.position + (Vector3.up * 2);
            cailloux.GetComponent<Rigidbody>().AddForce(Aleatoire.AleatoireVectorDirection() * forceExpulsionCailloux);
            nbMinerais++;
        }
    }

    [ContextMenu("Minerais--")]
    public void MineraisRecupere()
    {
        nbMinerais--;
    }
}
