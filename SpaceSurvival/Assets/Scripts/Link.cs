using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject firstGameObject;
    public GameObject secondGameObject;

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        
        RefreshLine();        
    }

    private void Update()
    {
        RefreshLine();
    }

    public void RefreshLine()
    {
        line.SetPosition(0, firstGameObject.transform.position);
        line.SetPosition(1, secondGameObject.transform.position);
    }
}
