using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyKill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            OnEnemyKill?.Invoke();
            other.GetComponent<PlayerController>().Die();
        }
    }
}
