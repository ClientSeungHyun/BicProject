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

    private bool isFiring;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
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
        }*/
        if(Input.GetKeyDown(KeyCode.W)) 
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject bullet = bulletPool.GetObject();
        bullet.transform.position = barrelEnd.position;
        bullet.transform.rotation = barrelEnd.rotation;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;
    }
}
