using System.Collections.Generic;
using UnityEngine;

public class FieldObjectPool
{
    GameObject expPrefab;
    Transform poolTransform;

    List<FieldObject> fieldObjectPool = new();

    public void Init(Transform _poolTransform)
    {
        poolTransform = _poolTransform;
        expPrefab = Utils.ResourcesLoad<GameObject>("XP");

        for (int i = 0; i < 60; i++)
        {
            CreateFieldObject();
        }
    }

    public FieldObject CreateFieldObject()
    {
        var newObj = Object.Instantiate(expPrefab, poolTransform).GetComponent<FieldObject>();
        newObj.gameObject.SetActive(false);
        fieldObjectPool.Add(newObj);

        return newObj;
    }

    public FieldObject GetFieldObject(Transform transform, float xp)
    {
        FieldObject fieldObj = null;

        foreach(var item in fieldObjectPool)
        {
            if(!item.gameObject.activeSelf)
            {
                fieldObj = item;
                break;
            }
        }

        if (null == fieldObj)
        {
            fieldObj = CreateFieldObject();
        }

        fieldObj.gameObject.SetActive(true);
        fieldObj.transform.SetParent(null);
        fieldObj.transform.position = transform.position;
        fieldObj.Init(xp);

        return fieldObj;
    }

    public void ReturnFieldObject(FieldObject fieldObj)
    {
        fieldObj.gameObject.SetActive(false);
        fieldObj.transform.SetParent(poolTransform);
    }
}
