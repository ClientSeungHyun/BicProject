using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public float attackRange = 0.5f; // ���� ����
    public float attackCooldown = 2f; // ���� ��ٿ� �ð�
    public Animator animator; // �ִϸ�����

    private NavMeshAgent navMeshAgent;
    private float attackTimer; // ���� Ÿ�̸�

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // �÷��̾�� ��ġ�� ���� �ִϸ��̼� ���
            animator.SetTrigger("Attack");
            Attack();
        }
        else
        {
            // �÷��̾ ���� �̵�
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = 5f; // �׻� 5�� �ӵ��� ����
            animator.SetTrigger("run"); // �׻� run �ִϸ��̼� ���
        }
    }

    private void Attack()
    {
        // ���� �ִϸ��̼� ��� �� ��ٿ� Ÿ�̸� ����
        attackTimer = attackCooldown;
        // ���� ���� �߰� (��: �÷��̾�� �������� ������ ��)
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
        {
            // ���� ��ٿ� �ð� ����
            attackTimer -= Time.fixedDeltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // �÷��̾��� �Ѿ˿� ������ death �ִϸ��̼� ��� �� �ı�
            animator.SetTrigger("Death");
            Destroy(gameObject);
        }
    }
}