using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    List<GameObject> plates = new List<GameObject>();
    public GameObject plate;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 25; i++)
        {
            for(int j = 0; j < 25; j++)
            {
                GameObject newPlate = Instantiate(plate);
                plate.transform.position = new Vector3(-24 + i + i, -2.5f, -24 + j +j);
                newPlate.transform.parent = gameObject.transform;
                plates.Add(newPlate);
            }
        }
    }
    
    public void Restart()
    {
        foreach (GameObject plate in plates)
        {
            plate.GetComponent<Plate>().Restart();
        }
    }
}
