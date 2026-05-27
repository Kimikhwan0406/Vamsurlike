using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDView : MonoBehaviour, IView
{
    [SerializeField] Image expBar;
    [SerializeField] TextMeshProUGUI expTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] TextMeshProUGUI timeTxt;
    [SerializeField] TextMeshProUGUI enemyCntTxt;
    [SerializeField] TextMeshProUGUI goldTxt;

    public bool IsOpen => this.gameObject.activeSelf;

    // TODO: 테스트용, 추후 게임 끝난 후 스테이지 종료 버튼으로 변경
    public void OnClickGoLobbyTest()
    {
        GameManager.UI.ShowLobbyHUD();
        GameManager.Instance.StageExit();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        Init();
    }

    #region Update View
    public void UpdateExp(float exp)
    {
        expBar.fillAmount = exp;
        expTxt.text = $"{exp*100:0.##}%";
    }

    public void UpdateLevel(int level)
    {
        levelTxt.text = $"Level {level}";
    }

    public void UpdateTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);

        timeTxt.text = $"{min:00}:{sec:00}";
    }

    public void UpdateEnemyCount(int enemyCount)
    {
        enemyCntTxt.text = enemyCount.ToString();
    }

    public void UpdateGold(int gold)
    {
        goldTxt.text = gold.ToString();
    }
    #endregion

    void Init()
    {
        expBar.fillAmount = 0f;
        expTxt.text = "0.00%";
        levelTxt.text = "Level 1";
        timeTxt.text = "00:00";
        enemyCntTxt.text = "0";
        goldTxt.text = "0";
    }
}
