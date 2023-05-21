using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{

    [SerializeField] private float maxShieldSize;
    [SerializeField] private float shieldGrowSpeed;
    private float shieldPushForce;
    [SerializeField] private float currentShieldSize = 0.0f;

    [SerializeField] private bool isGenereShieldActive;
    [SerializeField]  private bool isBoostShieldActive;
    [SerializeField] private bool isBoostReady;

    private PlayerControl player;
    public Animator animator;
    
    private void Awake()
    {
        Init();
    }

    //������ ���� �� ȸ��
    public void PlayerEgManage()   
    {
        if (isGenereShieldActive == true)
            player.PlayerEg(player.PlayerEg() - player.ShieldEnergyConsumption() * Time.deltaTime);
        else if (player.PlayerEg() < 100 && isBoostShieldActive == false) // ������ ���̸� ���������� ä���
            player.PlayerEg(player.PlayerEg() + 3.0f * Time.deltaTime);
        else if (player.PlayerEg() >= 100) //100�� �Ѱ����� �׳� 100���� ����
            player.PlayerEg(100f);

    }
    //�Ϲ� ���� ���
    public void GenereShield() 
    {
        if (isGenereShieldActive == true)
        {
            // ������ ũ�⸦ ������Ű��, �ִ� ũ�� ����
            shieldGrowSpeed = 10;
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
        }
    }
    //�ν�Ʈ ���� ���
    public void StartBoostShield() 
    {
        float startShieldSize = maxShieldSize + 1.0f;

        
        if (isBoostShieldActive == true && isBoostReady == false)
        {
            shieldGrowSpeed = 20;
            
            //���带 ����
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, startShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
            
        }
       
    }
    //�ν�Ʈ ���� ������� ��� 
    public void EndBoostShield()
    {
        float endShieldSize = maxShieldSize + 8.0f;
        shieldGrowSpeed = 30;
        currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, endShieldSize);
        transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
       
        if (currentShieldSize >= endShieldSize)
        {
            isBoostReady = false;
            isBoostShieldActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            // �� ������Ʈ�� ���� �������� �о
            Vector3 pushDirection = other.transform.position - transform.position;
            if(isBoostShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
            if (isGenereShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
        }
    }

    //�ʱ�ȭ �Լ�
    private void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        animator = player.GetComponent<Animator>();
        isGenereShieldActive = false;
        isBoostShieldActive = false;
        isBoostReady = false;
        maxShieldSize = 5;
        shieldPushForce = 4.0f;
        shieldGrowSpeed = 10f;

    }

    //�Ϲ� ���� �ߵ� ���� Ȯ�� �� ���� �Լ�
    public bool IsGenereShield()
    { 
        return isGenereShieldActive;
    }
    public void IsGenereShield(bool s)
    {
        isGenereShieldActive = s;
    }
    //�ν�Ʈ ���� �ߵ� ���� Ȯ�� �� ���� �Լ�
    public bool IsBoostShield()
    {
        return isBoostShieldActive;
    }
    public void IsBoostShield(bool s)
    {
        isBoostShieldActive = s;
    }
    //���� ���� ������ Ȯ�� �� ���� �Լ�
    public float CurrentShieldSize()
    {
        return currentShieldSize;
    }
    public void CurrentShieldSize(float c)
    {
        currentShieldSize = c;
    }
    //�ν�Ʈ ����� �غ� Ȯ�� �� ���� �Լ�
    public bool IsBoostReady()
    {
        return isBoostReady;
    }
    public void IsBoostReady(bool ar)
    {
        isBoostReady = ar;
    }
}
