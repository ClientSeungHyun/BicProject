using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    float moveSpeed;     //�̵� �ӵ�
    float sightAngle; //�þ߰� ����

    GameObject[] AccelPosition;     //���� ���� ����
    NavMeshAgent playerAgent;   //�׺���̼�
    Rigidbody playerRigidbody;  //�����ٵ�

    public int maxplayerHP = 100;  //�÷��̾� �������� �� ������ �������� ���巹��
    public int playerHP = 100;
    public float playerEg = 100.0f;
    public int weaponLV = 1;
    public int shieldLV = 1;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5.0f;
        sightAngle = 80f;
        playerAgent = GetComponent<NavMeshAgent>();
        AccelPosition = GameObject.FindGameObjectsWithTag("AccelPosition");
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //�׺���̼� ����� �̿��� ����Ʈ �̵�(�������)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 destination = AccelPosition[0].transform.position;
            playerAgent.destination = destination;
            playerEg -= 20; //�̵��� ������ 20����
        }
        Thumb();
        if (playerEg < 100) // ������ ���̸� ���������� ä���
        {
            playerEg += 0.1f;
        }
        else //100�� �Ѱ����� �׳� 100���� ����
        {
            playerEg = 100f;
        }
    }

    //��Ʈ�ѷ� ���̽�ƽ
    void Thumb()
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

        //�ӽù��� pc �̵�
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;

        playerRigidbody.velocity = inputDir * moveSpeed;

        //transform.LookAt(transform.position + inputDir);
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
        if(maxplayerHP >= 160) //�ִ� 3���� ����
        {
            maxplayerHP = 160;
        }
        else //�ִ� ���� �ƴ� �� ����
        {
            maxplayerHP += 20;
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
}
