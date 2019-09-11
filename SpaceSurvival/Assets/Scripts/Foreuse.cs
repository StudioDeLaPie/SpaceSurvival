using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreuse : MonoBehaviour, I_Elec
{
    //[ShowOnly] public bool haveMine = false; //true si la foreuse est au dessus d'une mine

    public DetecteurForeuse detecteur;
        public GameObject prefabMinerais;
        public Connexion connexion;

    public ForeuseElec foreuseElec;

    new public Animation animation;

    private Coffre coffre; //Un des coffres connecté à la foreuse

    public void TurnOff()
    {
        animation.Stop();
        CancelInvoke("Minage");
    }

    public void TurnOn()
    {
        animation.Play();
        InvokeRepeating("Minage", 3f, 3f);
    }

    /*private void SpawnMinerais() à Supprimer après les tests. Si tu lis ce message c'est.... que j'ai oublié.... sorry
    {
        prefabMinerais = detecteur.prefabMinerais;

        if (prefabMinerais != null)
        {
            GameObject mineraisTemp = Instantiate(prefabMinerais);
            mineraisTemp.transform.position = gameObject.transform.position + Vector3.forward;
            mineraisTemp.GetComponent<Rigidbody>().AddForce(new Vector3(2f, 5f, 2f));
        }
    }*/

    /// <summary>
    /// Appellé en boucle pour "MINER" un minerais et le placer dans les coffres
    /// La Foreuse s'arrête si il n'y a plus de place dans les coffres
    /// </summary>
    private void Minage()
    {
        if (CoffreConnected())
        {
            Debug.Log("Minage");
            prefabMinerais = detecteur.prefabMinerais;

            GameObject temp = Instantiate(prefabMinerais);

            if (!coffre.AjouterItem(temp.GetComponent<Recoltable>().Recolte()))
            {
                foreuseElec.SwitchON_OFF();
                Destroy(temp);
            }
        }
        else
        {
            Debug.Log("AucunCoffre branché");
            foreuseElec.SwitchON_OFF();

        }
    }

    /// <summary>
    /// Renvoie true si au moins un coffre est connecté
    /// </summary>
    /// <returns></returns>
    private bool CoffreConnected()
    {
        foreach (Connexion co in connexion.connexions)
        {
            if (co.GetComponent<Coffre>())
            {
                coffre = co.GetComponent<Coffre>();
                return true;
            }
        }
        return false;
    }
}
