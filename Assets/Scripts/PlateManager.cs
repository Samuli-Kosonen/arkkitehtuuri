using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    List<GameObject> plates = new List<GameObject>();
    List<Plate> colorPlates = new List<Plate>();
    public GameObject plate;
    public GameObject coin;
    public GameObject enemy;
    public float maxCoinTimer = 3f;
    public float maxEnemyTimer = 5f;
    private float coinTimer;
    private float enemyTimer;
    int index = 0;
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
                newPlate.GetComponent<Plate>().index = index;
                index++;
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
            int random = Random.Range(1, plates.Count);
            if(random != 313)plates[random].GetComponent<Plate>().AddObj(coin);
            coinTimer = maxCoinTimer;
        }
        if(enemyTimer <= 0)
        {
            Plate curPlate;
            int random = Random.Range(1, plates.Count);
            if (random != 313) curPlate = plates[random].GetComponent<Plate>();
            else curPlate = plates[0].gameObject.GetComponent<Plate>();
            if (!curPlate.colored) curPlate.AddObj(enemy);
            enemyTimer = maxEnemyTimer;
        }
    }

    public void UpdateGame(int coinAmount)
    {
        //colorPlates.Clear();

        foreach (GameObject plate in plates)
        {
            Plate curPlate = plate.gameObject.GetComponent<Plate>();

            if (curPlate != null && curPlate.colored) curPlate.UpdateGame(coinAmount);
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
