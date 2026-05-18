using UnityEngine;

public class InGameCore : MonoBehaviour
{
    public GameObject player;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddTime(Time.deltaTime);
    }

    void Init()
    {
        PlayerSpawn();
    }

    void PlayerSpawn()
    {
        player = Instantiate(Resources.Load<GameObject>("Player"));
    }
}
