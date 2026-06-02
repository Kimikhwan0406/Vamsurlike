using System;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : SingletonBehaviour<PoolManager>
{
    [Serializable]
    public class Pool
    {
        public string poolId;
        public GameObject prefab;
        public int size;
    }

    [SerializeField]
    Pool[] pools;

    Dictionary<string, Queue<GameObject>> poolDictionary = new();
    Dictionary<string, List<GameObject>> activedObjects = new();


    void Start()
    {
        // 미리 생성
        foreach (Pool pool in pools)
        {
            poolDictionary.Add(pool.poolId, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.poolId, pool.prefab);
            }

            // OnDisable에 ReturnToPool 구현여부와 중복구현 검사
            if (poolDictionary[pool.poolId].Count <= 0)
                Debug.LogError($"{pool.poolId} PoolObject의 OnDisable에 ReturnToPool이 구현되지 않았습니다");
            else if (poolDictionary[pool.poolId].Count != pool.size)
                Debug.LogError($"{pool.poolId}에 ReturnToPool이 중복됩니다");
        }
    }

    public void SpawnFromPool(string poolId, Vector3 position)
        => GetFromPool(poolId, position, Quaternion.identity);
    public void SpawnFromPool(string poolId, Vector3 position, Quaternion rotation) 
        => GetFromPool(poolId, position, rotation);

    public T SpawnFromPool<T>(string poolId, Vector3 position) where T : Component
    {
        GameObject obj = GetFromPool(poolId, position, Quaternion.identity);
        if(obj.TryGetComponent<T>(out T component))
            return component;
        else
            throw new Exception($"Pool with ID {poolId} does not contain a component of type {typeof(T)}.");
    }
    public T SpawnFromPool<T>(string poolId, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = GetFromPool(poolId, position, rotation);
        if(obj.TryGetComponent<T>(out T component))
            return component;
        else
            throw new Exception($"Pool with ID {poolId} does not contain a component of type {typeof(T)}.");
    }

    public void DespawnToPool(GameObject obj)
    {
        poolDictionary[obj.name].Enqueue(obj);
        activedObjects[obj.name].Remove(obj);

        obj.SetActive(false);
    }

    public void AllDespawnToPool()
    {
        foreach(var activedList in activedObjects.Values)
        {
            for(int i = activedList.Count - 1; i >= 0; i--)
            {
                DespawnToPool(activedList[i]);
            }
        }

        activedObjects.Clear();
    }

    GameObject GetFromPool(string poolId, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(poolId))
            throw new Exception($"Pool with ID {poolId} does not exist.");  

        Queue<GameObject> poolQueue = poolDictionary[poolId];
        if(poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.poolId == poolId);
            CreateNewObject(pool.poolId, pool.prefab);
        }

        GameObject obj = poolQueue.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);

        if(!activedObjects.ContainsKey(poolId))
        {
            activedObjects.Add(poolId, new ());
        }

        activedObjects[poolId].Add(obj);

        return obj;
    }

    GameObject CreateNewObject(string poolId, GameObject prefab)
    {
        var obj = Instantiate(prefab);
        obj.name = poolId;
        poolDictionary[poolId].Enqueue(obj);
        obj.SetActive(false);

        return obj;
    }
}
