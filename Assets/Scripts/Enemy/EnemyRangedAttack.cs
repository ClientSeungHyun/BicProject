using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player; // 플레이어 Transform
    public Animator animator; // 애니메이터
    public Transform target; // 플레이어의 위치
    public ObjectPool missilePool;
    public int MonsterCount; //몬스터 수 판별 함수
    private bool isAttacking; // 현재 공격 중인지 여부
    private float distance; // 플레이어와의 거리

    private float timer;
    private float timerDuration;

    private GameManagers gameManagerScript;    //게임 매니저 스크립트

    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timer = 0f;
        timerDuration = 5f;
    }

    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
            distance = Vector3.Distance(transform.position, player.position);
            navMeshAgent.SetDestination(player.position);

            //목적지 도착
            if (distance <= 12f)
            {
                StartAttack();
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Idle");
            }
            else
            {
                animator.SetTrigger("Run");
                navMeshAgent.isStopped = false;
            }
        }
    }

    private void StartAttack()
    {
        timer += Time.deltaTime;
        transform.LookAt(player.transform);
        if (timer >= timerDuration)
        {
            isAttacking = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            rotation *= Quaternion.Euler(0f, -90f, 0f);
            missilePool.GetObject(transform.position + transform.forward, rotation);
            timer = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Bullet과 충돌하면 death 애니메이션 재생 및 파괴
            animator.SetTrigger("Death");
            MonsterCount++;
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