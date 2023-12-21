using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int totalCostAmount;
    public int totalGAmount;
    public int totalHAmount;
    public PathFindingGrid PFG;
    public int x, y;
    Image image;
    public Sprite available;
    public Sprite claimed;
    public Sprite current;
    public Sprite obstacle;
    public Sprite objective;

    public TextMeshProUGUI hCost;
    public TextMeshProUGUI gCost;
    public TextMeshProUGUI totalCost;

    public enum State{
        Hidden,
        Available,
        Claimed,
        Current,
        Obstacle,
        Start,
        End
    }
    public State state;

    // Start is called before the first frame update
    void Start()
    {
       image = GetComponentInChildren<Image>();
       StartCoroutine(Initialize());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Hidden:
                image.sprite = default;
                break;
            case State.Available:
                image.sprite = available;
                CalculateCost();
                break;
            case State.Claimed:
                image.sprite = claimed;
                break;
            case State.Current:
                image.sprite = current;
                break;
            case State.Obstacle:
                image.sprite = obstacle;
                break;
            case State.Start:
                image.sprite = objective;
                PFG.startX = x;
                PFG.startY = y;
                break;
            case State.End:
                image.sprite = objective;
                break;
            default:
                state = State.Hidden; 
                break;
        }
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1);
        CalculateCost();
    }

    public void CalculateCost()
    {
        if (state == State.Hidden && state == State.Obstacle) return;
        //erotus nykysestä tilestä lopputileen, yhteiset x ja y = 14 erilliset = 10

        int distanceToEndX = Math.Abs(x - PFG.endX);
        int distanceToEndY = Math.Abs(y - PFG.endY);
        int distanceToStartX = Math.Abs(x - PFG.startX);
        int distanceToStartY = Math.Abs(y - PFG.startY);

        if(distanceToEndX > distanceToEndY)
        {
            //G COST
            int xAmount = distanceToEndX - distanceToEndY;
            int diagonalAmount = distanceToEndX - xAmount;

            totalGAmount= (diagonalAmount * 14) + (xAmount * 10);
            gCost.text = totalGAmount.ToString();
        }
        else if (distanceToEndX == distanceToEndY)
        {
            //G COST
            totalGAmount = distanceToEndX *14;
            gCost.text = totalGAmount.ToString();
        }
        else
        {
            //G COST
            int yAmount = distanceToEndY - distanceToEndX;
            int diagonalAmount = distanceToEndY - yAmount;

            totalGAmount = (diagonalAmount * 14) + (yAmount * 10);
            gCost.text = totalGAmount.ToString();
        }

        if(distanceToStartX > distanceToStartY)
        {
            //H COST
            int xAmount = distanceToStartX - distanceToStartY;
            int diagonalAmount = distanceToStartX - xAmount;

            totalHAmount = (diagonalAmount * 14) + (xAmount * 10);
            hCost.text = totalHAmount.ToString();
        }
        else if(distanceToStartX == distanceToStartY)
        {
            //H COST
            totalHAmount = distanceToStartX * 14;
            hCost.text = totalHAmount.ToString();
        }
        else  {
            //H COST
            int yAmount = distanceToStartY - distanceToStartX;
            int diagonalAmount = distanceToStartY - yAmount;

            totalHAmount = (diagonalAmount * 14) + (yAmount * 10);
            hCost.text = totalHAmount.ToString();
        }

        //TOTAL COST
        totalCostAmount = totalGAmount + totalHAmount;
        totalCost.text = totalCostAmount.ToString();
    }

    public void CheckSurroundings()
    {
        if (state == State.Hidden)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;

                    if (checkX >= 0 && checkY >= 0 && checkX <= 24 && checkY <= 24)
                    {
                        State neighbourState = PFG.GetState(checkX, checkY);
                        if (neighbourState == State.Start || neighbourState == State.Claimed)
                        {
                            state = State.Available;
                            Debug.Log("Löytyi");
                        }
                    }
                }
            }
        }

        if(state == State.End) 
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;

                    if (checkX >= 0 && checkY >= 0 && checkX <= 24 && checkY <= 24)
                    {
                        State neighbourState = PFG.GetState(checkX, checkY);
                        if (neighbourState == State.Claimed)
                        {
                            PFG.started = false;
                            Debug.Log("loppu");
                        }
                    }
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)){
            if(state == State.Available)
            {
                state = State.Claimed;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(state == State.Hidden)
            {
                PFG.started = true;
                state = State.Start;
            }
        }
        if(Input.GetMouseButtonDown(2))
        {
            if (state == State.Hidden)
            {
                state = State.Obstacle;
            }
        }
    }
}
