using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private int currenShieldLV;
    private float maxShieldSize;
    private float shieldGrowSpeed;
    private float shieldPushForce;
    private float currentShieldSize = 0.0f;
    
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

    //������ ����
    void PlayerEgManage()   
    {
        if (isGenereShieldActive == true)
            player.PlayerEg(player.PlayerEg() - 10.0f * Time.deltaTime);
        else if (player.PlayerEg() < 100 && isAccelShieldActive == false) // ������ ���̸� ���������� ä���
            player.PlayerEg(player.PlayerEg() + 3.0f * Time.deltaTime);
        else if (player.PlayerEg() >= 100) //100�� �Ѱ����� �׳� 100���� ����
            player.PlayerEg(100f);

    }

    //���� ����
    void ShieldManage()
    {
        if (Input.GetKeyDown(KeyCode.Z) && player.PlayerEg() >= 50f) 
        {
            isAccelShieldActive = true;
            player.PlayerEg(player.PlayerEg() - 50f);
        }
        if (Input.GetKeyDown(KeyCode.B) && isAccelShieldActive == false && player.PlayerEg() > 10f) 
        {
            isGenereShieldActive = !isGenereShieldActive;
        }

        if (isAccelShieldActive == true || isGenereShieldActive == true) 
        {
            // ������ ũ�⸦ ������Ű��, �ִ� ũ�� ����
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);

            if (currentShieldSize == maxShieldSize && isAccelShieldActive == true) 
            {
                Invoke("ShieldDestroy", 1);
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

    //Invoke�� ���� off
    void ShieldDestroy()
    {
        isAccelShieldActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((isAccelShieldActive || isGenereShieldActive) && other.CompareTag("Enemy")) 
        {
            // �� ������Ʈ�� ���� �������� �о
            Vector3 pushDirection = other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce, ForceMode.Impulse);
        }
    }

    private void ShieldLVManage()
    {
        switch (currenShieldLV) //���� ������ ���� ũ��� �ӵ� ����
        {
            case 1:
                maxShieldSize = 5;
                shieldGrowSpeed = 5;
                break;
            case 2:
                maxShieldSize = 7;
                shieldGrowSpeed = 7;
                break;
            case 3:
                maxShieldSize = 10;
                shieldGrowSpeed = 10;
                break;
            default:
                maxShieldSize = 5;
                shieldGrowSpeed = 5;
                player.ShieldLV(1);
                break;
        }
    }

    //�ʱ�ȭ �Լ�
    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        isGenereShieldActive = false;
        isAccelShieldActive = false;
        currenShieldLV = player.ShieldLV();
        ShieldLVManage();
    }
}
