using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public float range = 100;
    public Inventaire inventaire;

    private RaycastHit _hitInfo;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _hitInfo = new RaycastHit();
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ProcessAim())
        {
            Recoltable r = _hitInfo.collider.GetComponent<Recoltable>();
            if (r != null)
                inventaire.AimingRecoltable(r);
        }
    }

    private bool ProcessAim()
    {
        return Physics.Raycast(_transform.position, _transform.forward, out _hitInfo, range);
    }
}
