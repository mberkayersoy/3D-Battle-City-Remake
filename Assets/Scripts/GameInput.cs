using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    private PlayerInputActions playerInputActions;

    public event EventHandler OnPlayerMovementAction;
    public event EventHandler OnPlayerShotAction;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();


        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.Shot.performed += Shot_performed;
    }
    private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerMovementAction?.Invoke(this, EventArgs.Empty);
    }
    private void Shot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerShotAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
