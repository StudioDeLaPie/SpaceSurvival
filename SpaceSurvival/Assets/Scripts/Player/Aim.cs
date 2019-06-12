using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public LayerMask layersToAim;
    public float range = 100;
    public Inventaire inventaire;
    public OutilDeplacementPortable outilDeplacementPortable;

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
            Recoltable r = _hitInfo.collider.GetComponentInParent<Recoltable>();
            if (r != null)
                inventaire.AimingRecoltable(r);

            Portable p = _hitInfo.transform.root.GetComponentInChildren<Portable>();
            if (p != null)
                outilDeplacementPortable.AimingPortable(p);

            Coffre c = _hitInfo.transform.root.GetComponentInChildren<Coffre>();
            if (c != null)
                inventaire.AimingCoffre(c);
        }
    }

    private bool ProcessAim()
    {
        return Physics.Raycast(_transform.position, _transform.forward, out _hitInfo, range, layersToAim);
    }
}
