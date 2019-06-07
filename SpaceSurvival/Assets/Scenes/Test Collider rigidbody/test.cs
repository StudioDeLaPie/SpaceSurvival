using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name + "   racine -> " + other.transform.root.name);
        if (other.transform.root != transform.root) //pas un objet de ma hiérarchie
        {
            Debug.LogWarning("Colision avec pas hierarchie" + other.name);

        }
        else
        {
            Debug.Log("Collision hierarchie");
        }
    }
}
