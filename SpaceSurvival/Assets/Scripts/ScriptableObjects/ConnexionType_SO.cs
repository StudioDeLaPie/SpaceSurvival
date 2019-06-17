using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeConnexion", menuName = "ConnexionType")]
public class ConnexionType_SO : ScriptableObject
{
    public string nom;
    public List<ConnexionType_SO> connexionsTypes;

    /// <summary>
    /// Retourne si cette connection est possible avec un objet qui a ce type de connexion
    /// </summary>
    /// <param name="typeConnexion"></param>
    /// <returns></returns>
    public bool CanBeConnectWith(ConnexionType_SO typeConnexion)
    {
        return connexionsTypes.Contains(typeConnexion); 
    }
}
