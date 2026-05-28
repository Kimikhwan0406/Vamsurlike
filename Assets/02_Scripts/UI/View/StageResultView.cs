using TMPro;
using UnityEngine;

public class StageResultView : MonoBehaviour, IView
{
    [SerializeField] GameObject gameOverGO;
    [SerializeField] GameObject resultGO;

    [Header("Top Layout")]
    [SerializeField] TextMeshProUGUI survivedText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI enemyText;

    [Header("Bottom Layout")]
    [SerializeField] Transform weaponLayout;

    public bool IsOpen => gameObject.activeSelf;

    public void OnClickGameOverQuitButton()
    {
        gameOverGO.SetActive(false);
        resultGO.SetActive(true);
    }

    public void OnClickResultOKButton()
    {
        GameManager.Instance.StageExit();
    }

    public void SetSlotParent(GameObject slotObj)
    {
        slotObj.transform.SetParent(weaponLayout);
    }
    
    public void SetTopInfo()
    {
        var hudPresenter = GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>();

        survivedText.text = (Mathf.Floor(hudPresenter.GetPlayTime() / 60)).ToString() + ":" + (hudPresenter.GetPlayTime() % 60).ToString();
        goldText.text = hudPresenter.GetGold().ToString();
        levelText.text = hudPresenter.GetLevel().ToString();
        enemyText.text = hudPresenter.GetEnemyCount().ToString();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
