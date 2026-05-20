using System.IO;
using UnityEngine;

public static class Utils
{
    public static T ResourcesLoad<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // TODO: persistentDataPath로 변경하기
    public static string GetPath(string path)
    {
        return Path.Combine(Application.dataPath, path + ".json");
    }
}
