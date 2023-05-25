using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


/*
 * 게임 시작 조건
 * 스토리 진행이 끝나야함 isScripting
 * 총이 소환되어야함 playercontrol스크립트에 isHaveWeapon
 */

public class PlayerInfor    //계속해서 플레이어에게 전달되어야 할 정보를 저장할 클래스
{
    private int weaponLV;    //무기 레벨
    private int energyLV;    //에너지소모량 결정하는 레벨
    private int reloadLV;   //탄창 수를 늘려주는 레벨
    private bool isMoveAble;    //플레이어가 움직일 수 있는지 변수

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
            Debug.Log("강화 최대치 입니다");
    }
    public void EnergyLevelUP()
    {
        if (energyLV <= 3)
            energyLV++;
        else
            Debug.Log("강화 최대치 입니다");
    }
    public void ReloadLevelUP()
    {
        if (reloadLV <= 3)
            reloadLV++;
        else
            Debug.Log("강화 최대치 입니다");
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
    SceneManagers sceneManagerScript;
    StoryScript storyScript;
    OVRPlayerController ovrPlayerControl;

    private GameObject player;
    private PlayerControl playerScript;
    public int chooseCard;
    public float subSpeed = 0.0f; //자막 속도
    private string sceneName;   //씬이름을 받아와 현재 어떤 씬인지 확인하기 위한 변수
    [SerializeField] private bool isPlaying;     //플레이가 진행 중인지 확인
    private bool isStageClear;  //스테이지가 클리어 됐나 확인하는 변수

    //씬이 변경될 때마다 호출되는 3함수들
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  //델리게이트 체인 추가
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        storyScript = GameObject.FindGameObjectWithTag("StoryScript").GetComponent<StoryScript>();
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerControl>();
            ovrPlayerControl = player.GetComponent<OVRPlayerController>();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  //델리게이트 체인 제거;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //초기화 함수
        Init();
    }

    private void Update()
    {
        UpgradeStatus();

        if (storyScript.IsStoryComplete() && playerScript.IsHaveWeapon())
            isPlaying = true;   //이 변수가 true일 때 모든 인게임 동작 실행(조정하기)
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

    //초기 설정 함수
    private void Init()
    {
        playerInfo = new PlayerInfor();
        playerInfo.Init();
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagers>();
        isPlaying = false;

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
}
