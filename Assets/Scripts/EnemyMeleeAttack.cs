using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public float attackRange = 0.5f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운 시간
    public Animator animator; // 애니메이터

    private NavMeshAgent navMeshAgent;
    private float attackTimer; // 공격 타이머

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // 플레이어와 겹치면 공격 애니메이션 재생
            animator.SetTrigger("Attack");
            Attack();
        }
        else
        {
            // 플레이어를 향해 이동
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = 5f; // 항상 5의 속도로 추적
            animator.SetTrigger("run"); // 항상 run 애니메이션 재생
        }
    }

    private void Attack()
    {
        // 공격 애니메이션 재생 후 쿨다운 타이머 시작
        attackTimer = attackCooldown;
        // 공격 로직 추가 (예: 플레이어에게 데미지를 입히는 등)
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
        {
            // 공격 쿨다운 시간 감소
            attackTimer -= Time.fixedDeltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 플레이어의 총알에 맞으면 death 애니메이션 재생 및 파괴
            animator.SetTrigger("Death");
            Destroy(gameObject);
        }
    }
}