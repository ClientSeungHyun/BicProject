using UnityEngine;
using UnityEngine.AI;
public class EnemyNav : MonoBehaviour
{
    public Transform player; // 플레이어 Transform

    private NavMeshAgent navMeshAgent;
    private float walkSpeed = 3f;
    private float runSpeed = 15f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed; // Walk 속도 설정
        navMeshAgent.enabled = true;
    }

    void Update()
    {
        navMeshAgent.SetDestination(player.position); // 항상 플레이어를 추적합니다.
        navMeshAgent.speed = Vector3.Distance(transform.position, player.position) < 100f ? runSpeed : walkSpeed; // 거리에 따라 속도를 설정합니다.
    }
}