using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform target; // 플레이어의 위치
    private float speed; // 미사일 속도


    private void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // 플레이어를 향해 미사일 이동
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetTarget(Transform targetTransform)
    {
        // 플레이어 설정
        target = targetTransform;
    }

    public void Launch(float missileSpeed)
    {
        // 미사일 발사 설정
        speed = missileSpeed;
        gameObject.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 미사일 비활성화
            gameObject.SetActive(false);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        // 총알을 비활성화하고 오브젝트 풀로 반환합니다.
        gameObject.SetActive(false);
    }

  

    

}