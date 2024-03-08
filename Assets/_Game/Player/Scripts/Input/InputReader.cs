using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Player/InputReader")]
public class InputReader : ScriptableObject
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference cutNarrow;
    [SerializeField] private InputActionReference cutWide;
    [SerializeField] private InputActionReference cutFar;
    [SerializeField] private InputActionReference bump;
    [Space] 
    private List<InputActionReference> _allActions = new();
    
    public event Action<bool> CutNarrow;
    public event Action<bool> CutWide;
    public event Action<bool> CutFar;
    public event Action<bool> Bump;
    public event Action<bool> Jump;
    
    public Vector3 Direction()
    {
        return move.action.ReadValue<Vector2>();
    }
    
    public void EnableInputActions()
    {
        _allActions.AddRange(new InputActionReference[] { move, jump, cutNarrow, cutWide, cutFar, bump });
        _allActions.ForEach(x => x.action.Enable());
        AddListeners();
    }
    
    public void DisableInputActions()
    {
        _allActions.ForEach(x=>x.action.Disable());
        RemoveListeners();
    }

    private void AddListeners()
    {
        jump.action.performed += OnJumpPerformed;
        jump.action.canceled += OnJumpPerformed;
        cutNarrow.action.performed += OnCutNarrow;
        cutWide.action.performed += OnCutWide;
        cutFar.action.performed += OnCutFar;
        bump.action.performed += OnBump;
    }
    
    private void RemoveListeners()
    {
        jump.action.performed -= OnJumpPerformed;
        jump.action.canceled -= OnJumpPerformed;
        cutNarrow.action.performed -= OnCutNarrow;
        cutWide.action.performed -= OnCutWide;
        cutFar.action.performed -= OnCutFar;
        bump.action.performed -= OnBump;
    }

    private void OnCutNarrow(InputAction.CallbackContext callbackContext)
    {
        CutNarrow?.Invoke(callbackContext.ReadValueAsButton());
    }

    private void OnCutWide(InputAction.CallbackContext callbackContext)
    {
        CutWide?.Invoke(callbackContext.ReadValueAsButton());
    }

    private void OnCutFar(InputAction.CallbackContext callbackContext)
    {
        CutFar?.Invoke(callbackContext.ReadValueAsButton());
    }

    private void OnBump(InputAction.CallbackContext callbackContext)
    {
        Bump?.Invoke(callbackContext.ReadValueAsButton());
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext callbackContext)
    {
        Jump?.Invoke(callbackContext.ReadValueAsButton());
    }
    

}
