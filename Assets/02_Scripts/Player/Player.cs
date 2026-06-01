using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int GetPlayerXDir => inputHandler.MoveInput.x > 0 ? 1 : inputHandler.MoveInput.x < 0 ? -1 : 0;
    public Vector2 GetPlayerDir => inputHandler.MoveInput.normalized;

    PlayerInputHandler inputHandler;
    [SerializeField] Image healthBar;
    [SerializeField] float moveSpeed = 5f;

    float currentHealth;
    float maxHealth;
    float lastDamageTime;
    float invincibilityDuration = 0.5f;
    bool isInvincible = false;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    public void Init(string characterId)
    {
        CharacterData characterData = GameManager.DataTable.GetCharacterData(characterId);
        healthBar.fillAmount = 1f;
        maxHealth = characterData.MaxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            if (invincibilityDuration + lastDamageTime <= GameManager.Instance.GetPlayTime())
                isInvincible = false;
            else
                return;
        }

        lastDamageTime = GameManager.Instance.GetPlayTime();
        isInvincible = true;
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f)
        {
            Debug.Log("Player is dead.");
        }
    }
    void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
    }

    // RandomDropWeaponPattern의 범위 10f
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
