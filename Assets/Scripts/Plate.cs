using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plate : MonoBehaviour
{
    PlateManager pm;

    public Material red;
    public Material normal;
    public int index;
    public int x;
    public int y;
    public int aliveN;
    int maxLifetime = -1;
    int curLifetime;


    public bool hasObj = false;
    public bool colored = false;
    bool initialized = false;

    private void Start()
    {
        pm = GetComponentInParent<PlateManager>();
    }

    private void Update()
    {
        aliveN = GetAliveNeighborCount();
    }

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

    public void UpdateGOL()
    {
        /*Rules:
1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
2. Any live cell with two or three live neighbours lives on to the next generation.
3. Any live cell with more than three live neighbours dies, as if by overpopulation.
4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction
*/

        if (colored) //Alive
        {
            if(aliveN < 2) Restart();
            else if(aliveN > 3) Restart();
        }
        else //Dead
        {
            if (aliveN == 3) ChangeColor();
        }
    }

    int GetAliveNeighborCount()
    {
        int count = 0;
        int[] dx = { -1, 0, 1, 0 };
        int[] dy = { 0, 1, 0, -1 };


        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            // Check if the neighbor is within the grid
            if (nx >= 0 && nx <= 24 && ny >= 0 && ny <= 24)
            {
                // Check if the neighbor is colored
                if (pm.platArr[nx, ny].colored)
                {
                    count++;
                }
            }
        }


        return count;
    }
}
