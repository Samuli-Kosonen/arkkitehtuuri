using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlateManager : MonoBehaviour
{

    List<GameObject> plates = new List<GameObject>();
    List<Plate> colorPlates = new List<Plate>();
    public GameObject plate;
    public GameObject coin;
    public GameObject enemy;
    public GameObject playerPrefab;
    public float maxCoinTimer = 3f;
    public float maxEnemyTimer = 5f;
    private float coinTimer;
    private float enemyTimer;
    bool game=false;
    int index = 0;
    float timer = 2f;

    int height = 25;
    int width = 25;


    // Start is called before the first frame update
    void Start()
    {
        coinTimer = maxCoinTimer;
        enemyTimer = maxEnemyTimer;

        for (int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                // Insantiate plate and get reference
                GameObject newPlate = Instantiate(plate);
                Plate Plate = newPlate.GetComponent<Plate>();
                Plate.x = i;
                Plate.y = j -1;

                if (Plate.y == -1)
                {
                    Plate.y = 24;
                    Plate.x -= 1;
                }

                // Set plates index
                Plate.index = index;

                // Store raise index
                index++;

                // Set plate on correct position
                plate.transform.position = new Vector3(-24 + i + i, -2.5f, -24 + j +j);

                // Set plate as a child of this manager
                newPlate.transform.parent = gameObject.transform;

                // Add it to the list of plates
                plates.Add(newPlate);
            }
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Instantiate(playerPrefab);
            game = true;
        }

        if (game)
        {
            coinTimer -= Time.deltaTime;
            enemyTimer -= Time.deltaTime;
        }

        if (coinTimer <= 0)
        {
            int random = Random.Range(1, plates.Count);
            if (random == 313 || plates[random].GetComponent<Plate>().colored) return;

            plates[random].GetComponent<Plate>().AddObj(coin);
            coinTimer = maxCoinTimer;
        }
        if (enemyTimer <= 0)
        {

            int random = Random.Range(1, plates.Count);
            if (random == 313 || plates[random].GetComponent<Plate>().colored) return;

            plates[random].GetComponent<Plate>().AddObj(enemy);
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
