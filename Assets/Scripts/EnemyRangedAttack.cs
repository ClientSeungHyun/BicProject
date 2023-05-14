using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public GameObject missilePrefab; // 미사일 프리팹
    public float missileSpeed = 10f; // 미사일 속도
    public float trackingDistance = 100f; // 추적 거리
    public float trackingSpeed = 5f; // 추적 속도
    public float idleDistance = 7f; // 공격 거리
    public float attackDuration = 3f; // 공격 지속 시간
    public Animator animator; // 애니메이터

    private NavMeshAgent navMeshAgent;
    private float defaultSpeed; // 기본 이동 속도
    private float attackTimer; // 공격 타이머
    private bool isAttacking; // 현재 공격 중인지 여부
    private float distance; // 플레이어와의 거리

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed; // 기본 이동 속도 저장
        navMeshAgent.enabled = true;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);
        navMeshAgent.SetDestination(player.position);

        if (distance > trackingDistance) // 추적 범위 밖
        {
            StopTracking();
        }
        else if (distance <= idleDistance) // 공격 범위 내
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
        else // 추적 범위 내
        {
            TrackPlayer();
        }

        // 애니메이션 업데이트
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
        // 플레이어를 추적합니다.
        navMeshAgent.speed = trackingSpeed;
    }

    private void StopTracking()
    {
        // 이동을 멈추고 네비게이션 경로를 초기화합니다.
        navMeshAgent.speed = defaultSpeed;
    }

    private void StartAttack()
    {
        // 이동을 멈추고 공격 애니메이션을 재생합니다.
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
            // 플레이어를 향해 미사일을 발사합니다.
            GameObject missile = Instantiate(missilePrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            missile.GetComponent<Rigidbody>().velocity = transform.forward * missileSpeed;

            EndAttack();
        }
    }

    public void DestroyMissile(GameObject missile)
    {
        // 미사일을 파괴합니다.
        Destroy(missile);
    }

    private void EndAttack()
    {
        // 공격을 종료합니다.
        isAttacking = false;
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