using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager
{
    // TODO 데이터 간 의존성이 존재하여 데이터의 로드 및 저장 순서가 필요하다면 List가 필요할 수 있다.
    Dictionary<Type, IUserData> userDataCache = new();


    // TODO: 특정 데이터에 대한 저장은 GetData로 데이터를 가져와서 데이터를 수정 후 저장을 할 것이다.
    // 이 때 해당 데이터 클래스 내부에서 저장이 이루어지도록 한다.

    public void SaveAllData()
    {
        foreach (var data in userDataCache.Values)
        {
            data.SaveData();
        }
    }

    public void LoadAllData()
    {
        foreach (var data in userDataCache.Values)
        {
            data.LoadData();
        }
    }

    public void SetAllDefaultData()
    {
        foreach (var data in userDataCache.Values)
        {
            data.SetDefaultData();
        }
    }

    public BaseUserData GetData<T>() where T : class, IUserData
    {
        if(userDataCache.TryGetValue(typeof(T), out var data))
        {
            return data.GetData();
        }


        Debug.LogError($"UserData of type {typeof(T)} not found in cache.");
        return null;
    }
}
