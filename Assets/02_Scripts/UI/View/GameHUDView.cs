using TMPro;
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

    public void OnClickGoLobbyTest()
    {
        GameManager.UI.ShowLobbyHUD();
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
        // TODO: maxEXP로 나누기
        expBar.fillAmount = exp;
        expTxt.text = $"{exp:0.##}%";
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
