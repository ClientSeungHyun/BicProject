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
    private int reloadLV;   //źâ ���� �÷��ִ� ����
    private bool isMoveAble;    //�÷��̾ ������ �� �ִ��� ����

    public void Init()
    {
        weaponLV = 1;
        energyLV = 1;
        reloadLV = 1;
        isMoveAble = false;
    }

    public void WeaponLeveUP()
    {
        if (weaponLV <= 3)
            weaponLV++;
        else
            Debug.Log("��ȭ �ִ�ġ �Դϴ�");
    }
    public void EnergyLevelUP()
    {
        if (energyLV <= 3)
            energyLV++;
        else
            Debug.Log("��ȭ �ִ�ġ �Դϴ�");
    }
    public void ReloadLevelUP()
    {
        if (reloadLV <= 3)
            reloadLV++;
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
    public int ReloadLV()
    {
        return reloadLV;
    }

}

public class GameManagers : MonoBehaviour
{
    public PlayerInfor playerInfo;
    [SerializeField] SceneManagers sceneManagerScript;
    [SerializeField] StoryScript storyScript;
    [SerializeField] OVRPlayerController ovrPlayerControl;
    [SerializeField] MonsterManager monsterManagerScript;

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerControl playerScript;
    public int chooseCard;
    public float subSpeed = 0.0f; //�ڸ� �ӵ�
    [SerializeField] private string sceneName;   //���̸��� �޾ƿ� ���� � ������ Ȯ���ϱ� ���� ����
    [SerializeField] private bool isPlaying;     //�÷��̰� ���� ������ Ȯ��
    private bool isStageClear;  //���������� Ŭ���� �Ƴ� Ȯ���ϴ� ����

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
        UpgradeStatus();
        SettingGame();
        if(monsterManagerScript)
            isStageClear = monsterManagerScript.isStageClear;
        if (storyScript)
        {
            if (storyScript.IsStoryComplete() && playerScript.IsHaveWeapon() && !playerScript.IsDeath())
                isPlaying = true;   //�� ������ true�� �� ��� �ΰ��� ���� ����(�����ϱ�)
        }
        if (isStageClear)
            isPlaying = false;
    }

    public void UpgradeStatus()
    {
        if (chooseCard == 1 && player)
        {
            playerInfo.ReloadLevelUP();
            chooseCard = 0;
        }
        else if (chooseCard == 2 && player)
        {
            playerInfo.WeaponLeveUP();
            chooseCard = 0;
        }
        else if (chooseCard == 3 && player)
        {
            playerInfo.EnergyLevelUP();
            chooseCard = 0;
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

        DontDestroyOnLoad(gameObject);
        
    }
    
    private void SettingGame()
    {
        if (isPlaying)
        {
            ovrPlayerControl.Acceleration = 0.1f;
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
