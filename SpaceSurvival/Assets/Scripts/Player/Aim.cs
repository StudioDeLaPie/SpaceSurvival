using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public LayerMask layersToAim;
    public float range = 100;
    public Inventaire inventaire;
    public OutilDeplacementPortable outilDeplacementPortable;
    public UIManager uiManager;
    public UIPlayer uiPlayer;

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
            {
                inventaire.AimingRecoltable(r);
                uiPlayer.FeedbackAim("[Recolte] pour ramasser " + r.data.nom);
            }
            else
            {
                CarManager car = _hitInfo.collider.GetComponentInParent<CarManager>();
                if (car != null)
                {
                    uiPlayer.FeedbackAim("[Action] Conduire");
                    if (Input.GetButtonDown("Action"))
                        car.GetInCar(transform.root.gameObject);
                }

                Comete comete = _hitInfo.collider.GetComponentInParent<Comete>();
                if (comete != null)
                {
                    uiPlayer.FeedbackAim("[Clic gauche] Miner");
                    if (Input.GetButtonDown("MouseLeft"))
                        comete.MinerComete(_transform.position + (_hitInfo.point - _transform.position) * 0.5f);
                }

                Portable p = _hitInfo.transform.root.GetComponentInChildren<Portable>();
                if (p != null)
                {
                    outilDeplacementPortable.AimingPortable(p);
                    uiPlayer.FeedbackAim("[Action] pendant 1s pour déplacer");
                }

                Coffre c = _hitInfo.transform.root.GetComponentInChildren<Coffre>();
                if (c != null)
                {
                    uiManager.AimingCoffre(c);
                    uiPlayer.FeedbackAim("[Action] pour ouvrir le coffre", 2);
                }

                Imprimante i = _hitInfo.transform.root.GetComponentInChildren<Imprimante>();
                if (i != null)
                {
                    if (i.fonctionnel)
                    {
                        uiManager.AimingImprimante(i);
                        uiPlayer.FeedbackAim("[Action] pour activer l'imprimante", 2);
                    }
                    else
                        uiPlayer.FeedbackAim("Imprimante non alimentée");
                }

                EnginElec e = _hitInfo.transform.root.GetComponentInChildren<EnginElec>();
                if (e != null)
                {
                    uiPlayer.FeedbackAim("[SwitchEnginElectrique] pour allumer ou éteindre la machine", 2);
                    if (Input.GetButtonDown("SwitchEnginElectrique"))
                    {
                        e.SwitchON_OFF();
                    }
                }
            }
        }
    }

    private bool ProcessAim()
    {
        return Physics.Raycast(_transform.position, _transform.forward, out _hitInfo, range, layersToAim);
    }
}
