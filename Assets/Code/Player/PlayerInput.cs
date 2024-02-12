using Pasta;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Controls.PlayerActions _actions;
    public Vector2 Movement;
    public Vector2 Aim;
    public Vector2 MouseScreenPosition;
    public Vector2 MouseWorldPosition;
    //public bool DoDodge;
    public bool DoQuickAttack;
    public bool DoHeavyAttack;
    private Action _dodgeCallback;

    private bool _isMouseAim = true;
    private bool _isAiming = false;

    private AutoAim _autoAim = null;

    private float _lastAttackTime = 0;
    public bool HasRecentlyAttacked => Time.timeSinceLevelLoad - _lastAttackTime < 5f;

    public bool IsMouseAim => _isMouseAim;
    public bool IsAiming => _isAiming;
    private IPlayer _player = null;

    private void Awake()
    {
        _actions = InputReader.Current.GetPlayerActions();
        _camera = Camera.main;
        _autoAim = GetComponentInChildren<AutoAim>();
        Assert.IsNotNull(_autoAim);
    }

    public void Setup(IPlayer player, Action dodgeCallback)
    {
        _player = player;
        _dodgeCallback = dodgeCallback;
        HUD.OnOpenWindow += OnOpenWindow;
        HUD.OnCloseWindow += OnCloseWindow;
    }

    private void OnCloseWindow()
    {
        enabled = true;
    }

    private void OnOpenWindow(HUDWindow obj)
    {
        enabled = false;
    }

    private void OnEnable()
    {
        _actions.Enable();
        _actions.Dodge.performed += OnDodge;
        _actions.Aim.performed += OnAim;
        _actions.MousePos.performed += OnMousePos;
    }


    private void OnDisable()
    {
        _actions.Disable();
        _actions.Dodge.performed -= OnDodge;
        _actions.Aim.performed -= OnAim;
        _actions.MousePos.performed -= OnMousePos;
        Movement = Vector2.zero;
    }

    private void OnDestroy()
    {
        HUD.OnOpenWindow -= OnOpenWindow;
        HUD.OnCloseWindow -= OnCloseWindow;
    }

    private void Update()
    {
        Movement = _actions.Movement.ReadValue<Vector2>();
        MouseScreenPosition = _actions.MousePos.ReadValue<Vector2>();
        MouseWorldPosition = _camera.ScreenToWorldPoint(MouseScreenPosition);
        _isAiming = _actions.Aim.ReadValue<Vector2>() != Vector2.zero;
        //DoDodge = _actions.Dodge.IsPressed();
        DoQuickAttack = _actions.QuickAttack.IsPressed();
        DoHeavyAttack = _actions.HeavyAttack.IsPressed();

        if (_isMouseAim)
        {
            SetAim(MouseWorldPosition - (Vector2)transform.position);
        }
        else
        {
            if (_isAiming) // If player is actively aiming aim in that direction
            {
                SetAim(_actions.Aim.ReadValue<Vector2>());
            }
            else if (_autoAim.EnemiesInRange) // If player is not aiming but there are enemies nearby, aim at them
            {
                SetAim(_autoAim.ClosestEnemyDir());
            }
            else // Aim at movement direction otherwise
            {
                SetAim(Movement);
            }
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)Aim, Color.green);
    }

    private void SetAim(Vector2 dir)
    {
        dir.Normalize();
        if (dir == Vector2.zero) return;
        Aim = dir;
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        _isMouseAim = false;
    }

    private void OnMousePos(InputAction.CallbackContext obj)
    {
        _isMouseAim = true;
    }

    private void OnDodge(InputAction.CallbackContext obj)
    {
        if (_dodgeCallback == null) return;
        _dodgeCallback();
    }

    public void SetDodge(Action dodgeCallback) => _dodgeCallback = dodgeCallback;
}
