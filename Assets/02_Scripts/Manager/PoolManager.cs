using System.Collections.Generic;
using UnityEngine;

// Resources.LoadAll로 가져오다 보니 밑의 변수는 사전 순의로 정렬해서 선언해야함.
public enum PoolType
{
    FieldObject,
    Projectile,
    COUNT
}

public class PoolManager
{
    GameObject[] poolObjectPrefabs;
    Transform poolTransform;

    Dictionary<PoolType, Queue<GameObject>> objectPools = new();

    public void Init(Transform _poolTransform)
    {
        poolTransform = _poolTransform;
        poolObjectPrefabs = Resources.LoadAll<GameObject>("PoolObject");

        if (null == poolObjectPrefabs)
        {
            Debug.LogError("poolObjectPrefabs 로드 실패");
            return;
        }

        for (int i = 0; i < 60; i++)
        {
            CreateObject(PoolType.FieldObject);
        }

        for (int i = 0; i < 30; i++)
        {
            CreateObject(PoolType.Projectile);
        }
    }

    public GameObject CreateObject(PoolType type)
    {
        var newObj = Object.Instantiate(poolObjectPrefabs[(int)type], poolTransform);


        newObj.gameObject.SetActive(false);

        if (!objectPools.ContainsKey(type))
        {
            objectPools[type] = new Queue<GameObject>();
        }
        objectPools[type].Enqueue(newObj);

        return newObj;
    }

    public GameObject GetObject(PoolType type, Transform transform)
    {
        GameObject obj = null;

        if (objectPools.ContainsKey(type) && objectPools[type].Count > 0)
        {
            obj = objectPools[type].Dequeue();
        }
        else
        {

            // TODO 풀에 부족하면 지금은 한 개씩 생성중, 추후에 여러개 씩 미리 생성하도록 하자 -> 현재 풀 갯수의 2배
            obj = CreateObject(type);
        }

        obj.transform.position = transform.position;
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(null);

        return obj;
    }

    public void ReturnFieldObject(PoolType type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolTransform);

        objectPools[type].Enqueue(obj);
    }
}
