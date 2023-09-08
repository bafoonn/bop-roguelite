using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private Controls _controls;
    public Vector2 movement;
    public Action DodgeCallback;
    public Action InteractCallback;
    public Action QuickAttackCallback;
    public Action HeavyAttackCallback;
    public Action HookCallback;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Dodge.performed += OnDodge;
        _controls.Player.Interact.performed += OnInteract;
        _controls.Player.QuickAttack.performed += OnQuickAttack;
        _controls.Player.HeavyAttack.performed += OnHeavyAttack;
        _controls.Player.Hook.performed += OnHook;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.Player.Dodge.performed -= OnDodge;
        _controls.Player.Interact.performed -= OnInteract;
        _controls.Player.QuickAttack.performed -= OnQuickAttack;
        _controls.Player.HeavyAttack.performed -= OnHeavyAttack;
        _controls.Player.Hook.performed -= OnHook;
        movement = Vector2.zero;
    }

    private void Update()
    {
        movement = _controls.Player.Movement.ReadValue<Vector2>();
    }

    private void OnDodge(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (DodgeCallback != null)
        {
            DodgeCallback();
        }
    }
    private void OnHook(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (HookCallback != null)
        {
            HookCallback();
        }
    }

    private void OnHeavyAttack(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (HeavyAttackCallback != null)
        {
            HeavyAttackCallback();
        }
    }

    private void OnQuickAttack(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (QuickAttackCallback != null)
        {
            QuickAttackCallback();
        }
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (InteractCallback != null)
        {
            InteractCallback();
        }
    }

}
