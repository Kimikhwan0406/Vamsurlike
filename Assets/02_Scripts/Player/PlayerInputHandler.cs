using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInputHandler : MonoBehaviour, InputActions.IPlayerActions
{
    InputActions inputSystem;
    public Vector3 MoveInput;

    void Awake()
    {
        inputSystem = new InputActions();
        inputSystem.Player.SetCallbacks(this);
        inputSystem.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
