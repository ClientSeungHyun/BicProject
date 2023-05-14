using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Layser : MonoBehaviour
{
    private LineRenderer layser;        // ������
    public Color lineColor;
    private RaycastHit Collided_object; // �浹�� ��ü
    private GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü

    static float raycastDistance = 100f; // ������ ������ ���� �Ÿ�
    public float rayRenderDistance;

    public GameObject Llayser;
    public GameObject Rlayser;

    string sceneName = "";
    // Start is called before the first frame update
    void Start()
    {
        // ��ũ��Ʈ�� ���Ե� ��ü�� ���� ��������� ������Ʈ�� �ְ��ִ�.
        layser = this.gameObject.GetComponent<LineRenderer>();
        // ������ �������� ���� ǥ��
        lineColor = Color.black;
        layser.material.color = lineColor;
        // �������� �������� 2���� �ʿ� �� ���� ������ ��� ǥ�� �� �� �ִ�.
        layser.positionCount = 2;
        // ������ ���� ǥ��
        layser.startWidth = 0.01f;
        layser.endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        layser.SetPosition(0, transform.position); // ù��° ������ ��ġ
                                                   // ������Ʈ�� �־� �����ν�, �÷��̾ �̵��ϸ� �̵��� ���󰡰� �ȴ�.
                                                   //  �� �����(�浹 ������ ����)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            layser.SetPosition(1, Collided_object.point);

            //�浹�� ��ü �̸� Ȯ���� �̵��� �� ����
            if (Collided_object.collider.gameObject.name == "ClickStart")
            {
                sceneName = "StartScene";
            }
            else if (Collided_object.collider.gameObject.name == "ClickOption")
            {
                sceneName = "OptionScene";
            }
            else if (Collided_object.collider.gameObject.name == "ClickExit")
            {
                sceneName = "ExitScene";
            }
            else if (Collided_object.collider.gameObject.name == "ReturnOption")
            {
                sceneName = "TitleScene";
            }
            else if (Collided_object.collider.gameObject.name == "ReturnExit")
            {
                sceneName = "TitleScene";
            }
            else if (Collided_object.collider.gameObject.name == "ReturnStart")
            {
                sceneName = "TitleScene";
            }
            else sceneName = null;

        }

        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            layser.SetPosition(1, transform.position + (transform.forward * rayRenderDistance));
            sceneName = null;
        }


    }

    private void LateUpdate()
    {
        // ��ư�� ���� ��� + ���ÿ� �����°� ����
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            //�ݴ� ������ ��Ȱ��ȭ, �׷��� ������ �ϳ��� �ν�
            Rlayser.SetActive(false);
            //������ �� ����
            lineColor = Color.magenta;
            layser.material.color = lineColor;
            //���̵�
            if (sceneName != null)
                SceneManager.LoadScene(sceneName);

        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Llayser.SetActive(false);
            lineColor = Color.magenta;
            layser.material.color = lineColor;
            if (sceneName != null)
                SceneManager.LoadScene(sceneName);
            if (sceneName != null)
                SceneManager.LoadScene(sceneName);
        }
        
        // ��ư�� �� ���          
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            //�ݴ� ������ Ȱ��ȭ
            Rlayser.SetActive(true);
            //������ �� 
            lineColor = Color.black;
            layser.material.color = lineColor;
            //�ݴ� �������� �� �������־�� ��
            Rlayser.GetComponent<LineRenderer>().material.color = lineColor;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            Llayser.SetActive(true);
            lineColor = Color.black;
            layser.material.color = lineColor;
            Llayser.GetComponent<LineRenderer>().material.color = lineColor;
        }
    }
}
