using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInputHandler inputHandler;
    [SerializeField] float moveSpeed = 5f;

    float currentHealth;
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
        currentHealth = characterData.MaxHealth;
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
    }

    void Move()
    {
        transform.position += inputHandler.MoveInput * Time.deltaTime * moveSpeed;
    }
}
