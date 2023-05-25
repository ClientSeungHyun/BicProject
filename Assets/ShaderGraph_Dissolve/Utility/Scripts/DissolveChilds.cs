using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveChilds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] Renderer[] gunrenderer;
    [SerializeField] GameObject[] gunObject;

    public float dissoloveValue;    //0이면 나타나고 1이면 사라짐
    float generateDuration = 1f; // 증가에 걸리는 시간
    float destoryDuration = 1f;
    int weaponLV;
    int leftGun;
    int rightGun;

    bool isGenerate;
    bool isGunLoading;

    PlayerControl playercontrol;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < materials.Count; i++)
        {
                materials[i].SetFloat("_Dissolve", dissoloveValue);
        }
    }

    public IEnumerator GenerateGun()
    {
        float elapsedTime = 0f; //경과 시간
        isGunLoading = true;
        gunObject[rightGun].SetActive(true);
        gunObject[leftGun].SetActive(true);

        while (elapsedTime < generateDuration)
        {
            elapsedTime += Time.deltaTime;
            dissoloveValue = Mathf.Lerp(1f, 0f, elapsedTime / generateDuration);

            yield return null;
        }

        // 최종 값 설정
        dissoloveValue = 0f;
        isGenerate = true;
        isGunLoading = false;
    }


    public IEnumerator DestoryGun()
    {
        float elapsedTime = 0f; //경과 시간
        isGunLoading = true;

        while (elapsedTime < destoryDuration)
        {
            elapsedTime += Time.deltaTime;
            dissoloveValue = Mathf.Lerp(0f, 1f, elapsedTime / destoryDuration);

            yield return null;
        }

        // 최종 값 설정
        dissoloveValue = 1f;
        isGenerate = false;
        isGunLoading = false;
        gunObject[rightGun].SetActive(false);
        gunObject[leftGun].SetActive(false);
    }

    private void Init()
    {
        playercontrol = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        gunObject = new GameObject[transform.childCount];

        dissoloveValue = 1; //없는 상태
        isGenerate = false;
        isGunLoading = false;
        gunrenderer = GetComponentsInChildren<Renderer>();
        weaponLV = GameManagers.playerInfo.WeaponLV();

        //무기 번호 설정
        switch (weaponLV)
        {
            case 1:
                rightGun = 0;
                leftGun = 1;
                break;
            case 2:
                rightGun = 2;
                leftGun = 3;
                break;
            case 3:
                rightGun = 4;
                leftGun = 5;
                break;
        }

        for (int i=0; i<transform.childCount; i++)
        {
            gunObject[i] = transform.GetChild(i).gameObject;
            gunObject[i].SetActive(false);
        }

        materials.AddRange(gunrenderer[rightGun].materials);
        materials.AddRange(gunrenderer[leftGun].materials);
    }


    public bool IsGenerate()
    {
        return isGenerate;
    }
    public void IsGenerate(bool g)
    {
        isGenerate = g;
    }

    public bool IsGunLoading()
    {
        return isGunLoading;
    }
    public void IsGunLoading(bool g)
    {
        isGunLoading = g;
    }
}
