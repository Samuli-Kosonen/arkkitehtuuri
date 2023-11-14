using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    List<GameObject> plates = new List<GameObject>();
    public GameObject plate;
    public GameObject coin;
    public GameObject enemy;
    public float maxCoinTimer = 3f;
    public float maxEnemyTimer = 5f;
    private float coinTimer;
    private float enemyTimer;
    // Start is called before the first frame update
    void Start()
    {
        coinTimer = maxCoinTimer;
        enemyTimer = maxEnemyTimer;

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

    private void Update()
    {
        coinTimer -= Time.deltaTime;
        enemyTimer -= Time.deltaTime;

        if(coinTimer <= 0)
        {
            plates[Random.Range(1, plates.Count)].GetComponent<Plate>().AddObj(coin);
            coinTimer = maxCoinTimer;
        }
        if(enemyTimer <= 0)
        {
            plates[Random.Range(1, plates.Count)].GetComponent<Plate>().AddObj(enemy);
            enemyTimer = maxEnemyTimer;
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
