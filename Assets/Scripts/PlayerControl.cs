using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private float playerEg = 100.0f; //쉴드 에너지
    private float shieldEnergyConsumption;  // 쉴드 에너지 소모량
    private float boostEnergyConsumption;   //부스트 에너지 소모량
    private float moveSpeed;     //이동 속도
    private float sightAngle; //시야각 범위
    
    private bool isHaveWeapon; //무기가 소환됐나?
    private bool isDeath;
    private bool isPlaying;
    private bool isStageClear;

    public GameObject playerCamera;
    private Rigidbody playerRigidbody;  //리짓바디
    private CharacterController characterController;

    //대쉬 기능 변수
    private float dashSpeed = 10f;
    private Vector3 dashDirection;
    private bool isDashing = false;

    private Vector3 initPosition;   //y이동 고정을 위한 초기 위치 받는 변수

    //애니메이터용 변수
    private float speedTreshold = 0.001f;
    [Range(0, 1)]
    private float smoothing = 1;
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;


    public ShieldManager shieldScript;          //쉴드 관리 스크립트
    public DissolveChilds weaponDissolveScript;    //총 소환 및 사라짐 스크립트
    private UIManager UIManagerScript;          //UI 관리 스크립트
    private GameManagers gameManagerScript;    //게임 매니저 스크립트
    private StoryScript storyScrpit;

    private Vector3 moveDirection;  //임시방편 이동용

    private int maxPlayerHP = 10;  //최대 hp
    public int playerHP = 10;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isHaveWeapon = weaponDissolveScript.IsGenerate();
        isPlaying = gameManagerScript.IsPlaying();
        isStageClear = gameManagerScript.IsStageClear();

        if (playerHP <= 0)
            isDeath = true;
        if (isPlaying)
        {
            AnimatorControl();
            ShieldSystem();
        }
        if (storyScrpit.IsScripting() && !isStageClear)
            GunOnOFF();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        //체력 까임
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWeapon")
        {
            if (playerHP > 0)
            {
                playerHP--;
                UIManagerScript.healths[playerHP].gameObject.SetActive(false);
            }
        }

        //체력 회복
        if (other.gameObject.tag == "Juice")
        {
            if (playerHP < 10)
            {
                UIManagerScript.healths[playerHP].gameObject.SetActive(true);
                playerHP++;
            }
        }
    }


    private void ShieldSystem()
    {
        shieldScript.PlayerEgManage();

        //일반 쉴드
        if ((Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.RawButton.B)) && shieldScript.IsBoostShield() != true)    
        {
            shieldScript.IsGenereShield(!shieldScript.IsGenereShield());
            shieldScript.gameObject.SetActive(true);
        }
        //액셀 쉴드
        if ((Input.GetKeyDown(KeyCode.N) || OVRInput.GetDown(OVRInput.RawButton.A)) && playerEg >= 50.0f) 
        {
            if (shieldScript.IsGenereShield() == true)
                shieldScript.IsGenereShield(false);

            isDashing = true;
            shieldScript.gameObject.SetActive(true);
            shieldScript.IsBoostShield(true);
            playerEg -= boostEnergyConsumption;
        }

        shieldScript.GenereShield();
        shieldScript.StartBoostShield();
       

        //쉴드 원상복구
        if (shieldScript.IsGenereShield() == false && shieldScript.IsBoostShield() == false) 
        {
            shieldScript.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            shieldScript.CurrentShieldSize(1.0f);
            shieldScript.gameObject.SetActive(false);
        }
        if (playerEg <= 0)
        {
            shieldScript.IsBoostShield(false);
            shieldScript.IsGenereShield(false);
            playerEg = 0.0001f;
        }
    }

    private IEnumerator Boost()
    {
        MoveCharacter();
        yield return new WaitForSeconds(0.5f);
        
        
        isDashing = false;
    }

    private void MoveCharacter()
    {
        dashDirection = transform.forward;
        Vector3 movement = dashDirection * dashSpeed * Time.deltaTime;
        characterController.Move(movement);
    }

    private void EndBoostShield()
    {
        shieldScript.EndBoostShield();
        UIManagerScript.BoostOnOff(false);
    }

    //컨트롤러 조이스틱
    void GunOnOFF()
    {
        //총 나타나는 코드
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Z)) 
        {
            if (storyScrpit.IsStoryComplete())
            {
                if (!weaponDissolveScript.IsGenerate() && !weaponDissolveScript.IsGunLoading())
                {
                    StartCoroutine(weaponDissolveScript.GenerateGun());
                }
                else if (weaponDissolveScript.IsGenerate() && !weaponDissolveScript.IsGunLoading())
                {
                    StartCoroutine(weaponDissolveScript.DestoryGun());
                }
            }
        }

    }

    void AnimatorControl()
    {
        //속도 계산
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;

        //지역 속도
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        //애니메이션 설정
        float previousDirectionX = animator.GetFloat("DirectionX");
        float previousDirectionY = animator.GetFloat("DirectionY");

        animator.SetBool("isWalking", headsetLocalSpeed.magnitude > speedTreshold);
        animator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
        animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));

        animator.SetBool("isCrouching", shieldScript.IsBoostShield());
        animator.SetBool("isSprint", shieldScript.IsBoostReady());

        transform.position = new Vector3(transform.position.x, initPosition.y, transform.position.z);

        //준비자세 애니메이션 재생이 끝났다면 레디를 true로 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && shieldScript.IsBoostReady() != true)
        {
            shieldScript.IsBoostReady(true);
            UIManagerScript.BoostOnOff(true);
        }
        if (shieldScript.IsBoostReady() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {

            StartCoroutine(Boost());
            if (!isDashing)
                Invoke("EndBoostShield", 0.7f);
        }

    }

    public void Init()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        characterController = GetComponent<CharacterController>();
        animator = GameObject.FindGameObjectWithTag("Character").GetComponent<Animator>();
        vrRig = GameObject.FindGameObjectWithTag("Character").GetComponent<VRRig>();
        playerRigidbody = GetComponent<Rigidbody>();
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();
        storyScrpit = GameObject.Find("Story").GetComponent<StoryScript>();

        playerHP = maxPlayerHP;
        moveSpeed = 3.0f;
        sightAngle = 80f;
        isHaveWeapon = false;
        isDeath = false;
        shieldScript.gameObject.SetActive(false);
        previousPos = vrRig.head.vrTarget.position;

        PlayerStatus();
    }

    private void PlayerStatus()
    {
        //레벨에 따른 에너지 소모량
        switch (gameManagerScript.playerInfo.EnergyLV())
        {
            case 1:
                shieldEnergyConsumption = 10f;
                boostEnergyConsumption = 50f;
                break;
            case 2:
                shieldEnergyConsumption = 9f;
                boostEnergyConsumption = 45f;
                break;
            case 3:
                shieldEnergyConsumption = 8f;
                boostEnergyConsumption = 40f;
                break;
            default:
                shieldEnergyConsumption = 10f;
                boostEnergyConsumption = 50f;
                break;
        }
    }

    //플레이어 에너지 현황 전달 함수
    public float PlayerEg()
    {
        return playerEg;
    }
    public void PlayerEg(float e)
    {
        playerEg = e;
    }

    //쉴드 소모량 전달 함수
    public float ShieldEnergyConsumption()
    {
        return shieldEnergyConsumption;
    }
    //부스트 소모량 전달 함수
    public float BoostEnergyConsumption()
    {
        return boostEnergyConsumption;
    }

    public bool IsHaveWeapon()
    {
        return isHaveWeapon;
    }

    public bool IsDeath()
    {
        return isDeath;
    }

    public bool IsStageClear()
    {
        return isStageClear;
    }
}
