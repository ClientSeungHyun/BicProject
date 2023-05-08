using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private EnemyShooting enemyShooting;

    private void Start()
    {
        enemyShooting = FindObjectOfType<EnemyShooting>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyShooting.DestroyMissile(gameObject);
        }
    }
}