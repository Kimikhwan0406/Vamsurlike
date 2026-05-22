using UnityEngine;

public class InGameCore
{
    Player player;
    public Player Player => player;
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
        player = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Player")).GetComponent<Player>();
        // TODO: 플레이어 초기 데이터 셋팅
    }

    public void Release()
    {
        Object.Destroy(player);
        player = null;
    }
}
