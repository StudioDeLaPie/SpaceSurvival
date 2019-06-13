using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject firstGameObject;
    public GameObject secondGameObject;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        RefreshLine();        
    }

    private void Update()
    {
        RefreshLine();
    }

    public void RefreshLine()
    {
        Vector3 _pos1 = firstGameObject.transform.position;
        Vector3 _pos2 = secondGameObject.transform.position;
        _transform.position = new Vector3(((_pos1.x + _pos2.x) / 2), ((_pos1.y + _pos2.y) / 2), ((_pos1.z + _pos2.z) / 2));
        _transform.LookAt(firstGameObject.transform);
        _transform.localScale = new Vector3 (1,  1 , ((Vector3.Distance(_pos1, _pos2))));
    }
}
