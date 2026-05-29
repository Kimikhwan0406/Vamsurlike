using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public void OnClickGameStartBtn()
    {
        GameManager.UI.OpenUI<CharacterSelectView>(new CharacterSelectModel(), new CharacterSelectPresenter());
    }
    public void OnClickAchievementBtn()
    {

    }
    public void OnClickSkillBtn()
    {

    }
    public void OnClickSettingBtn()
    {

    }
}
