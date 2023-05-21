using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public float attackRange = 0.5f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운 시간
    public Animator animator; // 애니메이터
    private bool isAttacking; // 공격 중인지 여부
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
            isAttacking = true;
            Attack();
        }
        else
        {
            // 플레이어를 향해 이동
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = 5f; // 항상 5의 속도로 추적

            if (!isAttacking)
            {
                animator.SetTrigger("run"); // 항상 run 애니메이션 재생
            }
        }
    }

    private void Attack()
    {
        // 공격 애니메이션 재생 후 쿨다운 타이머 시작
        attackTimer = attackCooldown;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player과 충돌하면 Death 애니메이션 재생 및 파괴
            animator.SetTrigger("Death");
            // 애니메이션 재생 시간만큼 딜레이 후에 오브젝트 파괴
            float deathAnimationLength = GetDeathAnimationLength();
            StartCoroutine(DestroyAfterDelay(deathAnimationLength));
        }
    }

    private float GetDeathAnimationLength()
    {
        // Death 애니메이션 클립의 길이를 가져옴
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
        // 딜레이 후에 오브젝트 파괴
        Destroy(gameObject);
    }
}