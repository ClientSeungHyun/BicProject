using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int weaponLV;
    public float speed = 10f; // 총알의 속도
    public Transform bulletStartTransform;
    public Transform bulletEndTransform;

    private ParticleSystem[] bulletStartParticle;
    private ParticleSystem[] bulletEndParticle;

    ParticleSystem.MainModule[] bulletStartModule;
    ParticleSystem.MainModule[] bulletEndModule;
    ParticleSystem.MainModule[] bulletFlyingModule;

    Color bulletColor;

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
        Init();
    }
    private void Update()
    {
        // 총알을 앞으로 이동시킵니다.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        SetBulletColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // 총알이 다른 충돌체에 부딪히면 비활성화합니다.
            bulletEndTransform.transform.position = transform.position;
            bulletEndTransform.GetComponent<ParticleSystem>().Play();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        // 총알을 비활성화하고 오브젝트 풀로 반환합니다.
        gameObject.SetActive(false);
    }

    private void Init()
    {
        weaponLV = GameManagers.playerInfo.WeaponLV();
        bulletStartTransform = GameObject.Find("StartBulletParticle").transform;
        bulletEndTransform = GameObject.Find("EndBulletParticle").transform;
        //transform.position = bulletStartTransform.position;

        bulletStartParticle = new ParticleSystem[4];
        bulletEndParticle = new ParticleSystem[4];

        bulletStartModule = new ParticleSystem.MainModule[4];
        bulletEndModule = new ParticleSystem.MainModule[4];
        bulletFlyingModule = new ParticleSystem.MainModule[2];

        for (int i = 0; i < 4; i++)
        {
            bulletStartParticle[i] = bulletStartTransform.GetChild(i).GetComponent<ParticleSystem>();
            bulletEndParticle[i] = bulletEndTransform.GetChild(i).GetComponent<ParticleSystem>();
            bulletStartModule[i] = bulletStartParticle[i].main;
            bulletEndModule[i] = bulletEndParticle[i].main;
        }
        bulletFlyingModule[0] = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        bulletFlyingModule[1] = transform.GetChild(1).GetComponent<ParticleSystem>().main;

        switch (weaponLV)
        {
            case 1:
                bulletColor = Color.blue;
                break;
            case 2:
                bulletColor = new Color(0.749f, 0, 0.588f);
                break;
            case 3:
                bulletColor = Color.red;
                break;
        }
        SetBulletColor();
    }

    private void SetBulletColor()
    {
        for (int i = 0; i < 4; i++)
        {
            bulletStartModule[i].startColor = bulletColor;
            bulletEndModule[i].startColor = bulletColor;
        }
        bulletFlyingModule[0].startColor = bulletFlyingModule[1].startColor = bulletColor;
    }
}
