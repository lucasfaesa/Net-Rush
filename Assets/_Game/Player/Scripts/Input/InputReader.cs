using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Player/InputReader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event Action<Vector2> Move;
    public event Action<bool> Cut;
    public event Action<bool> Bump;

    private PlayerInputActions _playerInputActions;

    public Vector3 Direction => _playerInputActions.Player.Move.ReadValue<Vector2>();
    
    public void EnableInputActions()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.SetCallbacks(this);
        }
        
        _playerInputActions.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCut(InputAction.CallbackContext context)
    {
        Cut?.Invoke(context.ReadValueAsButton());
    }

    public void OnBump(InputAction.CallbackContext context)
    {
        Bump?.Invoke(context.ReadValueAsButton());
    }
}
