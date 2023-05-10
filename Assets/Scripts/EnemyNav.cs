using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNav : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = walkSpeed; // Walk �ӵ� ����
        navMeshAgent.enabled = true;
    }

    void Update()
    {
        navMeshAgent.SetDestination(player.position); // 항상 플레이어 추적
        navMeshAgent.speed = Vector3.Distance(transform.position, player.position) < 15f ? runSpeed : walkSpeed; // 거리에따라 속도 조절
    }

    public float GetSpeed()
    {
        return navMeshAgent.speed;
    }
}