using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PropsSpawner : MonoBehaviour
{
    public List<Prop> props;
    public delegate void PropsPlaced();
    public event PropsPlaced OnPropsPlaced;

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
        print("c'esstpartio");
        while (props.Count > 0)
        {
            _nbPlaced = 0;
            Prop prop = props[0];

            for (int i = 0; i < prop.maxQuantity; i++)
            {
                _spawnPoint.position = _planet.transform.position;
                _spawnPoint.Translate(0, _planet.GetMinMax.Max + 10, 0);
                _spawnPoint.RotateAround(transform.position, Random.onUnitSphere, Aleatoire.AleatoireBetweenFloat(0, 360));

                GameObject obj = Instantiate(prop.prefab, _spawnPoint.position, Quaternion.identity);
                obj.transform.parent = _propsTransform;
                obj.GetComponent<PropsPositionneur>().propsSpawner = this;
            }
            while (_nbPlaced < prop.maxQuantity)
            {
                Debug.Log("pas encore : " + _nbPlaced + " / " + prop.maxQuantity);
                yield return null;
            }
            props.RemoveAt(0);
            Debug.Log("Fini! 100% objet placés");
        }
        Debug.Log("Tous objets placés");
        OnPropsPlaced();
    }

    /// <summary>
    /// Appelé lorsqu'un des Props instanciés touche le sol
    /// </summary>
    public void PropPlaced()
    {
        _nbPlaced++;
    }
}
