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
    private bool isMoveAble;
    [SerializeField] private bool isHaveWeapon;

    public GameObject playerCamera;
    public GameObject UIM;
    [SerializeField] private UIManager UIManagerScript;
    public ShieldManager shieldScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public Rigidbody playerRigidbody;  //�����ٵ�

    public float dashSpeed = 10f;
    public Vector3 dashDirection;
    private bool isDashing = false;

    private Vector3 initPosition;
    //�ִϸ����Ϳ� ������
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

        //�غ��ڼ� �ִϸ��̼� ����� �����ٸ� ���� true�� 
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
        //ü�� ����
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWeapon")
        {
            if (playerHP > 0)
            {
                playerHP--;
                UIManagerScript.healths[playerHP].gameObject.SetActive(false);
            }
        }

        //ü�� ȸ��
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

        //�Ϲ� ����
        if ((Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.RawButton.B)) && shieldScript.IsAccelShield() != true)    
        {
            shieldScript.IsGenereShield(!shieldScript.IsGenereShield());
            shieldScript.gameObject.SetActive(true);
        }
        //�׼� ����
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

    //��Ʈ�ѷ� ���̽�ƽ
    void KeyController()
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("adsf");
            StartCoroutine(gunDissolveScript.GenerateGun());
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
