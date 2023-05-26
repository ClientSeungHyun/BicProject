using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryScript : MonoBehaviour
{
    public string[] stage01Script = {"�� ��������?", "�װ��� ��������� ���ɴ��� ���þ�!!",
    "�ϴ� �� �� Ǯ��� �׳��� ��ü ���� ���� ���� �̵��������!!", "�� ���� ���������� �����״� �����Ұž�!",
    "�׷� ȭ���ϰ� ���ٰ� ��!!!", ".........","����� ��� �ֳİ�?", "�� ��ư�� ������ ���Ⱑ ��ȯ�ǵ��� �ص׾�!!",
    "�� ��� ����� �������� ���� �غ� ������!", "�׷� ����� ����!"};

    public string[] stage02Script = {"���� ù ��° ������ �����ϰ� �ذ��߱���!!", "���� Żȯ ��ȹ�� ���� �����ο��?!",
    "���� ħ���� ��ġ ë���� �̹��� �� ���� �پ��� ��ü�� �������� �����߾�...","�̹����� ����� ����..."};

    public string[] stage03Script = {"�����߾�!!", ".....!!!!????", "�����̾�...", "������ ��������� �����ϴ� ��ü��",
    "�ΰ��� ���¸� ��� ������ �츮�� �������� ��", "������ ��ü�ɷ��� �ʿ� ���ص� �ջ��� ���� �����ڽ�����.",
    "���� ����ġ�� ���̶�� ���� ��", "�ʶ� ���� �ο��� �ǰ�����", "�ʶ�� ������ �Ŷ�� �Ͼ�!!"};

    public string[] stageScript;
    private TextMeshProUGUI textComponent;

    private string fullText;
    private string currentText;

    private float textSpeed = 0.5f;
    private float timer = 0f;

    private int currentIndex;
    private int i;

    private bool isScripting;   //��ũ��Ʈ ���� ��
    private bool isStoryComplete;    //��ȭ ��

    private GameManagers gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        textSpeed = gameManagerScript.subSpeed;

        if (SceneManager.GetActiveScene().name == "Stage01")
            stageScript = stage01Script;
        if (SceneManager.GetActiveScene().name == "Stage02")
            stageScript = stage02Script;
        if (SceneManager.GetActiveScene().name == "Stage03")
            stageScript = stage03Script;

        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = "";
        currentIndex = 0;
        i = 0;
        isScripting = false;
        isStoryComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        TextProceeding();
    }

    public void TextProceeding()
    {
        if ((OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.T)) || i == 0) 
        {
            //��ȭ ��ŵ
            if (isScripting)
            {
                currentText = "";
                currentIndex = 0;
                i++;
            }
            else
            {
                isScripting = true;
            }
        }
        //��ȭ ������
        if (isScripting)
        {
            timer += Time.deltaTime; // ��� �ð� ����

            //��ȭ �� 
            if (stageScript.Length <= i && (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.T))) 
            {
                isStoryComplete = true;
                textComponent.text = "";
            }
            //��ȭ ����
            else if (timer >= textSpeed && currentIndex < stageScript[i].Length)
            {
                currentText += stageScript[i][currentIndex]; // ���� ���� �߰�
                textComponent.text = currentText; // ������� ���� �ؽ�Ʈ ������Ʈ

                currentIndex++; // ���� �ε����� �̵�
                timer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
            //���� ��ȭ 
            else if(currentIndex >= stageScript[i].Length)
            {
                i++;
                isScripting = false;
                currentIndex = 0;
                currentText = "";
            }
            
        }
    }

    public bool IsScripting()
    {
        return isScripting;
    }
    
    public bool IsStoryComplete()
    {
        return isStoryComplete;
    }
}
