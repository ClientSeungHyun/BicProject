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

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public ObjectPool bulletPool;
    public GameObject bulletStartParticle;
    private AudioSource audioSource;

    public DissolveChilds gunDissolveScript;

    public int weaponLV;
    public int reloadLV;

    private float reloadTime;    //총알 장전 속도
    public float maxBulletMagazine;    //최대 총알 잔탄
    public float currentBulletMagzine;   //현재 총알 수
    private bool isReloading;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (gameManagerScript.IsPlaying())
        {
            Debug.Log("플레이중");
            //총이 생성되어있다면
            if (gunDissolveScript.IsGenerate() && !isReloading)
            {
                Debug.Log("조건만족");
                // PrimaryIndexTrigger 왼손 트리거 버튼
                if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.L)) && gameObject.tag == "LeftHandPistol")
                {
                    Debug.Log("발사");
                    Fire();
                }
                // SecondaryIndexTrigger 오른손 트리거 버튼
                if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.R)) && gameObject.tag == "RightHandPistol")
                {
                    Fire();
                }
            }

            //총알이 다 떨어지면 자동 재장전
            if (currentBulletMagzine <= 0)
            {
                Reload();
            }
        }
      
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = transform.TransformDirection(playerHand.position);
        Quaternion targetRotation = playerHand.rotation;

        // Rigidbody를 사용하여 손잡이 부착
        GetComponent<Rigidbody>().MovePosition(playerHand.position + new Vector3(0, 0.07f, 0.3f));
        GetComponent<Rigidbody>().MoveRotation(targetRotation);
    }

    public void AttachToHand()
    {
        // 총의 물리 시뮬레이션 비활성화
        rigidbody.isKinematic = true;

        // 총의 위치와 회전값을 플레이어의 손에 맞게 설정
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    
    private void Fire()
    {
        if (currentBulletMagzine >= 1)
        {
            currentBulletMagzine -= 1;
            bulletStartParticle.transform.position = bulletStartTransform.position;
            bulletStartParticle.GetComponent<ParticleSystem>().Play();
            bulletPool.GetObject(bulletStartTransform.position, bulletStartTransform.rotation);
        }
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

    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        bulletPool = GameObject.Find("Bullet").GetComponent<ObjectPool>();
        bulletStartTransform = transform.GetChild(1).transform;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        bulletStartParticle = GameObject.Find("StartBulletParticle");

        isReloading = false;
        AttachToHand();

        weaponLV = gameManagerScript.playerInfo.WeaponLV();
        reloadLV = gameManagerScript.playerInfo.ReloadLV();
        //웨폰 레벨 설정
        switch (weaponLV)
        {
            case 1:
                reloadTime = 3f;
                audioSource.pitch = 4f;
                break;
            case 2:
                reloadTime = 2f;
                audioSource.pitch = 6f;
                break;
            case 3:
                reloadTime = 1f;
                audioSource.pitch = 12f;
                break;
        }
        //재장전 레벨 설정
        switch (reloadLV)
        {
            case 1:
                maxBulletMagazine = 50f;
                break;
            case 2:
                maxBulletMagazine = 75f;
                break;
            case 3:
                maxBulletMagazine = 100f;
                break;
        }
        currentBulletMagzine = maxBulletMagazine;

        
    }
}
