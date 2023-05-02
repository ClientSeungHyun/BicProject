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
    private float accelDistance;
    private bool isAccelSystem;
    private bool isMoveAble;

    public GameObject playerCamera;
    public GameObject UIM;
    private UIManager UIManagerScript;
    public ShieldManager shieldScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public GameObject accelPoint; // 액셀 도착지점
    public Rigidbody playerRigidbody;  //리짓바디

    //애니메이터용 변수들
    public float speedTreshold = 0.001f;
    [Range(0, 1)]
    public float smoothing = 1;
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControl();
        ShieldSystem();
        Move();


        //준비자세 애니메이션 재생이 끝났다면 레디를 true로 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && shieldScript.IsAccelReady() != true) 
        {
            shieldScript.IsAccelReady(true);
        }
        if (shieldScript.IsAccelReady() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {
            Vector3 z = Vector3.zero;
            Vector3 point = new Vector3(accelPoint.transform.position.x, transform.position.y, accelPoint.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, point, 1.5f);
            if (transform.position == point)
            {
                shieldScript.EndAccelShield();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //체력 까임
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeapon")
        {
            if (playerHP > 0)
            {
                playerHP--;
                UIManagerScript.healths[playerHP].gameObject.SetActive(false);
            }
        }

        //체력 회복
        if (collision.gameObject.tag == "Juice")
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

        if (Input.GetKeyDown(KeyCode.B) && shieldScript.IsAccelShield() != true)
        {
            shieldScript.IsGenereShield(!shieldScript.IsGenereShield());
        }
        if (Input.GetKeyDown(KeyCode.N) && playerEg >= 50.0f) 
        {
            if (shieldScript.IsGenereShield() == true)
                shieldScript.IsGenereShield(false);

            shieldScript.IsAccelShield(true);

            AccelSystem();
        }

        shieldScript.GenereShield();
        shieldScript.StartAccelShield();
       

        //쉴드 원상복구
        if (shieldScript.IsGenereShield() == false && shieldScript.IsAccelShield() == false) 
        {
            shieldScript.transform.localScale = new Vector3(1, 1, 1);
            shieldScript.CurrentShieldSize(1.0f);
        }
        if (playerEg <= 0)
        {
            shieldScript.IsAccelShield(false);
            shieldScript.IsGenereShield(false);
            playerEg = 0.0001f;
        }
    }

    private void AccelSystem()
    {
        if (shieldScript.IsAccelShield())
        {
            playerEg -= 50.0f;
            isAccelSystem = true;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * accelDistance);
            RaycastHit[] hitDatas;
            hitDatas = Physics.RaycastAll(playerCamera.transform.position, playerCamera.transform.forward, accelDistance);

            //통과한 레이들 중에 지나가지 못하는 장애물이 있는지 검사
            //액셀 종료 지점 설정
            for (int i = 0; i < hitDatas.Length; i++)
            {
                //hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall")
                RaycastHit hit = hitDatas[i];
                if (hit.transform.CompareTag("Wall"))   //만약 지나가지 못하는 장애물이 있다면
                {
                    //Ray가 충돌한 지점으로 액셀 종료 위치 지정
                    Vector3 hitPosition = hit.point;
                    accelPoint.transform.position = new Vector3(hit.point.x, playerCamera.transform.position.y, hit.point.z);
                }
                else //없다면 레이 끝 부분이 액셀 종료 위치 지정
                {
                    accelPoint.transform.position = new Vector3(playerCamera.transform.position.x + playerCamera.transform.forward.x * accelDistance,
                        playerCamera.transform.position.y, playerCamera.transform.position.z + playerCamera.transform.forward.z * accelDistance);
                }
            }

           
        }
    }

    //컨트롤러 조이스틱
    void Move()
    {
        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (thumbstick.x < 0) //왼쪽
            {

            }
            else if (thumbstick.x > 0) //오른쪽
            {

            }
            else if (thumbstick.y > 0) // 위
            {

            }
            else if(thumbstick.y<0) //아래
            {


            }
        }

        //if(Input.GetKey(KeyCode.W))
        //    transform.Translate(Vector3.forward * moveSpeed);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z);
        //임시방편 pc 이동
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        

        //if (!isBorder)
           

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

    //void PlayerEgManage()
    //{
    //    if (shieldScript.IsGenereShield() == true)
    //        playerEg = PlayerEg() - 10.0f * Time.deltaTime;
    //    else if (playerEg < 100 && shieldScript.IsAccelShield() == false) // 에너지 까이면 지속적으로 채우기
    //        playerEg = playerEg + 3.0f * Time.deltaTime;
    //    else if (playerEg >= 100) //100을 넘겼을시 그냥 100으로 고정
    //        playerEg = 100f;

    //}

    public void Init()
    {
        UIManagerScript = UIM.GetComponent<UIManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerHP = maxPlayerHP;
        moveSpeed = 3.0f;
        sightAngle = 80f;
        accelDistance = 10.0f;
        isAccelSystem = false;
        isMoveAble = true;


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

    public float PlayerEg()
    {
        return playerEg;
    }
    public void PlayerEg(float e)
    {
        playerEg = e;
    }

    public bool IsAccelSystem()
    {
        return isAccelSystem;
    }
    public void IsAccelSystem(bool a)
    {
        isAccelSystem = a;
    }
}
