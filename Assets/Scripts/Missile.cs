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
            // �浹�� ��ü�� �÷��̾��� ���
            Destroy(gameObject); // �̻����� �����մϴ�.
        }
    }
}