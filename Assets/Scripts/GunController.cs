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
        /*if (leftHand.grabbedObject == gameObject || rightHand.grabbedObject == gameObject)
        {
            transform.position = (leftHand.grabbedObject == gameObject) ? leftHand.transform.position : rightHand.transform.position;
            transform.rotation = (leftHand.grabbedObject == gameObject) ? leftHand.transform.rotation : rightHand.transform.rotation;

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (!isFiring)
                {
                    InvokeRepeating("Fire", 0f, 0.1f);
                    isFiring = true;
                }
            }
            else
            {
                if (isFiring)
                {
                    CancelInvoke("Fire");
                    isFiring = false;
                }
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        */

        if(Input.GetKeyDown(KeyCode.W)) 
        {
            Fire();
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

    private void Fire()
    {
        GameObject bullet = bulletPool.GetObject();
        bullet.transform.position = barrelEnd.position;
        bullet.transform.rotation = barrelEnd.rotation;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;
    }
}
