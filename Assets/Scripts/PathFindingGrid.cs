using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFindingGrid : MonoBehaviour
{
    Camera cam;
    public Sprite available;
    public Sprite claimed;
    public Sprite current;
    public Sprite obstacle;
    public Sprite objective;

    private static readonly int Width = 25;
    private static readonly int Height = 25;
    Tile[,] tiles = new Tile[Width, Height];
    public GameObject TileObject;

    public int startX, startY, endX, endY;
    public bool started = true;
    float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindAnyObjectByType<Camera>();
        // Generate tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject tile = Instantiate(TileObject,
                                        new Vector3(x, 0f, y) * 1.05f,
                                        TileObject.transform.rotation);
                tiles[x,y] = tile.GetComponentInChildren<Tile>();
                tiles[x,y].x = x;
                tiles[x,y].y = y;
                tiles[x,y].PFG = this;
                tiles[x,y].state = Tile.State.Hidden;
            }
        }

        tiles[6,6].state = Tile.State.End;
        endX = 6;
        endY = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                CheckGame();
                Automate();
                timer = 0.5f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckGame();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            cam.transform.position += 0.5f * transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            cam.transform.position -= 0.5f * transform.right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cam.transform.position -= 0.5f * transform.forward;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cam.transform.position += 0.5f * transform.right;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cam.transform.position += 0.5f * transform.up;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            cam.transform.position -= 0.5f * transform.up;
        }
    }
    public void Automate()
    {
        Tile curTile = null;
        int currentMin = 1000;
        int currentGMin = 1000;

        foreach(Tile tile in tiles)
        {
            if (tile.state == Tile.State.Available)
            {
                if (tile.totalCostAmount < currentMin)
                {
                    currentMin = tile.totalCostAmount;
                    curTile = tile;
                }
                else if( tile.totalCostAmount == currentMin)
                {
                    if (tile.totalGAmount < currentGMin)
                    {
                        currentGMin = tile.totalGAmount;
                        curTile = tile;
                    }
                }
            }
        }
        if(curTile.state == Tile.State.Available)curTile.state = Tile.State.Claimed;
        CheckGame();
    }

    public void CheckGame()
    {
        foreach (Tile tile in tiles)
        {
            tile.CalculateCost();
            tile.CheckSurroundings();
        }
    }

    public Tile.State GetState(int x, int y)
    {
        Debug.Log("returning state: " + tiles[x, y].state.ToString());
        return tiles[x,y].state;
    }
    
}
