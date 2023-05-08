using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public GameObject missilePrefab; // 미사일 프리팹
    public float missileSpeed = 10f; // 미사일 속도
    public float missileInterval = 2f; // 미사일 발사 간격

    private float lastShootTime = 0f;

    void Update()
    {
        // 플레이어와 적 사이의 거리를 계산합니다.
        float distance = Vector3.Distance(transform.position, player.position);

        // 적이 플레이어를 향해 미사일을 발사합니다.
        if (distance < 50f && Time.time - lastShootTime > missileInterval) // 거리가 10미터 이내이고 발사 간격을 만족할 때
        {
            // 적이 플레이어를 바라보도록 합니다.
            transform.LookAt(player.position);

            // 미사일을 생성하고 발사합니다.
            GameObject missile = Instantiate(missilePrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            missile.GetComponent<Rigidbody>().velocity = transform.forward * missileSpeed;
            lastShootTime = Time.time;
        }
    }

}