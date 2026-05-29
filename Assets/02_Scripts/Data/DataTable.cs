using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class DataTable
{
    Dictionary<string, CharacterData> CharacterDataTable { get; set; } = new();
    Dictionary<string, EnemyData> EnemyDataTable { get; set; } = new();
    Dictionary<string, StageData> StageDataTable { get; set; } = new();
    Dictionary<string, WeaponData> WeaponDataTable { get; set; } = new();
    Dictionary<string, WeaponLevelData> WeaponLevelDataTable { get;  set; } = new();

    public Dictionary<string, CharacterData> GetCharacterDataTable() => CharacterDataTable;
    public Dictionary<string, WeaponData> GetWeaponDataTable() => WeaponDataTable;



    [Serializable]
    class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public void LoadAllData()
    {
        CharacterDataTable = LoadData<CharacterData>("Character");
        EnemyDataTable = LoadData<EnemyData>("Enemy");
        StageDataTable = LoadData<StageData>("BaseStage");
        WeaponDataTable = LoadData<WeaponData>("WeaponMy");
        WeaponLevelDataTable = LoadData<WeaponLevelData>("WeaponLevelMy");
    }

    Dictionary<string, T> LoadData<T>(string tableNmae) where T : BaseData
    {
        string resourcePath = $"JsonOutput/{tableNmae}";
        TextAsset textAsset = Utils.ResourcesLoad<TextAsset>(resourcePath);
        if(null == textAsset)
        {
            Debug.LogError($"리소스를 찾을 수 없습니다: Resources/{resourcePath}");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = textAsset.text;

            string wrappedJson = "{\"items\":" + jsonString + "}";

            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper == null || wrapper.items == null)
            {
                Debug.LogError($"[{typeof(T).Name}] JSON 파싱 결과가 비어 있습니다.");
            }
            
            if (null != wrapper && null != wrapper.items)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.items.Count}개 로드했습니다.");
                return wrapper.items.ToDictionary(value => value.Id.ToString());
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }

    #region Getters

    public CharacterData GetCharacterData(string id)
    {
        if (null == CharacterDataTable || string.IsNullOrEmpty(id)) return null;

        return CharacterDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public EnemyData GetEnemyData(string id)
    {
        if (null == EnemyDataTable || string.IsNullOrEmpty(id)) return null;

        return EnemyDataTable.TryGetValue(id, out var data) ? data : null;
    }

    /// <summary>
    /// ID는 (현재 분 + 1) + 269,000 형식으로 구성
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public StageData GetStageData(string id)
    {
        if (null == StageDataTable || string.IsNullOrEmpty(id)) return null;

        return StageDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public WeaponData GetWeaponData(string id)
    {
        if (null == WeaponDataTable || string.IsNullOrEmpty(id)) return null;

        return WeaponDataTable.TryGetValue(id, out var data) ? data : null;
    }

    public WeaponLevelData GetWeaponLevelData(string id)
    {
        if (null == WeaponLevelDataTable || string.IsNullOrEmpty(id)) return null;

        return WeaponLevelDataTable.TryGetValue(id, out var data) ? data : null;
    }

    #endregion
}
