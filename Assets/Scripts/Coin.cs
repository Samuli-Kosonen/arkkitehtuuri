using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event Action<Coin> OnCoinCollected;

    public int coinValue = 1;

    private void OnDisable()
    {
        // Invoke registered methods
        OnCoinCollected?.Invoke(this);
    }
}
