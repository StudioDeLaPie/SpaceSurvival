using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PropsSpawner : MonoBehaviour
{
    public List<Prop> props;
    public delegate void PropsInstantiated();
    public event PropsInstantiated OnPropsInstantiated;

    private Transform _spawnPoint;
    private Planet _planet;
    private Transform _propsTransform;
    private int _nbPlaced;

    // Start is called before the first frame update
    public void GameStart()
    {
        _spawnPoint = transform;
        _planet = GameObject.FindGameObjectWithTag("Planete").GetComponent<Planet>();

        _propsTransform = new GameObject("_Props_").transform;

        StartCoroutine(PlaceObjects());
    }

    private IEnumerator PlaceObjects()
    {
        while (props.Count > 0) //Tant qu'il reste un type de props à placer
        {
            _nbPlaced = 0;
            Prop prop = props[0];

            for (int i = 0; i < prop.maxQuantity; i++) //Spawn de la quantité voulue de ce prop
            {
                _spawnPoint.position = _planet.transform.position;
                _spawnPoint.Translate(0, _planet.GetMinMax.Max + 10, 0);
                _spawnPoint.RotateAround(transform.position, Random.onUnitSphere, Aleatoire.AleatoireBetweenFloat(0, 360));

                GameObject obj = Instantiate(prop.prefab, _spawnPoint.position, Quaternion.identity);
                obj.transform.parent = _propsTransform;
                obj.GetComponent<PropsPositionneur>().OnPropPlaced += PropPlaced;
            }
            while (_nbPlaced < prop.maxQuantity) //Tant qu'ils n'ont pas tous fini de se placer
            {
                yield return null; //on attend
            }
            props.RemoveAt(0); //Ce type de prop est placé, on le supprime de la liste
        }
        OnPropsInstantiated(); //On averti le GameStarter que tous les props sont placés
    }

    /// <summary>
    /// Appelé lorsqu'un des Props instanciés touche le sol
    /// </summary>
    private void PropPlaced()
    {
        _nbPlaced++;
    }
}
