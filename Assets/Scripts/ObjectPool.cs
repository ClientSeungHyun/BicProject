using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int size;

    public List<GameObject> objectPool = new List<GameObject>();

    private void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        int a = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            objectPool.Add(transform.GetChild(i).gameObject);
            objectPool[i].gameObject.SetActive(false);
        }
    }

    public GameObject GetObject(Vector3 startPos)
    {
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = startPos;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(true);
        objectPool.Add(newObj);
        return newObj;
    }
}
