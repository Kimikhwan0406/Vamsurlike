using UnityEngine;

public class InGameCore
{
    public Player Player => player;
    public float GetPlayTime() => GameManager.UI.GetPresenter<GameHUDPresenter>().GetPlayTime();


    Player player;

    string characterId;

    public InGameCore(string characterId)
    {
        this.characterId = characterId;
        PlayerSpawn();
        CameraManager.Instance.FollowPlayer(player.transform);
    }

    ~InGameCore()
    {
        Release();
    }

    public void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter>().AddTime(Time.deltaTime);
    }

    void PlayerSpawn()
    {
        player = Object.Instantiate(Utils.ResourcesLoad<GameObject>($"Player/{characterId}")).GetComponent<Player>();
        // TODO: 플레이어 초기 데이터 셋팅, 추후 캐릭터 선택 UI 에서 받아오기
        player.Init(characterId);
    }

    public void Release()
    {
        Object.Destroy(player.gameObject);
        player = null;
    }
}
