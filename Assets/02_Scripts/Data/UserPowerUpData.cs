using System.IO;
using UnityEngine;

public class UserPowerUpData : BaseUserData
{
    PowerUpData userPowerUpData = new ();
    public BaseUserData GetData() => userPowerUpData;

    public void SaveData()
    {
        string path = Utils.GetPath(GetType().ToString());
        string json = JsonUtility.ToJson(userPowerUpData, true);
        File.WriteAllText(path, json);
        Debug.Log($"저장 완료: {path}");
    }

    public void LoadData()
    {
        string path = Utils.GetPath(GetType().ToString());
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userPowerUpData = JsonUtility.FromJson<PowerUpData>(json);
        }
        else
        {
            Debug.LogWarning($"저장된 데이터가 없습니다: {path}");
            SetDefaultData();
        }
    }

    public void SetDefaultData()
    {
        userPowerUpData = new();
        SaveData();
    }
}
