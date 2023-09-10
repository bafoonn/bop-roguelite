using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private Camera _camera;
    private Controls _controls;
    public Vector2 Movement;
    public Vector2 Aim;
    public Vector2 MousePos;
    public Action DodgeCallback;
    public Action InteractCallback;
    public Action QuickAttackCallback;
    public Action HeavyAttackCallback;
    public Action HookCallback;
    private bool mouseAim = true;

    private void Awake()
    {
        _controls = new Controls();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Dodge.performed += OnDodge;
        _controls.Player.Interact.performed += OnInteract;
        _controls.Player.QuickAttack.performed += OnQuickAttack;
        _controls.Player.HeavyAttack.performed += OnHeavyAttack;
        _controls.Player.Hook.performed += OnHook;
        _controls.Player.Aim.performed += OnAim;
        _controls.Player.MousePos.performed += OnMousePos;
    }


    private void OnDisable()
    {
        _controls.Disable();
        _controls.Player.Dodge.performed -= OnDodge;
        _controls.Player.Interact.performed -= OnInteract;
        _controls.Player.QuickAttack.performed -= OnQuickAttack;
        _controls.Player.HeavyAttack.performed -= OnHeavyAttack;
        _controls.Player.Hook.performed -= OnHook;
        _controls.Player.Aim.performed -= OnAim;
        _controls.Player.MousePos.performed -= OnMousePos;
        Movement = Vector2.zero;
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        mouseAim = false;
    }

    private void OnMousePos(InputAction.CallbackContext obj)
    {
        mouseAim = true;
    }

    private void Update()
    {
        Movement = _controls.Player.Movement.ReadValue<Vector2>();
        MousePos = _controls.Player.MousePos.ReadValue<Vector2>();

        if (mouseAim)
        {
            Vector2 target = _camera.ScreenToWorldPoint(MousePos);
            Aim = (target - (Vector2)transform.position).normalized;
        }
        else
        {
            Aim = _controls.Player.Aim.ReadValue<Vector2>();
        }

        if (Aim == Vector2.zero)
        {
            Aim = Movement.normalized;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)Aim, Color.green);
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
