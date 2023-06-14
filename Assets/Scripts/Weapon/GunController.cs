using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    private PlayerControl player;
    private GameManagers gameManagerScript;    //게임 매니저 스크립트
    private Rigidbody rigidbody;

    public Transform playerHand;
    public Transform bulletStartTransform;

    public ObjectPool bulletPool;
    public GameObject bulletStartParticle;
    private AudioSource audioSource;

    public DissolveChilds gunDissolveScript;

    public int weaponLV;
    public int reloadLV;

    private float reloadTime;    //총알 장전 속도
    public float maxBulletMagazine;    //최대 총알 잔탄
    public float currentBulletMagzine;   //현재 총알 수
    public float vibrationDuration;     //진동 시간
    public float vibrationStrength;     //진동 강도

    private bool isReloading;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
            //총이 생성되어있다면
            if (gunDissolveScript.IsGenerate() && !isReloading)
            {
                //왼손 트리거 버튼
                if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.L)) && gameObject.tag == "LeftHandPistol")
                {
                    StartCoroutine(Fire(OVRInput.Controller.LTouch));
                }
                //오른손 트리거 버튼
                if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.R)) && gameObject.tag == "RightHandPistol")
                {
                    StartCoroutine(Fire(OVRInput.Controller.RTouch));
                }
            }

            //총알이 다 떨어지면 자동 재장전
            if (currentBulletMagzine <= 0)
            {
                Reload();
            }
        }
        AttachToHand();
    }
    void FixedUpdate()
    {
    }

    public void AttachToHand()
    {
        // 총의 물리 시뮬레이션 비활성화
        rigidbody.isKinematic = true;

        // 총의 위치와 회전값을 플레이어의 손에 맞게 설정
        transform.position = new Vector3(playerHand.position.x, playerHand.position.y, playerHand.position.z);
        Vector3 gunAngle = playerHand.transform.eulerAngles;

        //gunAngle.y *= -1;
        //gunAngle.y += 180f;
        if (gameObject.CompareTag("RightHandPistol")) gunAngle.z += 90f;
        if (gameObject.CompareTag("LeftHandPistol")) gunAngle.z -= 90f;
        Quaternion gunRoatation = Quaternion.Euler(gunAngle);
        transform.rotation = gunRoatation;
    }

    private void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            audioSource.Play();
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        float duration = reloadTime; // 증가할 시간
        float elapsedTime = 0f;
        float startValue = 0f;
        float endValue = maxBulletMagazine;

        while (elapsedTime < reloadTime)
        {
            currentBulletMagzine = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        currentBulletMagzine = endValue;
        isReloading = false;
    }

    private IEnumerator Fire(OVRInput.Controller controller)
    {
        if (currentBulletMagzine >= 1)
        {
            currentBulletMagzine -= 1;
            bulletStartParticle.transform.position = new Vector3(bulletStartTransform.position.x, bulletStartTransform.position.y, bulletStartTransform.position.z);
            bulletStartParticle.GetComponent<ParticleSystem>().Play();
            bulletPool.GetObject(bulletStartTransform.position, bulletStartTransform.rotation);

            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, controller);
            yield return new WaitForSeconds(vibrationDuration);
            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }

    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        bulletPool = GameObject.Find("Bullet").GetComponent<ObjectPool>();
        bulletStartTransform = transform.GetChild(0).transform.GetChild(0).transform;
       
        bulletStartParticle = GameObject.Find("StartBulletParticle");

        isReloading = false;
        AttachToHand();

        vibrationDuration = 0.1f;
        vibrationStrength = 0.4f;

        weaponLV = gameManagerScript.playerInfo.WeaponLV();
        //웨폰 레벨 설정
        switch (weaponLV)
        {
            case 1:
                reloadTime = 3f;
                maxBulletMagazine = 50f;
                audioSource.pitch = 4f;
                break;
            case 2:
                reloadTime = 2f;
                maxBulletMagazine = 75f;
                audioSource.pitch = 6f;
                break;
            case 3:
                reloadTime = 1f;
                maxBulletMagazine = 100f;
                audioSource.pitch = 12f;
                break;
        }
        currentBulletMagzine = maxBulletMagazine;

        
    }
}
