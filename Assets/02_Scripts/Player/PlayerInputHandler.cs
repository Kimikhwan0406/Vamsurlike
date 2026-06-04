using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputActions.IPlayerActions
{
    InputActions inputSystem;
    public Vector3 MoveInput { get; private set; }
    public Vector3 PreInput { get; private set; } = Vector3.right;

    void Awake()
    {
        inputSystem = new InputActions();
        inputSystem.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        inputSystem.Player.Enable();
    }

    void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        if(context.started)
        {
            PreInput = context.ReadValue<Vector2>();
        }
    }
}
