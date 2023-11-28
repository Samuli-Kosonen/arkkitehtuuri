using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject TileObject;
    private static readonly int Width = 25;
    private static readonly int Height = 25;
    bool[,] grid = new bool[Width, Height];
    bool[,] nextGrid = new bool[Width, Height]; // Second grid for double buffering
    GameObject[,] tiles = new GameObject[Width, Height];

    private float TimeAccu = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Generate tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Clear the grid
                grid[x, y] = false;
                nextGrid[x, y] = false; // Initialize the second grid
                                        // Instantiate the tile
                tiles[x, y] = Instantiate(TileObject,
                                        new Vector3(x, 0f, y) * 1.05f,
                                        TileObject.transform.rotation);
                tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
            }
        }

        grid[2, 15] = true;
        tiles[2, 15].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[0, 15] = true;
        tiles[0, 15].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[1, 15] = true;
        tiles[1, 15].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[2, 16] = true;
        tiles[2, 16].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[1, 17] = true;
        tiles[1, 17].GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private int GetLiveNeighbours(int x, int y)
    {
        int liveneighbours = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (!(i == x & j == y) && i >= 0 && j >= 0 && i < Width && j < Height)
                {
                    // current i,j is not x,y
                    if (grid[i, j] == true)
                    {
                        liveneighbours++;
                    }
                }
            }
        }
        return liveneighbours;
    }

    // Update is called once per frame
    void Update()
    {
        TimeAccu += Time.deltaTime;

        if (TimeAccu > 1.0f)
        {
            // First, fully update nextGrid based on the state of grid
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int live = GetLiveNeighbours(x, y);
                    if (grid[x, y] == true) // If the current cell is alive
                    {
                        if (live < 2 || live > 3)
                        {
                            nextGrid[x, y] = false;
                        }
                        else if (live == 2 || live == 3)
                        {
                            nextGrid[x, y] = true;
                        }
                    }
                    else // If the current cell is dead
                    {
                        if (live == 3)
                        {
                            nextGrid[x, y] = true;
                        }
                        else
                        {
                            nextGrid[x, y] = false;
                        }
                    }
                }
            }

            // Then, swap the grids
            var temp = grid;
            grid = nextGrid;
            nextGrid = temp;

            // Update the tiles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (grid[x, y] == true)
                    {
                        tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else
                    {
                        tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                }
            }

            TimeAccu = 0;
        }
    }

}