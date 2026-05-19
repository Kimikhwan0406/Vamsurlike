using UnityEngine;

public static class Utils
{
    public static T ResourcesLoad<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
