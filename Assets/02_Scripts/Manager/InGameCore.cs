
using UnityEngine;

public class InGameCore : MonoBehaviour
{

    void Update()
    {
        GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddTime(Time.deltaTime);
    }
}
