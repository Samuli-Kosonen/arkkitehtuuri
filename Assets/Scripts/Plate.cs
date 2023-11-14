using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Material red;
    public Material normal;

    public bool hasObj = false;
    
    public void ChangeColor()
    {
        gameObject.GetComponent<MeshRenderer>().material = red;
    }
    public void Restart()
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }

    public void AddObj(GameObject prefab)
    {
        if (!hasObj)
        {
            Instantiate(prefab, (transform.position + new Vector3(0f, 0.5f, 0f)), transform.rotation);
            hasObj = true;
        }
    }
}
