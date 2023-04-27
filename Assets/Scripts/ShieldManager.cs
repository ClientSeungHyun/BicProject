using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private int currenShieldLV;
    private float maxShieldSize;
    [SerializeField] private float shieldGrowSpeed;
    private float shieldPushForce;
    [SerializeField] private float currentShieldSize = 0.0f;
    
    private bool isGenereShieldActive;
    private bool isAccelShieldActive;

    private PlayerControl player;

    private void Start()
    {
        Init();
    }

    void Update()
    {
        if (currenShieldLV != player.ShieldLV())
        {
            currenShieldLV = player.ShieldLV();
            ShieldLVManage();
        }
        ShieldManage();
        PlayerEgManage();
    }

    //에너지 관리
    void PlayerEgManage()   
    {
        if (isGenereShieldActive == true)
            player.PlayerEg(player.PlayerEg() - 10.0f * Time.deltaTime);
        else if (player.PlayerEg() < 100 && isAccelShieldActive == false) // 에너지 까이면 지속적으로 채우기
            player.PlayerEg(player.PlayerEg() + 3.0f * Time.deltaTime);
        else if (player.PlayerEg() >= 100) //100을 넘겼을시 그냥 100으로 고정
            player.PlayerEg(100f);

    }

    //쉴드 관리
    void ShieldManage()
    {
        if (Input.GetKeyDown(KeyCode.Z) && player.PlayerEg() >= 50f) 
        {
            isAccelShieldActive = true;
            shieldGrowSpeed += 20;
            player.PlayerEg(player.PlayerEg() - 50f);
        }
        if (Input.GetKeyDown(KeyCode.B) && isAccelShieldActive == false && player.PlayerEg() > 10f) 
        {
            isGenereShieldActive = !isGenereShieldActive;
        }

        if (isAccelShieldActive == true || isGenereShieldActive == true) 
        {
            // 쉴드의 크기를 증가시키고, 최대 크기 지정
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);

            if (currentShieldSize == maxShieldSize && isAccelShieldActive == true) 
            {
                Invoke("ShieldDestroy", 1.0f);
            }
        }

        if (isAccelShieldActive == false && isGenereShieldActive == false)
        {
            transform.localScale = new Vector3(1, 1, 1);
            currentShieldSize = 1.0f;
        }

        if (player.PlayerEg() <= 0)
            isGenereShieldActive = isAccelShieldActive = false;
    }

    //Invoke용 쉴드 off
    void ShieldDestroy()
    {
        if(isAccelShieldActive == true) shieldGrowSpeed -= 20;
        isAccelShieldActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            // 적 오브젝트를 쉴드 방향으로 밀어냄
            Vector3 pushDirection = other.transform.position - transform.position;
            if(isAccelShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 2, ForceMode.Impulse);
            if (isGenereShieldActive == true)
            other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce, ForceMode.Impulse);
        }
    }

    private void ShieldLVManage()
    {
        switch (currenShieldLV) //쉴드 레벨에 따른 크기와 속도 차이
        {
            case 1:
                maxShieldSize = 25;
                shieldGrowSpeed = 15;
                break;
            case 2:
                maxShieldSize = 30;
                shieldGrowSpeed = 20;
                break;
            case 3:
                maxShieldSize = 35;
                shieldGrowSpeed = 25;
                break;
            default:
                maxShieldSize = 25;
                shieldGrowSpeed = 15;
                player.ShieldLV(1);
                break;
        }
    }

    //초기화 함수
    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        isGenereShieldActive = false;
        isAccelShieldActive = false;
        currenShieldLV = player.ShieldLV();
        shieldPushForce = 3.0f;
        ShieldLVManage();
    }
}
