using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SpawnerMine : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float delay = 120;
    [Range(0, 200)] public float forceExpulsionCailloux = 100;
    public int nbMaxCailloux = 6;

    [SerializeField] private int nbCailloux;
    private float lastTime;
    private Transform _transform;

    void Start()
    {
        _transform = gameObject.transform;
        SpawnCailloux();
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
        if (nbCailloux < nbMaxCailloux)
        {
            GameObject cailloux = GameObject.Instantiate(prefabs[Aleatoire.AleatoireBetween(0, prefabs.Count - 1)]);
            cailloux.transform.position = _transform.position + (Vector3.up * 2);
            cailloux.GetComponent<Rigidbody>().AddForce(Aleatoire.AleatoireVectorDirection() * forceExpulsionCailloux);
            nbCailloux++;
        }
    }

    [ContextMenu("Cailloux--")]
    public void CaillouxRecupere()
    {
        nbCailloux--;
    }
}
