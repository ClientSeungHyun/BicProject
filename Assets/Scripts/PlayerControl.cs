using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private int maxPlayerHP = 10;  //최대 hp
    private int playerHP = 10;   //현재 hp
    [SerializeField] private float playerEg = 100.0f; //쉴드 에너지
    private int weaponLV = 1;    //무기 레벨
    private int shieldLV = 1;    //쉴듸 레벨
    [SerializeField] private float moveSpeed;     //이동 속도
    private bool isBorder;
    private float sightAngle; //시야각 범위

    public GameObject playerCamera;
    public GameObject UIM;
    private UIManager UIManagerScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    

    GameObject AccelPosition;     //엑셀 도착 지점
    public Rigidbody playerRigidbody;  //리짓바디

    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {

        StopWall();
        Move();
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

    void StopWall() //충돌설정
    {
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 2.5f, Color.red);
        isBorder = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, 0.3f, LayerMask.GetMask("Wall"));
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
        if(maxPlayerHP >= 160) //최대 3레벨 제한
        {
            maxPlayerHP = 160;
        }
        else //최대 레벨 아닐 시 렙업
        {
            maxPlayerHP += 20;
        }
    }
    public void ShieldLevelUp()
    {
        if (shieldLV >= 3) //최대 3레벨 제한
        {
            shieldLV = 3;
        }
        else //최대 레벨 아닐 시 렙업
        {
            shieldLV++;
        }
    }

    public void Init()
    {
        UIManagerScript = UIM.GetComponent<UIManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerHP = maxPlayerHP;
        moveSpeed = 3.0f;
        sightAngle = 80f;
    }

    public int ShieldLV()
    {
        return shieldLV;
    }
    public void ShieldLV(int s)
    {
        shieldLV = s;
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
