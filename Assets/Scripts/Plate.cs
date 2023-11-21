using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public Material red;
    public Material normal;
    public int index;
    int maxLifetime = -1;
    int curLifetime;

    public bool hasObj = false;
    public bool colored = false;
    bool initialized = false;
    
    public void ChangeColor()
    {
        gameObject.GetComponent<MeshRenderer>().material = red;
        colored = true;
    }
    public void Restart()
    {
        gameObject.GetComponent<MeshRenderer>().material = normal;
        colored = false;
    }

    public void AddObj(GameObject prefab)
    {
        if (!hasObj)
        {
            Instantiate(prefab, (transform.position + new Vector3(0f, 0.5f, 0f)), transform.rotation);
            hasObj = true;
        }
    }

    public void UpdateGame(int coinAmount)
    {
        if (!initialized)
        {
            maxLifetime = coinAmount;
            curLifetime = coinAmount;
            initialized = true;
        }

        Debug.Log("Game updated");
        if(curLifetime != 0)curLifetime--;

        if (curLifetime <= 0)
        {
            Restart(); 
            initialized= false;
        }
        else if (coinAmount > maxLifetime)
        {
            curLifetime++;
            maxLifetime++;
        }
    }
}
