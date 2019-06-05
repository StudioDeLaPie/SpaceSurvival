using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SpawnerMinerais : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int minDelay = 80;
    public int maxDelay = 120;
    [Range(0, 200)] public float forceExpulsionCailloux = 100;
    public int nbMaxMinerais = 6;

    [SerializeField] private int nbMinerais;
    private float lastTime;
    private Transform _transform;
    private float _delay;

    void Start()
    {
        lastTime = Time.time;
        _transform = gameObject.transform;
        RandomDelay();
    }

    void Update()
    {
        if (Time.time > lastTime + _delay)
        {
            SpawnMinerai();
            lastTime = Time.time;
            RandomDelay();
        }
    }

    private void SpawnMinerai()
    {
        if (nbMinerais < nbMaxMinerais)
        {
            GameObject minerai = GameObject.Instantiate(prefabs[Aleatoire.AleatoireBetween(0, prefabs.Count - 1)]);
            minerai.transform.position = _transform.position + (_transform.up * 2);
            minerai.GetComponent<Rigidbody>().AddForce(Aleatoire.AleatoireVectorDirection() * forceExpulsionCailloux);
            minerai.GetComponent<Minerai>().Mine = this;
            nbMinerais++;
        }
    }

    [ContextMenu("Random delay")]
    private void RandomDelay()
    {
        _delay = Aleatoire.AleatoireBetween(minDelay, maxDelay);
    }

    public void MineraiRecolte()
    {
        nbMinerais--;
    }
}
