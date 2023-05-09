using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private int maxPlayerHP = 10;  //�ִ� hp
    private int playerHP = 10;   //���� hp
    [SerializeField] private float playerEg = 100.0f; //���� ������
    private int weaponLV = 1;    //���� ����
    private float moveSpeed;     //�̵� �ӵ�
    private float sightAngle; //�þ߰� ����
    private float accelDistance;
    private bool isAccelSystem;
    private bool isMoveAble;
    private bool isHaveWeapon;

    public GameObject playerCamera;
    public GameObject UIM;
    private UIManager UIManagerScript;
    public ShieldManager shieldScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public GameObject accelPoint; // �׼� ��������
    public Rigidbody playerRigidbody;  //�����ٵ�

    //�ִϸ����Ϳ� ������
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

        //�غ��ڼ� �ִϸ��̼� ����� �����ٸ� ���� true�� 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouching") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && shieldScript.IsAccelReady() != true)
        {
            shieldScript.IsAccelReady(true);
        }
        if (shieldScript.IsAccelReady() && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {
            Debug.Log("ADSF");
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
        //ü�� ����
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeapon")
        {
            if (playerHP > 0)
            {
                playerHP--;
                UIManagerScript.healths[playerHP].gameObject.SetActive(false);
            }
        }

        //ü�� ȸ��
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

        //�Ϲ� ����
        if (Input.GetKeyDown(KeyCode.B) && shieldScript.IsAccelShield() != true)    
        {
            shieldScript.IsGenereShield(!shieldScript.IsGenereShield());
            shieldScript.gameObject.SetActive(true);
        }
        //�׼� ����
        if (Input.GetKeyDown(KeyCode.N) && playerEg >= 50.0f) 
        {
            if (shieldScript.IsGenereShield() == true)
                shieldScript.IsGenereShield(false);

            shieldScript.IsAccelShield(true);
            shieldScript.gameObject.SetActive(true);
            AccelSystem();
        }

        shieldScript.GenereShield();
        shieldScript.StartAccelShield();
       

        //���� ���󺹱�
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

    private void AccelSystem()
    {
        if (shieldScript.IsAccelShield())
        {
            playerEg -= 50.0f;
            isAccelSystem = true;
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * accelDistance);
            RaycastHit[] hitDatas;
            hitDatas = Physics.RaycastAll(playerCamera.transform.position, playerCamera.transform.forward, accelDistance);

            //����� ���̵� �߿� �������� ���ϴ� ��ֹ��� �ִ��� �˻�
            //�׼� ���� ���� ����
            for (int i = 0; i < hitDatas.Length; i++)
            {
                //hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall")
                RaycastHit hit = hitDatas[i];
                if (hit.transform.CompareTag("Wall"))   //���� �������� ���ϴ� ��ֹ��� �ִٸ�
                {
                    //Ray�� �浹�� �������� �׼� ���� ��ġ ����
                    Vector3 hitPosition = hit.point;
                    accelPoint.transform.position = new Vector3(hit.point.x, playerCamera.transform.position.y, hit.point.z);
                }
                else //���ٸ� ���� �� �κ��� �׼� ���� ��ġ ����
                {
                    accelPoint.transform.position = new Vector3(playerCamera.transform.position.x + playerCamera.transform.forward.x * accelDistance,
                        playerCamera.transform.position.y, playerCamera.transform.position.z + playerCamera.transform.forward.z * accelDistance);
                }
            }
        }
    }

    //��Ʈ�ѷ� ���̽�ƽ
    void Move()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))  //�޼� Ʈ����
        {
            if (isHaveWeapon)
            {

            }
            else
            {
                isHaveWeapon = true;
                //�� ����
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))  //������ Ʈ���� ��ư
        {
            if (isHaveWeapon)
            {

            }
            else
            {
                isHaveWeapon = true;
                //�� ����
            }
        }

        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (thumbstick.x < 0) //����
            {

            }
            else if (thumbstick.x > 0) //������
            {

            }
            else if (thumbstick.y > 0) // ��
            {

            }
            else if(thumbstick.y<0) //�Ʒ�
            {


            }
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z);
        //�ӽù��� pc �̵�
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

           

    }

    //Ÿ���� �þ� ���� �ִ°�??
    bool IsTargetInSight()
    {

        ////Ÿ���� ���� 
        //Vector3 targetDir = (AccelPosition[0].transform.position - transform.position).normalized; //ũ�Ⱑ 1�� ���ͷ� ���� -> ��ְ�
        //float dot = Vector3.Dot(transform.forward, targetDir);  //���� -> |a||b|cos@ - |a||b| = 1(���� ����)

        ////������ �̿��� �� ����ϱ�
        ////thetha = cos^-1( a dot b / |a||b|)
        ////Mathf.Rad2Deg�� �̿��Ͽ� ���� ���� ������ ��ȯ
        //float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        ////Debug.Log("Ÿ�ٰ� AI�� ���� : " + theta);
        //if (theta <= sightAngle) return true;   //�þ߰� ���ο� ����
        //else return false;


        return false;

    }

    void AnimatorControl()
    {
        //�ӵ� ���
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;

        //���� �ӵ�
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        //�ִϸ��̼� ����
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
        accelDistance = 10.0f;
        isAccelSystem = false;
        isMoveAble = true;
        isHaveWeapon = false;
        shieldScript.gameObject.SetActive(false);

        animator = GameObject.FindGameObjectWithTag("Character").GetComponent<Animator>();
        vrRig = GameObject.FindGameObjectWithTag("Character").GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
    }

    public void GunLevelUp()
    {
        if (weaponLV >= 3) //�ִ� 3���� ����
        {
            weaponLV = 3;
        }
        else //�ִ� ���� �ƴ� �� ����
        {
            weaponLV++;
        }
    }
    public void HPLevelUp()
    {
        if (maxPlayerHP >= 160) //�ִ� 3���� ����
        {
            maxPlayerHP = 160;
        }
        else //�ִ� ���� �ƴ� �� ����
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
