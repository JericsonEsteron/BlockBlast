using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IInputActions, IInputReader
{
    public Action<bool> OnClicked;

    public PlayerInput inputActions;

    public Vector2 PointerDelta => inputActions.Input.Delta.ReadValue<Vector2>();

    public void DisableInputControl()
    {
        if(inputActions == null) return;
        
        inputActions.Disable();
    }

    public void EnableInputControl()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerInput();
            inputActions.Input.SetCallbacks(this);
        }
        inputActions.Enable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                OnClicked?.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                OnClicked?.Invoke(false);
                break;
        }
    }

    public void OnDelta(InputAction.CallbackContext context)
    {
    }
}

