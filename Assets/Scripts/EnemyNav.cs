using UnityEngine;
using UnityEngine.AI;
public class EnemyNav : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform

    private NavMeshAgent navMeshAgent;
    private float walkSpeed = 3f;
    private float runSpeed = 15f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed; // Walk �ӵ� ����
        navMeshAgent.enabled = true;
    }

    void Update()
    {
        navMeshAgent.SetDestination(player.position); // �׻� �÷��̾ �����մϴ�.
        navMeshAgent.speed = Vector3.Distance(transform.position, player.position) < 100f ? runSpeed : walkSpeed; // �Ÿ��� ���� �ӵ��� �����մϴ�.
    }
}