using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 10f; // 총알의 속도
    public GameObject bulletEndParticle;

    private void OnEnable()
    {
        // 총알을 발사하면 일정 시간 후에 자동으로 비활성화합니다.
        Invoke("Deactivate", 2f);
    }

    private void OnDisable()
    {
        // 총알이 비활성화되면 취소합니다.
        CancelInvoke();
    }
    private void Start()
    {
        bulletEndParticle = GameObject.Find("EndBulletParticle");
    }
    private void Update()
    {
        // 총알을 앞으로 이동시킵니다.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 총알이 다른 충돌체에 부딪히면 비활성화합니다.
            bulletEndParticle.transform.position = transform.position;
            bulletEndParticle.GetComponent<ParticleSystem>().Play();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        // 총알을 비활성화하고 오브젝트 풀로 반환합니다.
        gameObject.SetActive(false);
    }
}
