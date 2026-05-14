using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, InputActions.IPlayerActions
{
    InputActions inputActions;
    PlayerMove playerMove;

    void Awake()
    {
        inputActions = new();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();

        playerMove = GetComponent<PlayerMove>();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        playerMove.MoveInput = new Vector3(input.x, input.y, 0);
    }
}
