using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNav : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    public float walkSpeed = 3f;
    public float runSpeed = 15f;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = walkSpeed; // Walk �ӵ� ����
        navMeshAgent.enabled = true;
    }

    void Update()
    {
        navMeshAgent.SetDestination(player.position); // �׻� �÷��̾ �����մϴ�.
        navMeshAgent.speed = Vector3.Distance(transform.position, player.position) < 100f ? runSpeed : walkSpeed; // �Ÿ��� ���� �ӵ��� �����մϴ�.
    }

    public float GetSpeed()
    {
        return navMeshAgent.speed;
    }
}