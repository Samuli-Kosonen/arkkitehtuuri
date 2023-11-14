using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Material red;
    public Material normal;
    
    public void ChangeColor()
    {
        gameObject.GetComponent<MeshRenderer>().material = red;
    }
    public void Restart()
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
    }
}
