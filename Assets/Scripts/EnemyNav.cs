using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public float speed = 3f;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;

    }

    void Update()
    {
        // �÷��̾�� �� ������ �Ÿ��� ����մϴ�.
        float distance = Vector3.Distance(transform.position, player.position);

        // ���� �÷��̾ �����մϴ�.
        if (distance < 50f) // �Ÿ��� 10���� �̳��� �� ����
        {
            navMeshAgent.SetDestination(player.position); // NavMeshAgent�� �̿��Ͽ� �÷��̾ �����մϴ�.
        }
        else // �Ÿ��� 10���� �̻��̸� �̵��� ����ϴ�.
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}