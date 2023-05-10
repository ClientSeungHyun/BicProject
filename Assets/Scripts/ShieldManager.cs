using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    private int shieldLV;
    [SerializeField] private int currenShieldLV;
    [SerializeField] private float maxShieldSize;
    [SerializeField] private float shieldGrowSpeed;
    private float shieldPushForce;
    [SerializeField] private float currentShieldSize = 0.0f;

    [SerializeField] private bool isGenereShieldActive;
    [SerializeField]  private bool isAccelShieldActive;
    [SerializeField] private bool isAccelReady;

    private PlayerControl player;
    public Animator animator;
    
    private void Awake()
    {
        Init();
    }

    //������ ����
    public void PlayerEgManage()   
    {
        if (isGenereShieldActive == true)
            player.PlayerEg(player.PlayerEg() - 10.0f * Time.deltaTime);
        else if (player.PlayerEg() < 100 && isAccelShieldActive == false) // ������ ���̸� ���������� ä���
            player.PlayerEg(player.PlayerEg() + 3.0f * Time.deltaTime);
        else if (player.PlayerEg() >= 100) //100�� �Ѱ����� �׳� 100���� ����
            player.PlayerEg(100f);

    }
    public void GenereShield() //�Ϲ� ���� ���
    {
        if (isGenereShieldActive == true)
        {
            // ������ ũ�⸦ ������Ű��, �ִ� ũ�� ����
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
        }
    }

    public void StartAccelShield() //�׼� ���� ���
    {
        float startShieldSize = maxShieldSize + 1.0f;

        
        if (isAccelShieldActive == true && isAccelReady == false)
        {
            shieldGrowSpeed += 10;
            
            //���带 ����
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, startShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
            
        }
       
    }

    public void EndAccelShield()
    {
        float endShieldSize = maxShieldSize + 8.0f;
        shieldGrowSpeed += 10;
        currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, endShieldSize);
        transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
       
        if (currentShieldSize >= endShieldSize)
        {
            isAccelReady = false;
            isAccelShieldActive = false;
            shieldGrowSpeed -= 20;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            // �� ������Ʈ�� ���� �������� �о
            Vector3 pushDirection = other.transform.position - transform.position;
            if(isAccelShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
            if (isGenereShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
        }
    }

    public void ShieldLVManage()
    {
        switch (currenShieldLV) //���� ������ ���� ũ��� �ӵ� ����
        {
            case 1:
                maxShieldSize = 4;
                shieldGrowSpeed = 3;
                break;
            case 2:
                maxShieldSize = 6;
                shieldGrowSpeed = 5;
                break;
            case 3:
                maxShieldSize = 8;
                shieldGrowSpeed =8;
                break;
            default:
                maxShieldSize = 4;
                shieldGrowSpeed = 3;
                shieldLV = 1;
                break;
        }
        if (currenShieldLV != shieldLV)
        {
            currenShieldLV = shieldLV;
        }
    }

    public void ShieldLevelUp()
    {
        if (shieldLV >= 3) //�ִ� 3���� ����
        {
            shieldLV = 3;
        }
        else //�ִ� ���� �ƴ� �� ����
        {
            shieldLV++;
        }
    }

    //�ʱ�ȭ �Լ�
    private void Init()
    {
        ShieldLVManage();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        animator = player.GetComponent<Animator>();
        isGenereShieldActive = false;
        isAccelShieldActive = false;
        isAccelReady = false;
        currenShieldLV = shieldLV;
        shieldPushForce = 3.0f;

    }

    public bool IsGenereShield()
    { 
        return isGenereShieldActive;
    }
    public void IsGenereShield(bool s)
    {
        isGenereShieldActive = s;
    }

    public bool IsAccelShield()
    {
        return isAccelShieldActive;
    }
    public void IsAccelShield(bool s)
    {
        isAccelShieldActive = s;
    }

    public float CurrentShieldSize()
    {
        return currentShieldSize;
    }
    public void CurrentShieldSize(float c)
    {
        currentShieldSize = c;
    }

    public bool IsAccelReady()
    {
        return isAccelReady;
    }
    public void IsAccelReady(bool ar)
    {
        isAccelReady = ar;
    }
}
