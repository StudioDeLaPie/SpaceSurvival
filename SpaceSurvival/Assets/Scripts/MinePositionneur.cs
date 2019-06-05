using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePositionneur : PropsPositionneur
{
    private MinMax minMaxPlanet;
    private Transform _transform;

    private void Start()
    {
        minMaxPlanet = GameObject.FindGameObjectWithTag("Planete").GetComponent<Planet>().GetMinMax;
        _transform = GetComponent<Transform>();
    }

    override
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground" && (Vector3.Distance(Vector3.zero, _transform.position) < minMaxPlanet.Min + 3))
        {
            propsSpawner.PropPlaced();
            Destroy(this);
        }

        else
        {
            propsSpawner.PropPlaced();
            Destroy(gameObject);
        }
    }
}