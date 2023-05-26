using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private float playerEg = 100.0f; //���� ������
    private float shieldEnergyConsumption;  // ���� ������ �Ҹ�
    private float boostEnergyConsumption;   //�ν�Ʈ ������ �Ҹ�
    private float moveSpeed;     //�̵� �ӵ�
    private float sightAngle; //�þ߰� ����
    
    private bool isHaveWeapon; //���Ⱑ ��ȯ�Ƴ�?
    private bool isDeath;
    private bool isPlaying;
    private bool isStageClear;

    public GameObject playerCamera;
    private Rigidbody playerRigidbody;  //�����ٵ�
    private CharacterController characterController;

    //�뽬 ��� ����
    private float dashSpeed = 10f;
    private Vector3 dashDirection;
    private bool isDashing = false;

    private Vector3 initPosition;   //y�̵� ������ ���� �ʱ� ��ġ �޴� ����

    //�ִϸ����Ϳ� ����
    private float speedTreshold = 0.001f;
    [Range(0, 1)]
    private float smoothing = 1;
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;


    public ShieldManager shieldScript;          //���� ���� ��ũ��Ʈ
    public DissolveChilds weaponDissolveScript;    //�� ��ȯ �� ����� ��ũ��Ʈ
    private UIManager UIManagerScript;          //UI ���� ��ũ��Ʈ
    private GameManagers gameManagerScript;    //���� �Ŵ��� ��ũ��Ʈ
    private StoryScript storyScrpit;

    private Vector3 moveDirection;  //�ӽù��� �̵���

    private int maxPlayerHP = 10;  //�ִ� hp
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

        //�Ϲ� ����
        if ((Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.RawButton.B)) && shieldScript.IsBoostShield() != true)    
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
            shieldScript.IsBoostShield(true);
            playerEg -= boostEnergyConsumption;
        }

        shieldScript.GenereShield();
        shieldScript.StartBoostShield();
       

        //���� ���󺹱�
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

    //��Ʈ�ѷ� ���̽�ƽ
    void GunOnOFF()
    {
        //�� ��Ÿ���� �ڵ�
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

        animator.SetBool("isCrouching", shieldScript.IsBoostShield());
        animator.SetBool("isSprint", shieldScript.IsBoostReady());

        transform.position = new Vector3(transform.position.x, initPosition.y, transform.position.z);

        //�غ��ڼ� �ִϸ��̼� ����� �����ٸ� ���� true�� 
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
        //������ ���� ������ �Ҹ�
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

    //�÷��̾� ������ ��Ȳ ���� �Լ�
    public float PlayerEg()
    {
        return playerEg;
    }
    public void PlayerEg(float e)
    {
        playerEg = e;
    }

    //���� �Ҹ� ���� �Լ�
    public float ShieldEnergyConsumption()
    {
        return shieldEnergyConsumption;
    }
    //�ν�Ʈ �Ҹ� ���� �Լ�
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
