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
    private float moveSpeed;     //�̵� �ӵ�
    private bool isBorder;
    private float sightAngle; //�þ߰� ����

    public GameObject playerCamera;
    public GameObject headPosition;
    public GameObject UIM;
    private UIManager UIManagerScript;

    

    GameObject[] AccelPosition;     //���� ���� ����
    public Rigidbody playerRigidbody;  //�����ٵ�

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
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeapon")
        {
            if (playerHP > 0)
            {
                playerHP--;
                UIManagerScript.healths[playerHP].gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Juice")
        {
            if (playerHP < 10)
            {
                UIManagerScript.healths[playerHP].gameObject.SetActive(true);
                playerHP++;
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

        //�ӽù��� pc �̵�
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        if(!isBorder)
            transform.position += inputDir * moveSpeed * Time.deltaTime;
    }

    void StopWall() //�浹����
    {
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 2.5f, Color.red);
        isBorder = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, 0.5f, LayerMask.GetMask("Wall"));
    }

    //Ÿ���� �þ� ���� �ִ°�??
    bool IsTargetInSight()
    {

        //Ÿ���� ���� 
        Vector3 targetDir = (AccelPosition[0].transform.position - transform.position).normalized; //ũ�Ⱑ 1�� ���ͷ� ���� -> ��ְ�
        float dot = Vector3.Dot(transform.forward, targetDir);  //���� -> |a||b|cos@ - |a||b| = 1(���� ����)

        //������ �̿��� �� ����ϱ�
        //thetha = cos^-1( a dot b / |a||b|)
        //Mathf.Rad2Deg�� �̿��Ͽ� ���� ���� ������ ��ȯ
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Debug.Log("Ÿ�ٰ� AI�� ���� : " + theta);
        if (theta <= sightAngle) return true;   //�þ߰� ���ο� ����
        else return false;


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
        AccelPosition = GameObject.FindGameObjectsWithTag("AccelPosition");
        playerRigidbody = GetComponent<Rigidbody>();

        playerHP = maxPlayerHP;
        moveSpeed = 5.0f;
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
