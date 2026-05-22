using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputHandler inputHandler;
    [SerializeField] float moveSpeed = 5f;

    float currentHealth;
    float lastDamageTime;
    float invincibilityDuration = 0.5f;
    bool isInvincible = false;
    

    public void Init(float _currentHealth)
    {
        currentHealth = _currentHealth;
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

        if(currentHealth <= 0f)
        {
            Debug.Log("Player is dead.");
        }
    }
    void Update()
    {
        Move();
        OnAutoSkill();
    }

    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
    }

    // TODO: 여기에 스킬 추가
    void OnAutoSkill()
    {

    }
}
