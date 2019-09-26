using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsPositionneur : MonoBehaviour
{
    //public Prop_SO prop;
    public LayerMask layerMask;
    public List<GameObject> listObjectsToActivateWhenPlaced;

    private Transform _transform;



    /// <summary>
    /// Tente de placer le prop et renvoi true si réussit
    /// #OPTI (Destroy la liste des enfants?)
    /// </summary>
    /// <param name="propsSpawner"></param>
    /// <param name="minMaxPlanet"></param>
    /// <returns></returns>
    public bool Placer(PropsSpawner propsSpawner, MinMax minMaxPlanet)
    {
        _transform = GetComponent<Transform>();

        RaycastHit hit;
        if (Physics.Raycast(_transform.position, (Vector3.zero - _transform.position).normalized, out hit, Vector3.Distance(_transform.position, Vector3.zero), layerMask))
        {
            if (hit.collider.tag == "Ground" && (Vector3.Distance(Vector3.zero, hit.point) < minMaxPlanet.Min + 3)) //Si ce qu'on a touché est le sol et que ca se touve à moins de 3 mètres au dessus de la surface
            {
                //Place sur le sol
                _transform.position = hit.point;

                //Oriente
                _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.FromToRotation(_transform.up, (_transform.position - Vector3.zero).normalized) * _transform.rotation, 50 * Time.deltaTime);

                listObjectsToActivateWhenPlaced.ForEach(go => go.SetActive(true)); //Réactivation des enfants du prop
                gameObject.isStatic = true;
                Destroy(this, 0.1f);
                return true;
            }
            else
            {
                Destroy(gameObject, 0.1f);
            }
        }
        else
            Destroy(gameObject, 0.1f);
        return false;
    }
}
