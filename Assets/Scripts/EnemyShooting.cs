using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public GameObject missilePrefab; // �̻��� ������
    public float missileSpeed = 10f; // �̻��� �ӵ�
    public float missileInterval = 2f; // �̻��� �߻� ����

    private float lastShootTime = 0f;

    void Update()
    {
        // �÷��̾�� �� ������ �Ÿ��� ����մϴ�.
        float distance = Vector3.Distance(transform.position, player.position);

        // ���� �÷��̾ ���� �̻����� �߻��մϴ�.
        if (distance < 50f && Time.time - lastShootTime > missileInterval) // �Ÿ��� 10���� �̳��̰� �߻� ������ ������ ��
        {
            // ���� �÷��̾ �ٶ󺸵��� �մϴ�.
            transform.LookAt(player.position);

            // �̻����� �����ϰ� �߻��մϴ�.
            GameObject missile = Instantiate(missilePrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            missile.GetComponent<Rigidbody>().velocity = transform.forward * missileSpeed;
            lastShootTime = Time.time;
        }
    }

}