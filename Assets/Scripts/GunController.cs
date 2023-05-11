using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour
{
    public Transform barrelEnd;
    public ObjectPool bulletPool;
    public OVRGrabber leftHand;
    public OVRGrabber rightHand;

    public GameObject[] guns;
    public PlayerControl player;

    private Rigidbody rigidbody;

    public Transform playerHand;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    private bool isFiring;

    public Transform leftBulletStart;
    public Transform rightBulletStart;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

        rigidbody = GetComponent<Rigidbody>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        AttachToHand();
    }
    private void Update()
    {
        // PrimaryIndexTrigger 왼손 트리거 버튼
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            Debug.Log("왼손 트리거 버튼 클릭");
        }
        // SecondaryIndexTrigger 오른손 트리거 버튼
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("오른손 트리거 버튼 클릭");
        }
        
      
    }
    void FixedUpdate()
    {
        playerHand.rotation = Quaternion.Euler(0, 180, 0);
        rigidbody.MovePosition(playerHand.position);
        rigidbody.MoveRotation(playerHand.rotation);
    }

    public void AttachToHand()
    {
        // 총의 물리 시뮬레이션 비활성화
        rigidbody.isKinematic = true;

        // 총의 위치와 회전값을 플레이어의 손에 맞게 설정
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
    }

    private void Fire(Transform startBullet)
    {
        bulletPool.transform.position = startBullet.position;
    }
}
