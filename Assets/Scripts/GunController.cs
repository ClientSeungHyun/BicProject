using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    private PlayerControl player;
    private Rigidbody rigidbody;

    public Transform playerHand;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public ObjectPool bulletPool;
    public GameObject bulletStartParticle;

    public Transform bulletStartTransform;
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        // PrimaryIndexTrigger 왼손 트리거 버튼
        if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) || Input.GetKeyDown(KeyCode.L)) && gameObject.name == "LeftHandPistol")
        {
            bulletStartParticle.transform.position = bulletStartTransform.position;
            bulletStartParticle.GetComponent<ParticleSystem>().Play();
            bulletPool.GetObject(bulletStartTransform.position);
        }
        // SecondaryIndexTrigger 오른손 트리거 버튼
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("오른손 트리거 버튼 클릭");
        }
        
      
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = transform.TransformDirection(playerHand.position);
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward) * playerHand.rotation;

        // Rigidbody를 사용하여 손잡이 부착
        rigidbody.MovePosition(playerHand.position + new Vector3(0, 0.02f, 0.05f));
        rigidbody.MoveRotation(targetRotation);
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

    private void Fire(Transform startBullet)
    {
        bulletPool.transform.position = startBullet.position;
    }

    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        rigidbody = GetComponent<Rigidbody>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        bulletStartParticle = GameObject.Find("StartBulletParticle");

        AttachToHand();
    }
}
