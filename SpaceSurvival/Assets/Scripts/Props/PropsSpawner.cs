﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


public class PropsSpawner : MonoBehaviour
{
    public List<Prop_SO> props;
    public delegate void PropsPlaced();
    public event PropsPlaced OnPropsPlaced;

    private Transform _spawnPoint;
    private Planet _planet;
    private MinMax _planetMinMax;
    private LoadingScreen _loadingScreen;
    private Transform _propsTransform;
    private int _nbPlaced;
    private int _nbPropsPerIteration = 500; //Combien de spawns à faire avant de yield dans la coroutine

    // Start is called before the first frame update
    public void GameStart(Planet planet, LoadingScreen loadingScreen)
    {
        _spawnPoint = transform;
        _planet = planet;
        _planetMinMax = _planet.GetMinMax;
        _loadingScreen = loadingScreen;

        _propsTransform = new GameObject("_Props_").transform;

        StartCoroutine(PlaceObjects());
    }

    private IEnumerator PlaceObjects()
    {
        float nbPropsTotal = 0;
        float nbPropsSpawnes = 0;
        props.ForEach(p => nbPropsTotal += p.maxQuantity);

        while (props.Count > 0) //Tant qu'il reste un type de props à placer
        {
            _nbPlaced = 0;
            Prop_SO prop = props[0];
            int nbDone = 0;

            for (int i = 0; i < prop.maxQuantity; i++) //Spawn de la quantité voulue de ce prop
            {
                _spawnPoint.position = _planet.transform.position;
                _spawnPoint.Translate(0, _planet.GetMinMax.Max + 10, 0);
                _spawnPoint.RotateAround(transform.position, Random.onUnitSphere, Aleatoire.AleatoireBetweenFloat(0, 360));

                GameObject obj = Instantiate(prop.prefab, _spawnPoint.position, Quaternion.identity);
                obj.transform.parent = _propsTransform;
                obj.GetComponent<PropsPositionneur>().Placer(this, _planetMinMax);

                nbDone++;
                if (nbDone == _nbPropsPerIteration) //Si X objets ont été spawné
                {
                    nbPropsSpawnes += _nbPropsPerIteration;
                    nbDone = 0;
                    _loadingScreen.PercentPropsPlaced(nbPropsSpawnes / nbPropsTotal);
                    yield return null; //on laisse Unity passer à la frame suivante
                }
            }

            nbPropsSpawnes += nbDone;
            _loadingScreen.PercentPropsPlaced(nbPropsSpawnes / nbPropsTotal);

            props.Remove(prop); //Ce type de prop est placé, on le supprime de la liste
        }
        OnPropsPlaced();//On averti le GameStarter que tous les props sont placés
    }
}
