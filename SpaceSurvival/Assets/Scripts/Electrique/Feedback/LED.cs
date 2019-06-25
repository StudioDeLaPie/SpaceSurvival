using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FeedbackEnginElec;

public class LED : MonoBehaviour
{
    [SerializeField] private MeshRenderer _rendererOnOff;
    [SerializeField] private MeshRenderer _rendererPanne;
    [SerializeField] private Material on;
    [SerializeField] private Material off;
    [SerializeField] private Material panneAllumee;
    [SerializeField] private Material panneEteinte;

    private EtatEngin _etat;

    public void SetEtat(EtatEngin etat)
    {
        switch (etat)
        {
            case EtatEngin.onOK:
                _rendererOnOff.sharedMaterial = on;
                _rendererPanne.sharedMaterial = panneEteinte;
                break;
            case EtatEngin.offOK:
                _rendererOnOff.sharedMaterial = off;
                _rendererPanne.sharedMaterial = panneEteinte;
                break;
            case EtatEngin.onPanne:
                _rendererOnOff.sharedMaterial = on;
                _rendererPanne.sharedMaterial = panneAllumee;
                break;
            case EtatEngin.offPanne:
                _rendererOnOff.sharedMaterial = off;
                _rendererPanne.sharedMaterial = panneAllumee;
                break;
        }
    }
}
