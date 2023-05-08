using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float lifetime = 5f; // �̻��� ����

    private void Start()
    {
        // ���� �ð��� ���� �ڿ� �ڵ����� �̻����� �����մϴ�.
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �÷��̾��� ��쿡�� �̻����� �����մϴ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}