using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance {  get; private set; }

    private PlayerInputAction _playerInputAction;

    public event EventHandler OnPlayerInputAttack1;
    public event EventHandler OnPlayerInputAttack2;

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();
        _playerInputAction.Fighting.Attack01.started += PlayerAttack01_started;
        _playerInputAction.Fighting.Attack02.started += PlayerAttack02_started;
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePositions()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;
    }

    public void DisablePlayerInput()
    {
        _playerInputAction.Disable();
    }

    private void PlayerAttack01_started(InputAction.CallbackContext obj)
    {
        OnPlayerInputAttack1?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerAttack02_started(InputAction.CallbackContext obj)
    {
        OnPlayerInputAttack2?.Invoke(this, EventArgs.Empty);
    }
}
