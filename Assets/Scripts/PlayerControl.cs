using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    private int maxPlayerHP = 10;  //�ִ� hp
    private int playerHP = 10;   //���� hp
    [SerializeField] private float playerEg = 100.0f; //���� ������
    private int weaponLV = 1;    //���� ����
    private int shieldLV = 1;    //���� ����
    [SerializeField] private float moveSpeed;     //�̵� �ӵ�
    private float sightAngle; //�þ߰� ����
    private float accelDistance;
    private bool isAccelWall;
    private bool isMoveAble;

    public GameObject playerCamera;
    public GameObject UIM;
    private UIManager UIManagerScript;
    private CharacterController characterController;
    private Vector3 moveDirection;
    public GameObject accelPoint; // �׼� ��������
    public Rigidbody playerRigidbody;  //�����ٵ�

    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {
        AccelSystem();
        Move();
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

    private void AccelSystem()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * accelDistance);
            RaycastHit[] hitDatas;
            hitDatas = Physics.RaycastAll(playerCamera.transform.position, playerCamera.transform.forward, accelDistance);

            //����� ���̵� �߿� �������� ���ϴ� ��ֹ��� �ִ��� �˻�
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

        //if(Input.GetKey(KeyCode.W))
        //    transform.Translate(Vector3.forward * moveSpeed);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z);
        //�ӽù��� pc �̵�
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        

        //if (!isBorder)
           

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
        if(maxPlayerHP >= 160) //�ִ� 3���� ����
        {
            maxPlayerHP = 160;
        }
        else //�ִ� ���� �ƴ� �� ����
        {
            maxPlayerHP += 20;
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

    public void Init()
    {
        UIManagerScript = UIM.GetComponent<UIManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        playerHP = maxPlayerHP;
        moveSpeed = 3.0f;
        sightAngle = 80f;
        accelDistance = 10.0f;
        isMoveAble = true;
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
