using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryScript : MonoBehaviour
{
    public string[] stage01Script = {"잘 도착했지?", "그곳이 고물덩어리에게 점령당한 도시야!!",
    "일단 몸 좀 풀라고 그나마 개체 수가 적은 곳에 이동시켜줬어!!", "이 구역 정리정도야 너한테는 간단할거야!",
    "그럼 화려하게 날뛰고 와!!!", ".........","무기는 어디 있냐고?", "이 버튼을 누르면 무기가 소환되도록 해뒀어!!",
    "고물 덩어리 놈들이 몰려오기 전에 준비를 끝내둬!", "그럼 행운을 빌게!"};

    public string[] stage02Script = {"역시 첫 번째 구역은 간단하게 해결했구나!!", "도시 탈환 계획은 아주 순조로운걸?!",
    "너의 침입을 눈치 챘는지 이번한 더 많고 다양한 기체를 보내오기 시작했어...","이번에도 행운을 빌게..."};

    public string[] stage03Script = {"수고했어!!", ".....!!!!????", "저놈이야...", "저놈이 고물덩어리들을 조종하는 개체야",
    "인간의 형태를 띄고 있지만 우리를 따라했을 뿐", "하지만 신체능력은 너와 비교해도 손색이 없을 괴물자식이지.",
    "저놈만 물리치면 끝이라고 봐도 돼", "너라도 힘든 싸움이 되겠지만", "너라면 가능할 거라고 믿어!!"};

    public string[] stageScript;
    private TextMeshProUGUI textComponent;

    private string fullText;
    private string currentText;

    private float textSpeed = 0.5f;
    private float timer = 0f;

    private int currentIndex;
    private int i;

    private bool isScripting;   //스크립트 진행 중
    private bool isStoryComplete;    //대화 끝

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
            //대화 스킵
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
        //대화 진행중
        if (isScripting)
        {
            timer += Time.deltaTime; // 경과 시간 측정

            //대화 끝 
            if (stageScript.Length <= i && (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.T))) 
            {
                isStoryComplete = true;
                textComponent.text = "";
            }
            //대화 진행
            else if (timer >= textSpeed && currentIndex < stageScript[i].Length)
            {
                currentText += stageScript[i][currentIndex]; // 다음 글자 추가
                textComponent.text = currentText; // 현재까지 나온 텍스트 업데이트

                currentIndex++; // 다음 인덱스로 이동
                timer = 0f; // 타이머 초기화
            }
            //다음 대화 
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
