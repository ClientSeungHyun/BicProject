using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float maxShieldSize;
    public float shieldGrowSpeed;
    public float shieldPushForce;

    public bool isShieldActive = false;
    public float currentShieldSize = 0.0f;

    public PlayerControl player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        switch(player.shieldLV) //쉴드 레벨에 따른 크기와 속도 차이
        {
            case 1:
                maxShieldSize = 10;
                shieldGrowSpeed = 5;
                break;
            case 2:
                maxShieldSize = 15;
                shieldGrowSpeed = 7;
                break;
            case 3:
                maxShieldSize = 20;
                shieldGrowSpeed = 10;
                break;
            default:
                maxShieldSize = 10;
                shieldGrowSpeed = 5;
                player.shieldLV = 1;
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isShieldActive = true;
        }

        if (isShieldActive)
        {
            // 쉴드의 크기를 증가시키고, 최대 크기 지정
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);

            if (currentShieldSize == maxShieldSize)
            {
                isShieldActive = false;
            }
        }

        if (isShieldActive == false)
        {
            transform.localScale = new Vector3(1, 1, 1);
            currentShieldSize = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isShieldActive && other.CompareTag("enemy"))
        {
            // 적 오브젝트를 쉴드 방향으로 밀어냄
            Vector3 pushDirection = other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce, ForceMode.Impulse);
        }
    }
}
