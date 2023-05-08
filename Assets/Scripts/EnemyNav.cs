using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public float speed = 3f;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;

    }

    void Update()
    {
        // 플레이어와 적 사이의 거리를 계산합니다.
        float distance = Vector3.Distance(transform.position, player.position);

        // 적이 플레이어를 추적합니다.
        if (distance < 50f) // 거리가 10미터 이내일 때 추적
        {
            navMeshAgent.SetDestination(player.position); // NavMeshAgent를 이용하여 플레이어를 추적합니다.
        }
        else // 거리가 10미터 이상이면 이동을 멈춥니다.
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}