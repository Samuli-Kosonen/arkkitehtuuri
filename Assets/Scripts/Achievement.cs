using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    int highscoreNum = 0;

    private const int nAchievements = 3;
    public enum Achievement_ID
    {
        Coin_Collector,
        Terminated,
        SomeOtherFancyAchievement
    }

    // Are the achievements unlocked
    private bool[] bUnlockedAchievements = new bool[nAchievements];


    public int nCoins = 0;
    int nDeaths = 0;

    private void Start()
    {
        // Add our method to listen to Coin-class event:
        Coin.OnCoinCollected += CoinWasCollected;
        Enemy.OnEnemyKill += EnemyKilledPlayer;
    }

    private void Update()
    {
        score.text = "Score: " + nCoins.ToString();
        if(highscoreNum < nCoins) highscoreNum = nCoins;

        highScore.text = "Highscore: " + highscoreNum.ToString();
    }
    void CoinWasCollected()
    {
        nCoins++;

        if (nCoins == 5)
        {
            int index = (int)Achievement_ID.Coin_Collector;
            if (!bUnlockedAchievements[index])
            {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: COIN COLLECTOR!!!");
                Debug.Log("Collect 5 coins without dying");
            }
        }
    }

    public void EnemyKilledPlayer()
    {
        nCoins = 0;
        nDeaths++;
        if(nDeaths == 5)
        {
            int index = (int)Achievement_ID.Terminated;
            if (!bUnlockedAchievements[index])
            {
                bUnlockedAchievements[index] = true;
                Debug.Log("You've unlocked: TERMINATED!!!");
                Debug.Log("Die 5 times");
            }
        }
    }
}
