using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private int maxPlayerHP = 10;  //최대 hp
    private int playerHP = 10;   //현재 hp
    [SerializeField] private float playerEg = 100.0f; //쉴드 에너지
    private int weaponLV = 1;    //무기 레벨
    private float moveSpeed;     //이동 속도
    private float sightAngle; //시야각 범위
    private bool isMoveAble;
    [SerializeField] private bool isHaveWeapon;

    public GameObject playerCamera;
    public GameObject UIM;
    [SerializeField] private UIManager UIManagerScript;
    public ShieldManager shieldScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public Rigidbody playerRigidbody;  //리짓바디

    public float dashSpeed = 10f;
    public Vector3 dashDirection;
    private bool isDashing = false;

    private Vector3 initPosition;
    //애니메이터용 변수들
    public float speedTreshold = 0.001f;
    [Range(0, 1)]
    public float smoothing = 1;
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;

    public DissolveChilds gunDissolveScript;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
        AnimatorControl();
        ShieldSystem();
        KeyController();

        transform.position = new Vector3(transform.position.x, initPosition.y, transform.position.z);

        //준비자세 애니메이션 재생이 끝났다면 레디를 true로 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && shieldScript.IsAccelReady() != true)
        {
            shieldScript.IsAccelReady(true);
            UIManagerScript.BoostOnOff(true);
        }
        if (shieldScript.IsAccelReady() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {

            StartCoroutine(Accel());
            if (!isDashing)
                Invoke("EndAccelShield", 0.7f);
        }
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
        shieldScript.ShieldLVManage();

        //일반 쉴드
        if ((Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.RawButton.B)) && shieldScript.IsAccelShield() != true)    
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
            shieldScript.IsAccelShield(true);
            playerEg -= 50.0f;
        }

        shieldScript.GenereShield();
        shieldScript.StartAccelShield();
       

        //쉴드 원상복구
        if (shieldScript.IsGenereShield() == false && shieldScript.IsAccelShield() == false) 
        {
            shieldScript.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            shieldScript.CurrentShieldSize(1.0f);
            shieldScript.gameObject.SetActive(false);
        }
        if (playerEg <= 0)
        {
            shieldScript.IsAccelShield(false);
            shieldScript.IsGenereShield(false);
            playerEg = 0.0001f;
        }
    }

    private IEnumerator Accel()
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

    private void EndAccelShield()
    {
        shieldScript.EndAccelShield();
        UIManagerScript.BoostOnOff(false);
    }

    //컨트롤러 조이스틱
    void KeyController()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))  //왼손 트리거
        {
            if (isHaveWeapon)
            {

            }
            else
            {
                isHaveWeapon = true;
                //총 생성
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))  //오른손 트리거 버튼
        {
            if (isHaveWeapon)
            {

            }
            else
            {
                isHaveWeapon = true;
                //총 생성
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("adsf");
            StartCoroutine(gunDissolveScript.GenerateGun());
        }


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z);
        //임시방편 pc 이동
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);



    }

    //타겟이 시야 내에 있는가??
    bool IsTargetInSight()
    {

        ////타겟의 방향 
        //Vector3 targetDir = (AccelPosition[0].transform.position - transform.position).normalized; //크기가 1인 벡터로 만듬 -> 노멀값
        //float dot = Vector3.Dot(transform.forward, targetDir);  //내적 -> |a||b|cos@ - |a||b| = 1(생략 가능)

        ////내적을 이용한 각 계산하기
        ////thetha = cos^-1( a dot b / |a||b|)
        ////Mathf.Rad2Deg을 이용하여 라디안 값을 각도로 변환
        //float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        ////Debug.Log("타겟과 AI의 각도 : " + theta);
        //if (theta <= sightAngle) return true;   //시야각 내부에 있음
        //else return false;


        return false;

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

        animator.SetBool("isCrouching", shieldScript.IsAccelShield());
        animator.SetBool("isSprint", shieldScript.IsAccelReady());


       
    }

    public void Init()
    {
        UIManagerScript = UIM.GetComponent<UIManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerHP = maxPlayerHP;
        moveSpeed = 3.0f;
        sightAngle = 80f;
        isMoveAble = true;
        isHaveWeapon = false;
        shieldScript.gameObject.SetActive(false);

        animator = GameObject.FindGameObjectWithTag("Character").GetComponent<Animator>();
        vrRig = GameObject.FindGameObjectWithTag("Character").GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
    }

    public void GunLevelUp()
    {
        if (weaponLV >= 3) //최대 3레벨 제한
        {
            weaponLV = 3;
        }
        else //최대 레벨 아닐 시 렙업
        {
            weaponLV++;
        }
    }
    public void HPLevelUp()
    {
        if (maxPlayerHP >= 160) //최대 3레벨 제한
        {
            maxPlayerHP = 160;
        }
        else //최대 레벨 아닐 시 렙업
        {
            maxPlayerHP += 20;
        }
    }
    public void EnergyLevelUp()
    {

    }

    public float PlayerEg()
    {
        return playerEg;
    }
    public void PlayerEg(float e)
    {
        playerEg = e;
    }
}
