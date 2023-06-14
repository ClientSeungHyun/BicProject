using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


/*
 * ���� ���� ����
 * ���丮 ������ �������� isScripting
 * ���� ��ȯ�Ǿ���� playercontrol��ũ��Ʈ�� isHaveWeapon
 */

public class PlayerInfor    //����ؼ� �÷��̾�� ���޵Ǿ�� �� ������ ������ Ŭ����
{
    private int weaponLV;    //���� ����
    private int energyLV;    //�������Ҹ� �����ϴ� ����
    private int hpLV;   //źâ ���� �÷��ִ� ����
    private bool isMoveAble;    //�÷��̾ ������ �� �ִ��� ����

    public void Init()
    {
        weaponLV = 1;
        energyLV = 1;
        hpLV = 1;
        isMoveAble = false;
    }

    public void WeaponLeveUP(string name)
    {
        if (weaponLV < 3)
        {
            weaponLV++;
            LoadingSceneManager.LoadScene(name);
        }
        else
            Debug.Log("��ȭ �ִ�ġ �Դϴ�");
    }
    public void EnergyLevelUP(string name)
    {
        if (energyLV < 3)
        {
            energyLV++;
            LoadingSceneManager.LoadScene(name);
        }
        else
            Debug.Log("��ȭ �ִ�ġ �Դϴ�");
    }
    public void HPLevelUP(string name)
    {
        if (hpLV < 3)
        {
            hpLV++;
            LoadingSceneManager.LoadScene(name);
        }
        else
            Debug.Log("��ȭ �ִ�ġ �Դϴ�");
    }

    public int WeaponLV()
    {
        return weaponLV;
    }
    public int EnergyLV()
    {
        return energyLV;
    }
    public int HPLV()
    {
        return hpLV;
    }

}

public class GameManagers : MonoBehaviour
{
    public PlayerInfor playerInfo;
    SceneManagers sceneManagerScript;
    StoryScript storyScript;
    OVRPlayerController ovrPlayerControl;
    MonsterManager monsterManagerScript;

    private GameObject player;
    private PlayerControl playerScript;
    public float subSpeed = 0.0f; //�ڸ� �ӵ�
    private string sceneName;   //���̸��� �޾ƿ� ���� � ������ Ȯ���ϱ� ���� ����
    private bool isPlaying;     //�÷��̰� ���� ������ Ȯ��
    private bool isStageClear;  //���������� Ŭ���� �Ƴ� Ȯ���ϴ� ����
    public bool isBossDead;

    //���� ����� ������ ȣ��Ǵ� 3�Լ���
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  //��������Ʈ ü�� �߰�
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(GameObject.Find("Story"))
            storyScript = GameObject.Find("Story").GetComponent<StoryScript>();
        if(GameObject.Find("MonsterManage"))
            monsterManagerScript = GameObject.Find("MonsterManage").GetComponent<MonsterManager>();
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerControl>();
            ovrPlayerControl = player.GetComponent<OVRPlayerController>();
        }
        isPlaying = false;
        isStageClear = false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  //��������Ʈ ü�� ����;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //�ʱ�ȭ �Լ�
        Init();
    }

    private void Update()
    {
        SettingGame();
        if(monsterManagerScript)
            isStageClear = monsterManagerScript.isStageClear;
        if (storyScript)
        {
            if (storyScript.IsStoryComplete() && playerScript.IsHaveWeapon() && !playerScript.IsDeath())
                isPlaying = true;   //�� ������ true�� �� ��� �ΰ��� ���� ����(�����ϱ�)
        }
        if (isStageClear)
        {
            isPlaying = false;
            ovrPlayerControl.Acceleration = 0;

        }
        if (playerScript.IsDeath())
        {
            ovrPlayerControl.Acceleration = 0;
        }
    }

    //�ʱ� ���� �Լ�
    private void Init()
    {
        playerInfo = new PlayerInfor();
        playerInfo.Init();
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagers>();
        isPlaying = false;
        isStageClear = false;
        isBossDead = false;
        DontDestroyOnLoad(gameObject);
        
    }
    
    private void SettingGame()
    {
        if (isPlaying)
        {
            ovrPlayerControl.Acceleration =  0.1f;
        }
        else
        {
            ovrPlayerControl.Acceleration = 0;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }
    public bool IsStageClear()
    {
        return isStageClear;
    }
}
