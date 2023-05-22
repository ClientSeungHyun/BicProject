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
    private bool isPlaying;     //������ ���������� Ȯ���ϴ� ���� true�� ���� ����

    public void Init()
    {
        weaponLV = 1;
        energyLV = 3;
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

    //���� ����
    public void IsPlayStart()
    {
        isPlaying = true;
    }
}

public class GameManagers : MonoBehaviour
{
    public PlayerInfor playerInfo;
    SceneManagers sceneManagerScript;

    private GameObject player;
    private PlayerControl playerScript;
    public int chooseCard;
    public bool activeSubtitile = true;    //�ڸ� Ȱ��ȭ ����
    private string sceneName;   //���̸��� �޾ƿ� ���� � ������ Ȯ���ϱ� ���� ����
    private bool isScripting;   //���丮�� ���������� Ȯ���ϴ� ����
    private bool isStageClear;  //���������� Ŭ���� �Ƴ� Ȯ���ϴ� ����

    //���� ����� ������ ȣ��Ǵ� 3�Լ���
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  //��������Ʈ ü�� �߰�
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerControl>();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  //��������Ʈ ü�� ����;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�ʱ�ȭ �Լ�
        Init();
    }

    private void Update()
    {
        UpgradeStatus();

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
        sceneManagerScript = GameObject.FindGameObjectWithTag("SceneManger").GetComponent<SceneManagers>();

        DontDestroyOnLoad(gameObject);
        
    }

  
}
