using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int GetPlayerXDir => inputHandler.MoveInput.x > 0 ? 1 : inputHandler.MoveInput.x < 0 ? -1 : 0;
    public Vector2 MoveInput => inputHandler.MoveInput;
    public Vector2 CureentDirection { get => currentDirectionVector; }
    public int CurrentFacingDir => currentFacingDir == 0 ? preFacingDir : currentFacingDir;

    Animator anim;
    PlayerInputHandler inputHandler;
    [SerializeField] Image healthBar;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform playerModel;
    SpriteRenderer[] spriteRenderers;

    float currentHealth;
    float maxHealth;

    float lastDamageTime;
    float invincibilityDuration = 0.5f;
    bool isInvincible = false;

    Vector2 currentDirectionVector;
    float currentAngle;

    int preFacingDir = -1;
    int currentFacingDir = -1;

    void Awake()
    {
        anim = GetComponent<Animator>();

        inputHandler = GetComponent<PlayerInputHandler>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
            return;

        lastDamageTime = GameManager.Instance.GetPlayTime();
        isInvincible = true;
        currentHealth -= damage;

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }

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
            SmoothDirection();
            Flip();
            UpdateAnimation();

            if (isInvincible)
            {
                CheckInvincible();
            }
        }
    }

    void CheckInvincible()
    {
        if (invincibilityDuration + lastDamageTime <= GameManager.Instance.GetPlayTime())
        {
            isInvincible = false;
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
        currentFacingDir = GetPlayerXDir;
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

            currentDirectionVector = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }

    void Flip()
    {
        if (preFacingDir != currentFacingDir && currentFacingDir != 0)
        {
            playerModel.localScale = new Vector3(playerModel.localScale.x * -1f, playerModel.localScale.y, playerModel.localScale.z);

            preFacingDir = currentFacingDir;
        }
    }

    void UpdateAnimation()
    {
        anim.SetBool("isMoving", inputHandler.MoveInput != Vector3.zero);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
