using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comete : MonoBehaviour
{
    public GameObject glacePrefab;
    [Space]
    [SerializeField] private float quantiteEauEvaporable; //Si 50.000
    [SerializeField] private float quantiteEvapParUpdate; //il faut une évap de 1.666667 pour durer 10 minutes
    [SerializeField] private float nbGlaconsDansComete;

    private Conteneur atmosphere;
    private bool fonteEnCours = false;
    private float eauEvaporableRestante;

    private void Awake()
    {
        atmosphere = GameObject.FindGameObjectWithTag("Atmosphere").GetComponent<Conteneur>();
        eauEvaporableRestante = quantiteEauEvaporable;
    }

    private void FixedUpdate()
    {
        if (fonteEnCours)
        {
            atmosphere.AddGas(EGases.Vapeur, quantiteEvapParUpdate);
            eauEvaporableRestante -= quantiteEvapParUpdate;
            if (eauEvaporableRestante <= 0)
                Destroy(gameObject);
        }
    }

    public void MinerComete(Vector3 spawnPos)
    {
        Instantiate(glacePrefab, spawnPos, Quaternion.identity);//transform.position + (hitPos - transform.position) * 0.8f, Quaternion.identity);
        eauEvaporableRestante -= quantiteEauEvaporable / nbGlaconsDansComete;
    }

    public void CommencerFonte()
    {
        fonteEnCours = true;
    }
}
