using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int GetPlayerXDir => inputHandler.MoveInput.x > 0 ? 1 : inputHandler.MoveInput.x < 0 ? -1 : 0;
    public Vector2 CureentDirection { get => currentDir; }

    PlayerInputHandler inputHandler;
    [SerializeField] Image healthBar;
    [SerializeField] float moveSpeed = 5f;

    float currentHealth;
    float maxHealth;
    float lastDamageTime;
    float invincibilityDuration = 0.5f;
    bool isInvincible = false;

    Vector2 currentDir;
    float currentAngle;

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
            GameManager.Instance.GameOver();
        }
    }
    void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            Move();
        }

        SmoothDirection();
    }

    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
    }

    void SmoothDirection()
    {
        var moveinput = inputHandler.MoveInput;

        if (moveinput.sqrMagnitude > 0.001f)
        {
            var targetDir = moveinput.normalized;

            float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 720f * Time.deltaTime);

            float radian = currentAngle * Mathf.Deg2Rad;

            currentDir = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }

    // RandomDropWeaponPattern의 범위 10f
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
