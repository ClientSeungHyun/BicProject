using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public GameObject missilePrefab; // �̻��� ������
    public float missileSpeed = 10f; // �̻��� �ӵ�
    public float trackingDistance = 100f; // ���� �Ÿ�
    public float trackingSpeed = 5f; // ���� �ӵ�
    public float idleDistance = 7f; // ���� �Ÿ�
    public float attackDuration = 3f; // ���� ���� �ð�
    public Animator animator; // �ִϸ�����

    private NavMeshAgent navMeshAgent;
    private float defaultSpeed; // �⺻ �̵� �ӵ�
    private float attackTimer; // ���� Ÿ�̸�
    private bool isAttacking; // ���� ���� ������ ����
    private float distance; // �÷��̾���� �Ÿ�

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // �⺻ �̵� �ӵ� ����
        navMeshAgent.enabled = true;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        if (distance > trackingDistance) // ���� ���� ��
        {
            StopTracking();
        }
        else if (distance <= idleDistance) // ���� ���� ��
        {
            if (!isAttacking)
            {
                StartAttack();
            }
            else
            {
                UpdateAttack();
            }
        }
        else // ���� ���� ��
        {
            TrackPlayer();
        }

        // �ִϸ��̼� ������Ʈ
        if (isAttacking)
        {
            animator.SetTrigger("idle"); 
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("run");
        }
    }

    private void TrackPlayer()
    {
        // �÷��̾ �����մϴ�.
        navMeshAgent.speed = trackingSpeed;
    }

    private void StopTracking()
    {
        // �̵��� ���߰� �׺���̼� ��θ� �ʱ�ȭ�մϴ�.
        navMeshAgent.speed = defaultSpeed;
    }

    private void StartAttack()
    {
        // �̵��� ���߰� ���� �ִϸ��̼��� ����մϴ�.
        navMeshAgent.speed = 0f;
        animator.SetTrigger("Attack");

        isAttacking = true;
        attackTimer = 0f;
    }

    private void UpdateAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        {
            // �÷��̾ ���� �̻����� �߻��մϴ�.
            GameObject missile = Instantiate(missilePrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            missile.GetComponent<Rigidbody>().velocity = transform.forward * missileSpeed;

            EndAttack();
        }
    }

    public void DestroyMissile(GameObject missile)
    {
        // �̻����� �ı��մϴ�.
        Destroy(missile);
    }

    private void EndAttack()
    {
        // ������ �����մϴ�.
        isAttacking = false;
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