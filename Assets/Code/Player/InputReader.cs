using Pasta;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private Camera _camera;
    private Controls _controls;
    public Vector2 Movement;
    public Vector2 Aim;
    public Vector2 MouseScreenPos;
    public Vector2 MouseWorldPos;
    public Action DodgeCallback;
    public Action InteractCallback;
    public Action QuickAttackCallback;
    public Action HeavyAttackCallback;
    public Action HookCallback;
    private bool _isMouseAim = true;
    public bool _isAiming = false;

    private AutoAim _autoAim = null;

    private float _lastAttackTime = 0;
    public bool HasRecentlyAttacked => Time.timeSinceLevelLoad - _lastAttackTime < 5f;

    public bool IsMouseAim => _isMouseAim;
    public bool IsAiming => _isAiming;

    private void Awake()
    {
        _controls = new Controls();
        _camera = Camera.main;
        _autoAim = GetComponentInChildren<AutoAim>();
        Assert.IsNotNull(_autoAim);
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
        _isMouseAim = false;
    }

    private void OnMousePos(InputAction.CallbackContext obj)
    {
        _isMouseAim = true;
    }

    private void Update()
    {
        Movement = _controls.Player.Movement.ReadValue<Vector2>();
        MouseScreenPos = _controls.Player.MousePos.ReadValue<Vector2>();
        MouseWorldPos = _camera.ScreenToWorldPoint(MouseScreenPos);
        _isAiming = _controls.Player.Aim.ReadValue<Vector2>() != Vector2.zero;

        if (_isMouseAim)
        {
            Aim = MouseWorldPos - (Vector2)transform.position;
        }
        else
        {
            if (!_isAiming && _autoAim.EnemiesInRange)
            {
                Aim = _autoAim.ClosestEnemyDir();
            }
            else
            {
                Aim = _controls.Player.Aim.ReadValue<Vector2>();
            }
        }

        if (Aim == Vector2.zero)
        {
            Aim = Movement;
        }
        Aim.Normalize();

        Debug.DrawLine(transform.position, transform.position + (Vector3)Aim, Color.green);
    }

    private void OnDodge(InputAction.CallbackContext obj)
    {
        if (DodgeCallback != null)
        {
            DodgeCallback();
        }
    }
    private void OnHook(InputAction.CallbackContext obj)
    {
        if (HookCallback != null)
        {
            HookCallback();
        }
    }

    private void OnHeavyAttack(InputAction.CallbackContext obj)
    {
        if (HeavyAttackCallback != null)
        {
            HeavyAttackCallback();
        }
        _lastAttackTime = Time.time;
    }

    private void OnQuickAttack(InputAction.CallbackContext obj)
    {
        if (QuickAttackCallback != null)
        {
            QuickAttackCallback();
        }
        _lastAttackTime = Time.time;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (InteractCallback != null)
        {
            InteractCallback();
        }
    }

}
