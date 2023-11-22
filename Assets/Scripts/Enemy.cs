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
            PlayerController player = other.GetComponent<PlayerController>();
            if(player.currentPS == PlayerController.PowerState.PowerUp)
            {
                Destroy(gameObject);
            }
            else
            {
                OnEnemyKill?.Invoke();
                player.Die();
            }
        }
    }
}
