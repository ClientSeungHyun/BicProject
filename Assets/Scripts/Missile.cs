using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private EnemyRangedAttack enemyRangedAttack;

    private void Start()
    {
        enemyRangedAttack = FindObjectOfType<EnemyRangedAttack>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 객체가 플레이어인 경우
            Destroy(gameObject); // 미사일을 제거합니다.
        }
    }
}