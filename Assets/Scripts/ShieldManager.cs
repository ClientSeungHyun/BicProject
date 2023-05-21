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

    //에너지 감소 및 회복
    public void PlayerEgManage()   
    {
        if (isGenereShieldActive == true)
            player.PlayerEg(player.PlayerEg() - player.ShieldEnergyConsumption() * Time.deltaTime);
        else if (player.PlayerEg() < 100 && isBoostShieldActive == false) // 에너지 까이면 지속적으로 채우기
            player.PlayerEg(player.PlayerEg() + 3.0f * Time.deltaTime);
        else if (player.PlayerEg() >= 100) //100을 넘겼을시 그냥 100으로 고정
            player.PlayerEg(100f);

    }
    //일반 쉴드 기능
    public void GenereShield() 
    {
        if (isGenereShieldActive == true)
        {
            // 쉴드의 크기를 증가시키고, 최대 크기 지정
            shieldGrowSpeed = 10;
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, maxShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
        }
    }
    //부스트 쉴드 기능
    public void StartBoostShield() 
    {
        float startShieldSize = maxShieldSize + 1.0f;

        
        if (isBoostShieldActive == true && isBoostReady == false)
        {
            shieldGrowSpeed = 20;
            
            //쉴드를 전개
            currentShieldSize = Mathf.Min(currentShieldSize + shieldGrowSpeed * Time.deltaTime, startShieldSize);
            transform.localScale = new Vector3(currentShieldSize, currentShieldSize, currentShieldSize);
            
        }
       
    }
    //부스트 쉴드 종료시점 기능 
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
            // 적 오브젝트를 쉴드 방향으로 밀어냄
            Vector3 pushDirection = other.transform.position - transform.position;
            if(isBoostShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
            if (isGenereShieldActive == true)
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * shieldPushForce * 3, ForceMode.Impulse);
        }
    }

    //초기화 함수
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

    //일반 쉴드 발동 여부 확인 및 변경 함수
    public bool IsGenereShield()
    { 
        return isGenereShieldActive;
    }
    public void IsGenereShield(bool s)
    {
        isGenereShieldActive = s;
    }
    //부스트 쉴드 발동 여부 확인 및 변경 함수
    public bool IsBoostShield()
    {
        return isBoostShieldActive;
    }
    public void IsBoostShield(bool s)
    {
        isBoostShieldActive = s;
    }
    //현재 쉴드 사이즈 확인 및 변경 함수
    public float CurrentShieldSize()
    {
        return currentShieldSize;
    }
    public void CurrentShieldSize(float c)
    {
        currentShieldSize = c;
    }
    //부스트 기능이 준비 확인 및 변경 함수
    public bool IsBoostReady()
    {
        return isBoostReady;
    }
    public void IsBoostReady(bool ar)
    {
        isBoostReady = ar;
    }
}
