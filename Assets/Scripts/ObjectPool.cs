using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // 오브젝트 풀에 저장할 프리팹
    public int size; // 초기 생성할 오브젝트 개수

    public List<GameObject> objectPool = new List<GameObject>(); // 오브젝트 풀 리스트

    private void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(); // 자식 오브젝트들을 가져옴
        int a = transform.childCount; // 자식 오브젝트 개수를 저장

        // 초기 생성된 자식 오브젝트들을 오브젝트 풀에 추가하고 비활성화
        for (int i = 0; i < transform.childCount; i++)
        {
            objectPool.Add(transform.GetChild(i).gameObject); // 오브젝트 풀에 자식 오브젝트 추가
            objectPool[i].gameObject.SetActive(false); // 자식 오브젝트를 비활성화
        }
    }

    public GameObject GetObject(Vector3 startPos)
    {
        // 오브젝트 풀에서 비활성화된 오브젝트를 찾아 활성화하고 반환
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = startPos; // 시작 위치로 이동
                obj.SetActive(true); // 오브젝트 활성화
                return obj; // 오브젝트 반환
            }
        }

        // 오브젝트 풀에 사용 가능한 오브젝트가 없는 경우 새로운 오브젝트 생성
        GameObject newObj = Instantiate(prefab, transform); // 프리팹을 복제하여 새로운 오브젝트 생성
        newObj.SetActive(true); // 새로운 오브젝트 활성화
        objectPool.Add(newObj); // 오브젝트 풀에 새로운 오브젝트 추가
        return newObj; // 새로운 오브젝트 반환
    }
}
