using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public float attackRange = 0.5f; // ���� ����
    public float attackCooldown = 2f; // ���� ��ٿ� �ð�
    public Animator animator; // �ִϸ�����
    private bool isAttacking; // ���� ������ ����
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
            isAttacking = true;
            Attack();
        }
        else
        {
            // �÷��̾ ���� �̵�
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = 5f; // �׻� 5�� �ӵ��� ����

            if (!isAttacking)
            {
                animator.SetTrigger("run"); // �׻� run �ִϸ��̼� ���
            }
        }
    }

    private void Attack()
    {
        // ���� �ִϸ��̼� ��� �� ��ٿ� Ÿ�̸� ����
        attackTimer = attackCooldown;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player�� �浹�ϸ� Death �ִϸ��̼� ��� �� �ı�
            animator.SetTrigger("Death");
            // �ִϸ��̼� ��� �ð���ŭ ������ �Ŀ� ������Ʈ �ı�
            float deathAnimationLength = GetDeathAnimationLength();
            StartCoroutine(DestroyAfterDelay(deathAnimationLength));
        }
    }

    private float GetDeathAnimationLength()
    {
        // Death �ִϸ��̼� Ŭ���� ���̸� ������
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Death")
            {
                return clip.length;
            }
        }
        return 0f;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ������ �Ŀ� ������Ʈ �ı�
        Destroy(gameObject);
    }
}