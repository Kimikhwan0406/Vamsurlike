using UnityEngine;

public class InGameCore
{
    GameObject player;
    public GameObject Player => player;

    public InGameCore()
    {
        PlayerSpawn();
    }

    public void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddTime(Time.deltaTime);
    }

    void PlayerSpawn()
    {
        player = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Player"));
    }
}
