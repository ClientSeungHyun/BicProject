using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int size;

    private List<GameObject> objectPool = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            objectPool[i].SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
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
