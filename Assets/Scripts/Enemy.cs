using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKill;

    public int value = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) OnEnemyKill.Invoke(this);
    }
}
