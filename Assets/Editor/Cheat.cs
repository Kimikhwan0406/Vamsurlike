using UnityEditor;

#if UNITY_EDITOR

public class Cheat : Editor
{
    [MenuItem("Tools/Add XP(5)")]
    public static void AddXP()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter>().AddExp(5);
    }

    [MenuItem("Tools/Player Dead")]
    public static void PlayerDead()
    {
        GameManager.Instance.GameOver();
    }
}

#endif