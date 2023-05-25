using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int weaponLV;
    public float speed = 10f; // �Ѿ��� �ӵ�
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
        // �Ѿ��� �߻��ϸ� ���� �ð� �Ŀ� �ڵ����� ��Ȱ��ȭ�մϴ�.
        Invoke("Deactivate", 2f);
    }

    private void OnDisable()
    {
        // �Ѿ��� ��Ȱ��ȭ�Ǹ� ����մϴ�.
        CancelInvoke();
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        // �Ѿ��� ������ �̵���ŵ�ϴ�.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        SetBulletColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // �Ѿ��� �ٸ� �浹ü�� �ε����� ��Ȱ��ȭ�մϴ�.
            bulletEndTransform.transform.position = transform.position;
            bulletEndTransform.GetComponent<ParticleSystem>().Play();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        // �Ѿ��� ��Ȱ��ȭ�ϰ� ������Ʈ Ǯ�� ��ȯ�մϴ�.
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
