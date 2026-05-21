using UnityEngine;

public class InGameCore
{
    GameObject player;
    public GameObject Player => player;
    public float GetPlayTime() => GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().GetPlayTime();

    public InGameCore()
    {
        PlayerSpawn();
    }

    ~InGameCore()
    {
        Object.Destroy(player);
        player = null;
    }

    public void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddTime(Time.deltaTime);
    }

    void PlayerSpawn()
    {
        player = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Player"));
    }

    public void Release()
    {
        Object.Destroy(player);
        player = null;
    }
}
