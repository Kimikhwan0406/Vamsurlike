using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class XPDropTool : Editor
{
    [MenuItem("Tools/Add XP(5)")]
    public static void AddXP()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter>().AddExp(5);
    }
}

#endif