using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInput : MonoBehaviour, InputActions.IPlayerActions
{
    InputActions inputActions;
    PlayerMove playerMove;

    EntityManager entityManager;
    Entity playerPositionEntity;

    bool isMoving;

    void Awake()
    {
        inputActions = new();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();

        playerMove = GetComponent<PlayerMove>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Start()
    {
        playerPositionEntity = entityManager.CreateEntity(typeof(PlayerPositionComponent));
        entityManager.AddComponent<PlayerTag>(playerPositionEntity);
    }

    void Update()
    {
        entityManager.SetComponentData(playerPositionEntity, new PlayerPositionComponent
        {
            Position = transform.position,
            IsMoving = isMoving
        });
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        playerMove.MoveInput = new Vector3(input.x, input.y, 0);
        isMoving = value.phase == InputActionPhase.Performed;
    }
}
