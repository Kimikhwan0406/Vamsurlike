using UnityEngine;

public class FieldObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite[] sprits;
    float xp;

    public void Init(float xp)
    {
        this.xp = xp;

        if(xp == 1)
        {
            sprite.sprite = sprits[0];
        }
        else if (xp >= 5)
        {
            sprite.sprite = sprits[1];
        }
        else if (xp >= 10)
        {
            sprite.sprite = sprits[2];
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.UI.GetPresenter<GameHUDPresenter, GameHUDView>().AddExp(xp);
            GameManager.Pool.ReturnFieldObject(PoolType.FieldObject, gameObject);
        }
    }
}
