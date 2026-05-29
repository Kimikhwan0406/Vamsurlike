using UnityEngine;

public class LevelUpView : MonoBehaviour, IView
{
    public GameObject LevelUpSlotPrefab;
    public Transform LevelUpSlotGroupParent;

    public bool IsOpen => this.gameObject.activeSelf;

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }
}
