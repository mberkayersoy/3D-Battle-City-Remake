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
    public event EventHandler OnMobileMovementAction;
    public event EventHandler OnMobileShotAction;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Mobile.Enable();


        playerInputActions.Mobile.Movement.performed += Movement_performed1;
        playerInputActions.Player.Movement.performed += Movement_performed;
        playerInputActions.Player.Shot.performed += Shot_performed;
        playerInputActions.Mobile.Fire.performed += Fire_performed;
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMobileShotAction?.Invoke(this, EventArgs.Empty);
    }

    private void Movement_performed1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMobileMovementAction?.Invoke(this, EventArgs.Empty);
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

    public Vector2 GetMobileMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Mobile.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
