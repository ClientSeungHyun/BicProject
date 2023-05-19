using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveChilds : MonoBehaviour
{
    // Start is called before the first frame update
    List<Material> materials = new List<Material>();

    public float dissoloveValue;    //0이면 나타나고 1이면 사라짐
    float generateDuration = 1f; // 증가에 걸리는 시간
    float destoryDuration = 0.1f;

    bool isGenerate;
    void Start()
    {
        dissoloveValue = 1; //없는 상태
        isGenerate = false;
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
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

        while (elapsedTime < generateDuration)
        {
            elapsedTime += Time.deltaTime;
            dissoloveValue = Mathf.Lerp(1f, 0f, elapsedTime / generateDuration);

            yield return null;
        }

        // 최종 값 설정
        dissoloveValue = 0f;
        isGenerate = true;
    }


    public IEnumerator DestoryGun()
    {
        float elapsedTime = 0f; //경과 시간

        while (elapsedTime < destoryDuration)
        {
            elapsedTime += Time.deltaTime;
            dissoloveValue = Mathf.Lerp(0f, 1f, elapsedTime / destoryDuration);

            yield return null;
        }

        // 최종 값 설정
        dissoloveValue = 1f;
        isGenerate = false;
    }


    public bool IsGenerate()
    {
        return isGenerate;
    }
    public void IsGenerate(bool g)
    {
        isGenerate = g;
    }
}
