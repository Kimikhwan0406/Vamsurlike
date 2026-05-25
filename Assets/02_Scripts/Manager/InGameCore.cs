using UnityEngine;

public class InGameCore
{
    public Player Player => player;
    public float GetPlayTime() => GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().GetPlayTime();


    Player player;

    string characterId;

    public InGameCore(string characterId)
    {
        this.characterId = characterId;
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
        player = Object.Instantiate(Utils.ResourcesLoad<GameObject>($"Player/{characterId}")).GetComponent<Player>();
        // TODO: 플레이어 초기 데이터 셋팅, 추후 캐릭터 선택 UI 에서 받아오기
        player.Init(characterId);
    }

    public void Release()
    {
        Object.Destroy(player);
        player = null;
    }
}
