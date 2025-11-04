
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private InputActions inputActions;

    private void Awake()
    {
        instance = this;

        inputActions = new InputActions();
        inputActions.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
    public bool IsUpActionPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }
    public bool IsLeftActionPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }
    public Vector2 GetMovementInputVector2()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
    public bool IsUiActionPressed()
    {
        return inputActions.UI.ClickButton.IsPressed();
    }
}
