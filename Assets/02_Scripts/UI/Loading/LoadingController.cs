using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    const int maxLoadingBarPivotValue = 1372;

    [SerializeField] Image loadingBar;
    [SerializeField] Image loadingBarPivot;
    [SerializeField] TextMeshProUGUI loadingText;

    void Awake()
    {
        StartCoroutine(CoTempStart());
    }

    IEnumerator CoTempStart()
    {
        yield return new WaitForSeconds(1f);
        GameManager.UI.ShowLobbyHUD();
        Destroy(this.gameObject);
    }
}
